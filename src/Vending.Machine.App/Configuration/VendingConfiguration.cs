// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System.Collections.Generic;

namespace Vending.Machine.App.Configuration
{

    /// <summary>
    /// Vending machine configuration.
    /// </summary>
    public class VendingConfiguration
    {
        /// <summary>
        /// Gets the max number of sections for products.
        /// </summary>
        public int MaxSections { get; protected set; } = 20;

        /// <summary>
        /// Wallet configuration settings.
        /// </summary>
        public List<CoinChangeDeposit> ChangeWalletConfig { get; set; } = new List<CoinChangeDeposit>();

        /// <summary>
        /// Product stock configuration settings. 
        /// </summary>
        public List<ProductToSell> ProductStockConfig { get; set; } = new List<ProductToSell>();
    }
}