// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System.Collections.Generic;
using Vending.Machine.Abstraction.Models;

namespace Vending.Machine.Abstraction
{
    /// <summary>
    /// The generated events by coin receiver.
    /// </summary>
    public enum PaymentCmdEvent
    {
        Cancel,
        Payment
    }

    /// <summary>
    /// The coin payment receiver.
    /// </summary>
    public interface IPaymentReceiver : ILogicalSwitch
    {
        /// <summary>
        /// The list of accepted coins types.
        /// </summary>
        List<Coin> AcceptedCoins { get; }

        /// <summary>
        /// The code map to a Coin.
        /// </summary>
        Dictionary<char, Coin> MapToCoins { get; }

        /// <summary>
        /// The payment action event.
        /// </summary>
        event PaymentAction CoinAction;

        /// <summary>
        /// The fault of event.
        /// </summary>
        event FailtException FailtException;
    }


}
