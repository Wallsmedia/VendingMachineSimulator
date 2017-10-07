// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

namespace Vending.Machine.App.Configuration
{
    /// <summary>
    /// POCO container to load selling product into Vending machine simulator.
    /// </summary>
    public class CoinChangeDeposit
    {
        /// <summary>
        /// The coin nominal in fractions.
        /// </summary>
        public uint Nominal { get; set; }

        /// <summary>
        /// The number coins in the set.
        /// </summary>
        public uint Number { get; set; }
    }
}