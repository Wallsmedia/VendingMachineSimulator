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
            var mockDisplayPanel = new MockDisplayPanel();
            var mockReadKeypadInput = new MockReadKeypadInput();
            var mockVendingMessageRepository = new MockVendingMessageRepository();

            var paymentPanel = new PaymentReceiver(mockDisplayPanel, mockReadKeypadInput, mockVendingMessageRepository);

            PaymentCmdEvent cmd = PaymentCmdEvent.Cancel;
            INotionValue objCoin = null;

            var firstCoin = paymentPanel.MapToCoins.First();
            mockReadKeypadInput.SimInputAs = firstCoin.Key;

            paymentPanel.CoinAction += (c, p) =>
            {
                cmd = c;
                objCoin = p;
                paymentPanel.Off();
            };
            bool exception = false;
            paymentPanel.FailtException += ex => exception = true;
            paymentPanel.On();
            Thread.Sleep(10000);

            // Should be selected product A
            Assert.False(exception);
            Assert.Equal(PaymentCmdEvent.Payment, cmd);

            Assert.Equal(firstCoin.Value, objCoin);

            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.MakeYourPayment,
                MessageCode.Ok
            };

            Assert.Equal(TestCatchedCodes, mockVendingMessageRepository.CatchedCodes);
            Assert.Equal(mockVendingMessageRepository.ReturnList, mockDisplayPanel.DisplayList);
        }

        [Fact]
        public void TestCancel()
        {
            var mockDisplayPanel = new MockDisplayPanel();
            var mockReadKeypadInput = new MockReadKeypadInput();
            var mockVendingMessageRepository = new MockVendingMessageRepository();

            var paymentPanel = new PaymentReceiver(mockDisplayPanel, mockReadKeypadInput, mockVendingMessageRepository);

            PaymentCmdEvent cmd = PaymentCmdEvent.Payment;
            INotionValue objCoin = null;

            mockReadKeypadInput.SimInputAs = '#';

            paymentPanel.CoinAction += (c, p) =>
            {
                cmd = c;
                objCoin = p;
                paymentPanel.Off();
            };
            bool exception = false;
            paymentPanel.FailtException += ex => exception = true;
            paymentPanel.On();
            Thread.Sleep(10000);

            // Should be selected product A
            Assert.False(exception);
            Assert.Equal(PaymentCmdEvent.Cancel, cmd);

            Assert.Equal(null, objCoin);

            List<MessageCode> TestCatchedCodes = new List<MessageCode>
            {
                MessageCode.MakeYourPayment,
                MessageCode.Ok
            };

            Assert.Equal(TestCatchedCodes, mockVendingMessageRepository.CatchedCodes);
            Assert.Equal(mockVendingMessageRepository.ReturnList, mockDisplayPanel.DisplayList);

        }

    }
}



