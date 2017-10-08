// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vending.Machine.Abstraction;
using Vending.Machine.Abstraction.Models;
using Vending.Machine.Console.Abstract;

namespace Vending.Machine.Console
{
    /// <summary>
    /// Defines the vending machine state.
    /// </summary>
    public enum VendingMachineState
    {
        TurnedOff,
        OutOfStock,
        Order,
        Payment,
        Fault
    }
    /// <summary>
    /// Implement the vending machine. 
    /// </summary>
    public class VendingMachineController : IVendingMachineController
    {
        TaskCompletionSource<int> _complete = new TaskCompletionSource<int>();
        readonly IDisplayPanel _displayPanel;
        readonly IPaymentReceiver _coinReceiver;
        readonly IOrderPanel _orderPurchasePanel;
        readonly IVendingMessageRepository _vendingMessageRepository;
        readonly IProductRepository _productRepository;
        readonly IWalletRepository _walletRepository;
        private ISoldRecord _saleRecord;

        public VendingMachineState VendingMachineState { get; set; }

        /// <summary>
        /// The vending controller constructor.
        /// </summary>
        /// <param name="displayPanel">The display panel</param>
        /// <param name="coinReceiver">The payment module</param>
        /// <param name="orderPurchasePanel">The order module.</param>
        /// <param name="productRepository">The product repository.</param>
        /// <param name="walletRepository">The wallet repository.</param>
        /// <param name="saleRecord">Sold product logs</param>
        /// <param name="vendingMessageRepository">The messages repository.</param>
        public VendingMachineController(IDisplayPanel displayPanel,
            IPaymentReceiver coinReceiver,
            IOrderPanel orderPurchasePanel,
            IProductRepository productRepository,
            IWalletRepository walletRepository,
            ISoldRecord saleRecord,
            IVendingMessageRepository vendingMessageRepository)
        {
            _vendingMessageRepository = vendingMessageRepository;
            _displayPanel = displayPanel;
            _coinReceiver = coinReceiver;
            _coinReceiver.CoinAction += PaymentAction;
            _coinReceiver.FailtException += FailtException;
            _productRepository = productRepository;
            _walletRepository = walletRepository;
            _orderPurchasePanel = orderPurchasePanel;
            _orderPurchasePanel.OrderAction += OrderAction;
            _orderPurchasePanel.FailtException += FailtException;
            _saleRecord = saleRecord;
        }

        #region IVendingMachineController interface

        public Task PowerOffDone => _complete.Task;

        void FailtException(Exception ex)
        {
            VendingMachineState = VendingMachineState.Fault;
            _orderPurchasePanel.Off();
            _coinReceiver.Off();
            _complete.TrySetException(ex);
        }

        public void Off()
        {
            VendingMachineState = VendingMachineState.TurnedOff;
            DisplayMessageByCode(MessageCode.OutOfServise);
            _orderPurchasePanel.Off();
            _coinReceiver.Off();
            _complete.TrySetResult(0);
        }
        
        public void On()
        {
            TryToStartSelling();
        }

        #endregion

        private void TryToStartSelling()
        {
            if (_productRepository.ProductList.Any((p) => _productRepository.CountProduct(p.Code) == 0))
            {
                VendingMachineState = VendingMachineState.OutOfStock;
                DisplayMessageByCode(MessageCode.OutOfStock);
            }
            else
            {
                VendingMachineState = VendingMachineState.Order;
                DisplayMessageByCode(MessageCode.ReadyToService);
                _orderPurchasePanel.On();
            }
        }

        readonly PaymentWalletRepository _paymentWalletRepository = new PaymentWalletRepository();

        /// <summary>
        /// Processing payment events from the coin receiver
        /// </summary>
        /// <param name="cmd">The event to process.</param>
        /// <param name="payment">The coin received or null</param>
        void PaymentAction(PaymentCmdEvent cmd, INotionValue payment)
        {
            if (VendingMachineState != VendingMachineState.Payment)
            {
                return;
            }
            if (cmd == PaymentCmdEvent.Cancel)
            {
                // Cancellation of the order or/and payment 
                CancelOrder();
            }
            else if (cmd == PaymentCmdEvent.Payment)
            {
                _paymentWalletRepository.AddToWallet(payment, 1);
                Money total = _orderedProducts.Aggregate(Money.Zero, (m, p) => m + p.Price);
                Money paid = _paymentWalletRepository.WalletList.Aggregate(Money.Zero, (m, p) => m + new Money(0, (int)p.Item1.Nominal * p.Item2));
                string msg = String.Format(_vendingMessageRepository.GetMessage(MessageCode.BalancePayment), total, paid);
                _displayPanel.DisplayMessage(msg);
                if (paid >= total)
                {
                    // give change or cancel
                    if (paid > total)
                    {
                        Money change = paid - total;
                        PaymentWalletRepository collectedChange = ChangeComposer(_walletRepository, change);
                        if (collectedChange == null)
                        {
                            DisplayMessageByCode(MessageCode.RunOutOfChange);
                            CancelOrder();
                            return;
                        }
                        foreach (var pm in collectedChange.WalletList)
                        {
                            msg = String.Format(_vendingMessageRepository.GetMessage(MessageCode.GivenChange), pm.Item1.MoneyValue, pm.Item2);
                            _displayPanel.DisplayMessage(msg);
                        }

                    }
                    // complete purchase
                    foreach (var p in _orderedProducts)
                    {
                        msg = String.Format(_vendingMessageRepository.GetMessage(MessageCode.CollectYourPurchase), p);
                        _saleRecord.Sold(p);
                        _displayPanel.DisplayMessage(msg);
                    }

                    _orderedProducts.Clear();
                    foreach (var pm in _paymentWalletRepository.WalletList)
                    {
                        _walletRepository.AddToWallet(pm.Item1, pm.Item2);
                    }

                    _paymentWalletRepository.ClearMoney();
                    // switch back to selling
                    TryToStartSelling();
                    _coinReceiver.Off();
                }
            }
        }

        private void CancelOrder()
        {
            VendingMachineState = VendingMachineState.Order;

            // Display cancel message 
            DisplayMessageByCode(MessageCode.OrderCancel);

            // Return paid money
            foreach (var pm in _paymentWalletRepository.WalletList)
            {
                var msg = String.Format(_vendingMessageRepository.GetMessage(MessageCode.ReturnPayment), pm.Item1.MoneyValue,
                    pm.Item2);
                _displayPanel.DisplayMessage(msg);
            }
            _paymentWalletRepository.ClearMoney();

            // Return reserved stock of product
            foreach (var p in _orderedProducts)
            {
                _productRepository.AddToStock(p.Code, 1);
            }
            _orderedProducts.Clear();

            _coinReceiver.Off();
            TryToStartSelling();
        }

        Stack<Product> _orderedProducts = new Stack<Product>();

        /// <summary>
        /// Process the order actions
        /// </summary>
        /// <param name="cmd">The order command.</param>
        /// <param name="obj">The selected type of product.</param>
        void OrderAction(OrderCmdEvent cmd, Product obj)
        {
            if (VendingMachineState != VendingMachineState.Order)
            {
                return;
            }
            if (cmd == OrderCmdEvent.Select)
            {
                if (_productRepository.CountProduct(obj.Code) > 0)
                {
                    _orderedProducts.Push(obj);
                    string msg = String.Format(_vendingMessageRepository.GetMessage(MessageCode.Checkout), obj);
                    _displayPanel.DisplayMessage(msg);
                    _productRepository.RemoveSellFromStock(obj.Code);
                    VendingMachineState = VendingMachineState.Payment;
                    _orderPurchasePanel.Off();
                    _coinReceiver.On();
                }
            }
            else if (cmd == OrderCmdEvent.OutOfStock)
            {
                DisplayMessageByCode(MessageCode.OutOfStock);
                _orderPurchasePanel.Off();
                VendingMachineState = VendingMachineState.OutOfStock;
            }
        }

        private void DisplayMessageByCode(MessageCode messageCode)
        {
            _displayPanel.DisplayMessage(_vendingMessageRepository.GetMessage(messageCode));
        }

        /// <summary>
        /// Change composer, based on "greedy algorithm".
        /// </summary>
        /// <param name="walletRepository">The source wallet of the change</param>
        /// <param name="changeValue">The sum of change to compose.</param>
        /// <returns>The change wallet with composed change.</returns>
        public static PaymentWalletRepository ChangeComposer(IWalletRepository walletRepository, Money changeValue)
        {
            // Collect change 
            PaymentWalletRepository changeCollected = new PaymentWalletRepository();

            // Collect the change for big to small value
            foreach (var position in walletRepository.WalletList.OrderByDescending(w => w.Item1.Nominal).Where(p => p.Item2 > 0))
            {
                (INotionValue money, int count) = position;
                while (changeValue >= money.MoneyValue && count > 0)
                {
                    count--;
                    changeValue -= money.MoneyValue;
                    changeCollected.AddToWallet(money, 1);
                    walletRepository.RemoveFromWallet(money, 1);
                }
                if (changeValue == Money.Zero)
                {
                    break;
                }
            }

            //Cannot get the change
            if (changeValue != Money.Zero)
            {
                // return all money back
                foreach (var mc in changeCollected.WalletList)
                {
                    walletRepository.AddToWallet(mc.Item1, mc.Item2);
                }
                return null;
            }

            return changeCollected;
        }
    }
}

