// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.


using System.Collections.Generic;
using Vending.Machine.Console.Abstract;

namespace Vending.Machine.Console
{
    /// <summary>
    /// Implements the Vending machine message repository.
    /// </summary>
    public class VendingMessageRepository : IVendingMessageRepository
    {
        static Dictionary<MessageCode, string> _msgRepository = new Dictionary<MessageCode, string>()
        {
            [MessageCode.OutOfServise] = "Vending Machine Out of Service",
            [MessageCode.OutOfStock] = "Vending Machine Out of Stock",
            [MessageCode.ReadyToService] = "Ready to Service",

            [MessageCode.MakeYourOrder] = "\r\nMake your order",
            [MessageCode.SelectOrderLine] = "Code {0} to choose {1} by price {2}",
            [MessageCode.SelectOrder] = "Enter 'Code' for order :",

            [MessageCode.Checkout] = "\r\nCheckout: {0} ",

            [MessageCode.RunOutOfChange] = "Sorry, Run out Of Change",
            [MessageCode.OrderCancel] = "Order Canceled",
            [MessageCode.MakeYourPayment] = "Make payment,Enter {0} or  '#' -cancel;",
            [MessageCode.BalancePayment] = "Total: {0} Paid: {1} ",

            [MessageCode.CollectYourPurchase] = "\r\nCollect your order: {0}",

            [MessageCode.ReturnPayment] = "Return payment:  {0} x {1}",
            [MessageCode.GivenChange]   = "Change given:  {0} x {1}",

            [MessageCode.InvalidInput]   = " Wrong choice \r\n",
            [MessageCode.Ok]   = " Ok \r\n",



        };

        #region IVendingMessageRepository interface

        /// <summary>
        /// Provides message by a code.
        /// </summary>
        /// <param name="code">The massage code.</param>
        /// <returns>String text or empty.</returns>
        public string GetMessage(MessageCode code)
        {
            if (_msgRepository.ContainsKey(code))
            {
                return _msgRepository[code];
            }
            return string.Empty;
        }

        #endregion
    }
}

