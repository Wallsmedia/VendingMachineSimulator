// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.


namespace Vending.Machine.Abstraction.Models
{
    /// <summary>
    /// Represents the product for sale.
    /// </summary>
    public class Product
    {
        private char _code;

        /// <summary>
        /// Gets or sets the product Identity/Key/Id .
        /// </summary>
        public char Code
        {
            get => _code;
            set => _code = char.ToUpper(value);
        }

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        public Money Price { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The string to describe the product.
        /// </summary>
        public override string ToString() => $"'{Code}' price:[{Price}]  '{DisplayName}'";
    }
}
