// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.


using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vending.Machine.Abstraction;
using Vending.Machine.Console.Abstract;

namespace Vending.Machine.Console
{

    /// <summary>
    /// Implements the product ordering panel.
    /// </summary>
    public class OrderPanel : IOrderPanel
    {
        /// <summary>
        /// Gets or sets status flag.
        /// </summary>
        public bool IsStateOn { get; set; }

        private readonly IDisplayPanel _displayPanel;
        private readonly IReadKeypadInput _readKeypadInput;
        readonly IVendingMessageRepository _vendingMessageRepository;
        readonly IProductRepository _productRepository;
        CancellationTokenSource _cancellationTokenSource;
        CancellationToken _cancellationToken;
        Task _readKeypadTask;

        /// <summary>
        /// Constructs the class
        /// </summary>
        /// <param name="displayPanel">The display panel.</param>
        /// <param name="readKeypadInput">The key pad input.</param>
        /// <param name="productRepository">The product repository.</param>
        /// <param name="vendingMessageRepository">The message repository.</param>
        public OrderPanel(IDisplayPanel displayPanel, IReadKeypadInput readKeypadInput,
                                        IProductRepository productRepository, IVendingMessageRepository vendingMessageRepository)
        {
            _vendingMessageRepository = vendingMessageRepository;
            _displayPanel = displayPanel;
            _productRepository = productRepository;
            _readKeypadInput = readKeypadInput;
        }

        #region IOrderPanel interface

        public event OrderAction OrderAction;

        public event FailtException FailtException;

        public void Off()
        {
            _cancellationTokenSource?.Cancel();
            IsStateOn = false;
            _readKeypadTask = null;
        }

        public void On()
        {
            try
            {
                IsStateOn = true;
                // verify stock 
                if (IsAnyStock())
                {
                    // Activate key pad reader
                    _cancellationTokenSource = new CancellationTokenSource();
                    _cancellationToken = _cancellationTokenSource.Token;
                    _readKeypadTask = Task.Run((Action)ReadKeypadInstructions, _cancellationToken);
                }
                else
                {
                    // signal nothing to sell
                    OrderAction?.Invoke(OrderCmdEvent.OutOfStock, null);
                    IsStateOn = false;
                }
            }
            catch (Exception e)
            {
                FailtException?.Invoke(e);
            }
        }

        #endregion

        /// <summary>
        /// Verify is any stock to sell.
        /// </summary>
        private bool IsAnyStock()
        {
            return _productRepository.ProductList.Any((p) => _productRepository.CountProduct(p.Code) > 0);
        }

        /// <summary>
        /// Display the stock available to sell.
        /// </summary>
        private void DisplayProducts()
        {
            DisplayMessageByCode(MessageCode.MakeYourOrder);
            var list = _productRepository.ProductList;
            foreach (var item in list)
            {
                if (_productRepository.CountProduct(item.Code) > 0)
                {
                    string template = _vendingMessageRepository.GetMessage(MessageCode.SelectOrderLine);
                    if (!string.IsNullOrWhiteSpace(template))
                    {
                        string msg = string.Format(template, item.Code, item.DisplayName, item.Price);
                        _displayPanel.DisplayMessage(msg);
                    }
                }
            }
        }

        /// <summary>
        /// Communicate with keypad.
        /// </summary>
        private void ReadKeypadInstructions()
        {
            try
            {
                do
                {
                    // verify stock 
                    if (IsAnyStock())
                    {
                        DisplayProducts();
                    }
                    else
                    {
                        // Signal nothing to sell
                        OrderAction?.Invoke(OrderCmdEvent.OutOfStock, null);
                        IsStateOn = false;
                        continue;
                    }

                    DisplayMessageByCode(MessageCode.SelectOrder);

                    char code = _readKeypadInput.ReadInput(_cancellationToken);

                    // check the product code
                    if (_productRepository.ProductList.Any((p) => p.Code == code))
                    {
                        // valid code
                        var product = _productRepository.ProductList.First((p) => p.Code == code);
                        DisplayMessageByCode(MessageCode.Ok);
                        // select it
                        OrderAction?.Invoke(OrderCmdEvent.Select, product);
                    }
                    else
                    {
                        // inform - invalid code
                        DisplayMessageByCode(MessageCode.InvalidInput);
                    }
                }
                while (IsStateOn);
            }
            catch (Exception e)
            {
                FailtException?.Invoke(e);
            }
        }

        /// <summary>
        /// Display message from message repository.
        /// </summary>
        /// <param name="messageCode">THe message code.</param>
        private void DisplayMessageByCode(MessageCode messageCode)
        {
            _displayPanel.DisplayMessage(_vendingMessageRepository.GetMessage(messageCode));
        }

    }
}

