// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.



namespace Vending.Machine.Console.Abstract
{
    /// <summary>
    /// Defines messages types
    /// </summary>
    public enum MessageCode
    {
        OutOfServise,
        OutOfStock,
        ReadyToService,

        MakeYourOrder,
        SelectOrderLine,
        SelectOrder,

        MakeYourPayment,
        OrderCancel,
        BalancePayment,
        Checkout,
        CollectYourPurchase,
        ReturnPayment,
        RunOutOfChange,
        GivenChange,
        InvalidInput,
        Ok,
    }

    /// <summary>
    /// Defines message repository for a vending machine.
    /// </summary>
    public interface IVendingMessageRepository
    {
        /// <summary>
        /// Provides message by a code.
        /// </summary>
        /// <param name="code">The massage code.</param>
        /// <returns>String text or empty.</returns>
        string GetMessage(MessageCode code);
    }

}

