// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System.Collections.Generic;
using System.Threading;
using Vending.Machine.Abstraction;
using Vending.Machine.Abstraction.Models;
using Vending.Machine.Console;
using Vending.Machine.Console.Abstract;
using Xunit;
using Vending.Machine.Test.Mock;

namespace Vending.Machine.Test
{
    public partial class OrderPanelUnitTest
    {

        private Product PrdA = new Product() { Code = 'A', Price = new Money(0, 5), DisplayName = "Name-A" };
        private Product PrdB = new Product() { Code = 'B', Price = new Money(0, 10), DisplayName = "Name-B" };

        [Fact]
        public void TestSelect()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            var mockDisplayPanel = new MockDisplayPanel();
            // !!! define the simulation key pad command
            var mockReadKeypadInput = new MockReadKeypadInput() { SimInputAs = 'A' };
            var mockVendingMessageRepository = new MockVendingMessageRepository();
            IProductRepository product = new ProductRepository();
            product.RegisterOrUpdateProduct(PrdA);
            product.RegisterOrUpdateProduct(PrdB);
            product.AddToStock('a', 1);
            product.AddToStock('b', 1);
            var orderPanel = new OrderPanel(mockDisplayPanel, mockReadKeypadInput, product, mockVendingMessageRepository);
            OrderCmdEvent cmd = OrderCmdEvent.OutOfStock;
            Product objPrd = null;
            // !! define the action test listener
            orderPanel.OrderAction += (c, p) =>
            {
                cmd = c;
                objPrd = p;
                // off after getting the event
                orderPanel.Off();
            };
            bool exception = false;
            orderPanel.FailtException += ex => exception = true;

            // Test subject Action - select a product
            orderPanel.On();
            Thread.Sleep(10000);

            // verify - selected product A
            Assert.False(exception);
            Assert.Equal(OrderCmdEvent.Select, cmd);
            Assert.Equal(objPrd, PrdA);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.MakeYourOrder,
                MessageCode.SelectOrderLine,
                MessageCode.SelectOrderLine,
                MessageCode.SelectOrder,
                MessageCode.Ok
            };
            Assert.Equal(TestCatchedCodes, mockVendingMessageRepository.CatchedCodes);
            Assert.Equal(mockVendingMessageRepository.ReturnList, mockDisplayPanel.DisplayList);

        }


        [Fact]
        public void TestOutOfStock()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            var mockDisplayPanel = new MockDisplayPanel();
            // !!! define the simulation key pad command
            var mockReadKeypadInput = new MockReadKeypadInput() { SimInputAs = 'A' };
            var mockVendingMessageRepository = new MockVendingMessageRepository();
            IProductRepository product = new ProductRepository();
            product.RegisterOrUpdateProduct(PrdA);
            product.RegisterOrUpdateProduct(PrdB);
            var orderPanel = new OrderPanel(mockDisplayPanel, mockReadKeypadInput, product, mockVendingMessageRepository);
            OrderCmdEvent cmd = OrderCmdEvent.Select;
            Product objPrd = null;
            // !! define the action test listener
            orderPanel.OrderAction += (c, p) =>
            {
                cmd = c;
                objPrd = p;
                // off after getting the event
                orderPanel.Off();
            };
            bool exception = false;
            orderPanel.FailtException += ex => exception = true;

            // Test subject Action - out of stock 
            orderPanel.On();
            Thread.Sleep(10000);

            // verify - out of stock
            Assert.False(exception);
            Assert.Equal(OrderCmdEvent.OutOfStock, cmd);
            Assert.Null(objPrd);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode> { };
            Assert.Equal(TestCatchedCodes, mockVendingMessageRepository.CatchedCodes);
            Assert.Equal(mockVendingMessageRepository.ReturnList, mockDisplayPanel.DisplayList);

        }
    }
}



