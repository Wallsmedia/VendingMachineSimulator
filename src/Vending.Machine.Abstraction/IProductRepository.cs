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
    /// Provides the product repository for a vending machine.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Registers or updates product in vending machine.
        /// </summary>
        /// <param name="product">The vending product.</param>
        /// <returns>True if it is updated.</returns>
        bool RegisterOrUpdateProduct(Product product);

        /// <summary>
        /// Get list products in the product repository
        /// </summary>
        List<Product> ProductList { get; }

        /// <summary>
        /// Gets product stock availability.
        /// </summary>
        /// <param name="code">The product code.</param>
        /// <returns>Number of products.</returns>
        int CountProduct(char code);

        /// <summary>
        /// Records the sell operation.
        /// </summary>
        /// <param name="code">The product code.</param>
        /// <returns>The current stock after selling.</returns>
        int RemoveSellFromStock(char code);

        /// <summary>
        /// Records the add to stock operation.
        /// </summary>
        /// <param name="code">The product code.</param>
        /// <param name="number">The number of product.</param>
        /// <returns>The current stock after.</returns>
        int AddToStock(char code, int number);
    }
}