// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using Vending.Machine.Abstraction.Models;
using Vending.Machine.Console.Abstract;

namespace Vending.Machine.Console
{
    /// <summary>
    /// Log of sale Records 
    /// </summary>
    public class SoldRecord : ISoldRecord
    {
        /// <summary>
        /// Records the sale of a product.
        /// </summary>
        /// <param name="product">The sold product.</param>
        public void Sold(Product product)
        {
            //TODO in the non simulator version
        }
    }
}