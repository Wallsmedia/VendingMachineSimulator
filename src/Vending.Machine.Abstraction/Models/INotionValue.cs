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
    /// Represent the notion value.
    /// </summary>
    public interface INotionValue
    {
        /// <summary>
        /// Get the notion nominal, in fractions, 1/100.
        /// </summary>
        uint Nominal { get; }

        /// <summary>
        /// Get the notion value as <see cref="Money"/>.
        /// </summary>
        Money MoneyValue { get; }
    }
}