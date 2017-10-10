using System;
using System.Collections.Generic;
using System.Linq;
using Vending.Machine.Abstraction;
using Vending.Machine.Abstraction.Models;
using Vending.Machine.Console;
using Vending.Machine.Console.Abstract;
using Vending.Machine.Test.Mock;
using Xunit;

namespace Vending.Machine.Test
{
    public class VendingMachineControllerUnitTest
    {
        private Product PrdA = new Product() { Code = 'A', Price = new Money(0, 35), DisplayName = "Name-A" };
        private Product PrdB = new Product() { Code = 'B', Price = new Money(1, 0), DisplayName = "Name-B" };

        void InitProductRepository(ProductRepository productRepository, int a, int b)
        {
            productRepository.RegisterOrUpdateProduct(PrdA);
            productRepository.RegisterOrUpdateProduct(PrdB);
            productRepository.AddToStock(PrdA.Code, a);
            productRepository.AddToStock(PrdB.Code, b);
        }

        [Fact]
        public void TestOn()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);
            InitProductRepository(productRepository, 1, 1);

            // Test subject Action - on
            vendingMachineController.On();

            // verify - on
            Assert.True(orderPanel.OnCalled);
            Assert.Equal(VendingMachineState.Order, vendingMachineController.VendingMachineState);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.ReadyToService
            };
            Assert.Equal((IEnumerable<MessageCode>)TestCatchedCodes, (IEnumerable<MessageCode>)messageRepository.CatchedCodes);
            Assert.Equal((IEnumerable<string>)messageRepository.ReturnList, (IEnumerable<string>)displayPanel.DisplayList);
        }

        [Fact]
        public void TestOff()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);

            // Test subject Action - off
            vendingMachineController.Off();

            // verify - off
            Assert.True(orderPanel.OffCalled);
            Assert.True(paymentReceiver.OffCalled);

            Assert.Equal(VendingMachineState.TurnedOff, vendingMachineController.VendingMachineState);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.OutOfServise
            };
            Assert.Equal((IEnumerable<MessageCode>)TestCatchedCodes, (IEnumerable<MessageCode>)messageRepository.CatchedCodes);
            Assert.Equal((IEnumerable<string>)messageRepository.ReturnList, (IEnumerable<string>)displayPanel.DisplayList);
        }

        [Fact]
        public void TestFaultExceptionEvent1()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);

            // Test subject Action - Exception orderPanel
            orderPanel.InvokeFailtException(new Exception());

            // verify - Exception orderPanel
            Assert.True(orderPanel.OffCalled);
            Assert.True(paymentReceiver.OffCalled);
            Assert.Equal(VendingMachineState.Fault, vendingMachineController.VendingMachineState);
        }

        [Fact]
        public void TestFaultExceptionEvent2()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);

            // Test subject Action - Exception payment receiver
            paymentReceiver.InvokeFailtException(new Exception());

            // verify - Exception payment receiver
            Assert.True(orderPanel.OffCalled);
            Assert.True(paymentReceiver.OffCalled);
            Assert.Equal(VendingMachineState.Fault, vendingMachineController.VendingMachineState);
        }


        [Fact]
        public void TestSelectOrderFromOrderPanel()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);
            InitProductRepository(productRepository, 1, 1);
            // !!! setup order state
            vendingMachineController.VendingMachineState = VendingMachineState.Order;

            // Test subject Action - select order as product A
            orderPanel.InvokeOrderAction(OrderCmdEvent.Select, PrdA);

            Assert.Equal(VendingMachineState.Payment, vendingMachineController.VendingMachineState);

            // verify - select order as product A
            Assert.True(orderPanel.OffCalled);
            Assert.True(paymentReceiver.OnCalled);
            Assert.Equal(0, productRepository.CountProduct(PrdA.Code));

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.Checkout
            };
            Assert.Equal(TestCatchedCodes, messageRepository.CatchedCodes);
            Assert.Equal(messageRepository.ReturnList, displayPanel.DisplayList);
        }

        [Fact]
        public void TestOutOfStockFromOrderPanel()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);
            InitProductRepository(productRepository, 0, 0);
            // !!! setup order state
            vendingMachineController.VendingMachineState = VendingMachineState.Order;

            // Test subject Action - out of stock
            orderPanel.InvokeOrderAction(OrderCmdEvent.OutOfStock, null);

            Assert.Equal(VendingMachineState.OutOfStock, vendingMachineController.VendingMachineState);

            // verify - out of stock
            Assert.True(orderPanel.OffCalled);
            Assert.Equal(0, productRepository.CountProduct(PrdA.Code));

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.OutOfStock
            };
            Assert.Equal((IEnumerable<MessageCode>)TestCatchedCodes, (IEnumerable<MessageCode>)messageRepository.CatchedCodes);
            Assert.Equal((IEnumerable<string>)messageRepository.ReturnList, (IEnumerable<string>)displayPanel.DisplayList);
        }


        [Fact]
        public void TestOrderPaymentExactFromOrderPanel()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);
            InitProductRepository(productRepository, 1, 1);
            // !!! setup order state
            vendingMachineController.VendingMachineState = VendingMachineState.Order;

            // Test subject Action - select order as product A
            orderPanel.InvokeOrderAction(OrderCmdEvent.Select, PrdA);

            // verify reserved product
            Assert.Equal(PrdA, vendingMachineController.OrderedProducts[0]);

            Assert.Equal(VendingMachineState.Payment, vendingMachineController.VendingMachineState);

            // complete payment without change
            paymentReceiver.InvokeCoinAction(PaymentCmdEvent.Payment, new Coin(10));
            paymentReceiver.InvokeCoinAction(PaymentCmdEvent.Payment, new Coin(20));
            paymentReceiver.InvokeCoinAction(PaymentCmdEvent.Payment, new Coin(5));

            // verify switch back to order state
            Assert.True(paymentReceiver.OffCalled);
            Assert.True(orderPanel.OnCalled);
            Assert.Equal(VendingMachineState.Order, vendingMachineController.VendingMachineState);

            // verify money in the wallet 
            var total = walletRepository.WalletList.Aggregate(Money.Zero, (m, p) => m + new Money(0, (int)p.Item1.Nominal * p.Item2));
            Assert.Equal(new Money(0, 35), total);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.Checkout
                ,MessageCode.BalancePayment
                ,MessageCode.BalancePayment
                ,MessageCode.BalancePayment
                ,MessageCode.CollectYourPurchase
                , MessageCode.ReadyToService
            };
            Assert.Equal((IEnumerable<MessageCode>)TestCatchedCodes, (IEnumerable<MessageCode>)messageRepository.CatchedCodes);
            Assert.Equal((IEnumerable<string>)messageRepository.ReturnList, (IEnumerable<string>)displayPanel.DisplayList);
        }

        [Fact]
        public void TestOrderPaymentRunOutChangeFromOrderPanel()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);
            InitProductRepository(productRepository, 1, 1);
            // !!! setup order state
            vendingMachineController.VendingMachineState = VendingMachineState.Order;

            // Test subject Action - select order as product A
            orderPanel.InvokeOrderAction(OrderCmdEvent.Select, PrdA);

            // verify reserved product
            Assert.Equal(PrdA, vendingMachineController.OrderedProducts[0]);

            Assert.Equal(VendingMachineState.Payment, vendingMachineController.VendingMachineState);

            // complete payment without change
            paymentReceiver.InvokeCoinAction(PaymentCmdEvent.Payment, new Coin(10));
            paymentReceiver.InvokeCoinAction(PaymentCmdEvent.Payment, new Coin(20));
            paymentReceiver.InvokeCoinAction(PaymentCmdEvent.Payment, new Coin(50));

            // verify switch back to order state
            Assert.True(paymentReceiver.OffCalled);
            Assert.True(orderPanel.OnCalled);
            Assert.Equal(VendingMachineState.Order, vendingMachineController.VendingMachineState);

            // verify money in the wallet 
            var total = walletRepository.WalletList.Aggregate(Money.Zero, (m, p) => m + new Money(0, (int)p.Item1.Nominal * p.Item2));
            Assert.Equal(new Money(0,0), total);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.Checkout
                ,MessageCode.BalancePayment
                ,MessageCode.BalancePayment
                ,MessageCode.BalancePayment
                ,MessageCode.RunOutOfChange
                ,MessageCode.OrderCancel
                ,MessageCode.ReturnPayment
                ,MessageCode.ReturnPayment
                ,MessageCode.ReturnPayment
                ,MessageCode.ReadyToService
            };
            Assert.Equal((IEnumerable<MessageCode>)TestCatchedCodes, (IEnumerable<MessageCode>)messageRepository.CatchedCodes);
            Assert.Equal((IEnumerable<string>)messageRepository.ReturnList, (IEnumerable<string>)displayPanel.DisplayList);
        }



        [Fact]
        public void TestOrderPaymentChangeGivenFromOrderPanel()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            MockDisplayPanel displayPanel = new MockDisplayPanel();
            MockOrderPanel orderPanel = new MockOrderPanel();
            MockPaymentReceiver paymentReceiver = new MockPaymentReceiver();
            MockVendingMessageRepository messageRepository = new MockVendingMessageRepository();
            ProductRepository productRepository = new ProductRepository();
            WalletRepository walletRepository = new WalletRepository();
            SoldRecord saleRecord = new SoldRecord();
            VendingMachineController vendingMachineController = new VendingMachineController(displayPanel, paymentReceiver, orderPanel, productRepository, walletRepository, saleRecord, messageRepository);
            InitProductRepository(productRepository, 1, 1);
            // !!! setup order state
            vendingMachineController.VendingMachineState = VendingMachineState.Order;

            // setup wallet repository
            walletRepository.AddToWallet(new Coin(5), 1);
            walletRepository.AddToWallet(new Coin(10), 1);
            walletRepository.AddToWallet(new Coin(20), 1);
            walletRepository.AddToWallet(new Coin(50), 1);
            var before = walletRepository.WalletList.Aggregate(Money.Zero, (m, p) => m + new Money(0, (int)p.Item1.Nominal * p.Item2));

            // Test subject Action - select order as product A
            orderPanel.InvokeOrderAction(OrderCmdEvent.Select, PrdA);

            Assert.Equal(VendingMachineState.Payment, vendingMachineController.VendingMachineState);

            // verify reserved product
            Assert.Equal(PrdA, vendingMachineController.OrderedProducts[0]);

            // complete payment without change
            paymentReceiver.InvokeCoinAction(PaymentCmdEvent.Payment, new Coin(100));

            // verify switch back to order state
            Assert.True(paymentReceiver.OffCalled);
            Assert.True(orderPanel.OnCalled);
            Assert.Equal(VendingMachineState.Order, vendingMachineController.VendingMachineState);

            // verify money in the wallet 
            var total = walletRepository.WalletList.Aggregate(Money.Zero, (m, p) => m + new Money(0, (int)p.Item1.Nominal * p.Item2));
            Assert.Equal(before + PrdA.Price, total);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.Checkout,
                MessageCode.BalancePayment,
                MessageCode.GivenChange,
                MessageCode.GivenChange,
                MessageCode.GivenChange,
                MessageCode.CollectYourPurchase,
                MessageCode.ReadyToService,
            };
            Assert.Equal((IEnumerable<MessageCode>)TestCatchedCodes, (IEnumerable<MessageCode>)messageRepository.CatchedCodes);
            Assert.Equal((IEnumerable<string>)messageRepository.ReturnList, (IEnumerable<string>)displayPanel.DisplayList);
        }
       
    }  
}      
