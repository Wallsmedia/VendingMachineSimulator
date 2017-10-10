// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Vending.Machine.Abstraction;
using Vending.Machine.Abstraction.Models;
using Vending.Machine.Console;
using Vending.Machine.Console.Abstract;
using Xunit;
using Vending.Machine.Test.Mock;

namespace Vending.Machine.Test
{
    public partial class PaymentReceiverUnitTest
    {

        [Fact]
        public void TestPayment()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            var mockDisplayPanel = new MockDisplayPanel();
            var mockReadKeypadInput = new MockReadKeypadInput();
            var mockVendingMessageRepository = new MockVendingMessageRepository();
            var paymentPanel = new PaymentReceiver(mockDisplayPanel, mockReadKeypadInput, mockVendingMessageRepository);
            PaymentCmdEvent cmd = PaymentCmdEvent.Cancel;
            INotionValue objCoin = null;
            // !!! define the simulation key pad command of payment
            var firstCoin = paymentPanel.MapToCoins.First();
            mockReadKeypadInput.SimInputAs = firstCoin.Key;
            // !! define the action test listener
            paymentPanel.CoinAction += (c, p) =>
            {
                cmd = c;
                objCoin = p;
                // off after getting the event
                paymentPanel.Off();
            };
            bool exception = false;
            paymentPanel.FailtException += ex => exception = true;

            // Test subject Action - on payment receiver and get simulated payment
            paymentPanel.On();
            Thread.Sleep(10000);

            // verify - payment
            Assert.False(exception);
            Assert.Equal(PaymentCmdEvent.Payment, cmd);
            Assert.Equal(firstCoin.Value, objCoin);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.MakeYourPayment,
                MessageCode.Ok
            };
            Assert.Equal((IEnumerable<MessageCode>)TestCatchedCodes, (IEnumerable<MessageCode>)mockVendingMessageRepository.CatchedCodes);
            Assert.Equal((IEnumerable<string>)mockVendingMessageRepository.ReturnList, (IEnumerable<string>)mockDisplayPanel.DisplayList);
        }

        [Fact]
        public void TestCancel()
        {
            // setup the tested class instance and dependencies with the test case initialization.
            var mockDisplayPanel = new MockDisplayPanel();
            var mockReadKeypadInput = new MockReadKeypadInput();
            var mockVendingMessageRepository = new MockVendingMessageRepository();
            var paymentPanel = new PaymentReceiver(mockDisplayPanel, mockReadKeypadInput, mockVendingMessageRepository);
            PaymentCmdEvent cmd = PaymentCmdEvent.Payment;
            INotionValue objCoin = null;
            // !!! define the simulation key pad command
            mockReadKeypadInput.SimInputAs = '#';
            // !! define the action test listener
            paymentPanel.CoinAction += (c, p) =>
            {
                cmd = c;
                objCoin = p;
                // off after getting the event
                paymentPanel.Off();
            };
            bool exception = false;
            paymentPanel.FailtException += ex => exception = true;

            // Test subject Action - on payment receiver and get simulated cancel
            paymentPanel.On();
            Thread.Sleep(10000);

            // verify - cancel
            Assert.False(exception);
            Assert.Equal(PaymentCmdEvent.Cancel, cmd);
            Assert.Null(objCoin);

            // verify the  message flow
            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.MakeYourPayment,
                MessageCode.Ok
            };
            Assert.Equal((IEnumerable<MessageCode>)TestCatchedCodes, (IEnumerable<MessageCode>)mockVendingMessageRepository.CatchedCodes);
            Assert.Equal((IEnumerable<string>)mockVendingMessageRepository.ReturnList, (IEnumerable<string>)mockDisplayPanel.DisplayList);
        }

    }
}
