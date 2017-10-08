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
    /// Provides the wallet money repository for a vending machine.
    /// </summary>
    public interface IWalletRepository
    {
        /// <summary>
        /// Get list of the wallet content
        /// </summary>
        List<(INotionValue,int)> WalletList { get; }

        /// <summary>
        /// Removes from the wallet.
        /// </summary>
        /// <param name="notion">The product to sell.</param>
        /// <param name="number">The number to sell.</param>
        /// <returns>The current stock after selling.</returns>
        int RemoveFromWallet(INotionValue notion, int number);

        /// <summary>
        /// Adds to the wallet.
        /// </summary>
        /// <param name="notion">The product to add.</param>
        /// <param name="number">The number to add.</param>
        /// <returns>The current stock after.</returns>
        int AddToWallet(INotionValue notion, int number);

        void ClearMoney();

    }
}