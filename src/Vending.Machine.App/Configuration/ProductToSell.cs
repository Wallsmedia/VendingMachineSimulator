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
    public class ProductToSell
    {
        
        /// <summary>
        /// Gets or sets the title of the product.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the cost of the product in fraction nominal.
        /// </summary>
        public uint Cost { get; set; }

        /// <summary>
        /// The number of items in the stock
        /// </summary>
        public uint NumberInStock { get; set; }
    }

}
