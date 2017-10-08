// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vending.Machine.Abstraction;
using Vending.Machine.Abstraction.Models;

namespace Vending.Machine.Console
{
    /// <summary>
    /// Implements the Vending machine wallet repository.
    /// </summary>
    public class WalletRepository : IWalletRepository
    {
        protected readonly Dictionary<INotionValue, int> Repository = new Dictionary<INotionValue, int>();

        public List<(INotionValue, int)> WalletList => Repository.Where(r => r.Value > 0).Select(r => (r.Key, r.Value)).ToList();

        #region IWalletRepository interface

        public int AddToWallet(INotionValue notion, int number)
        {
            if (notion == null)
            {
                throw new ArgumentNullException(nameof(notion));
            }
            if (number < 0)
            {
                throw new InvalidDataException(nameof(number));
            }
            if (Repository.ContainsKey(notion))
            {
                Repository[notion] += number;
            }
            else
            {
                Repository[notion] = number;
            }
            return Repository[notion];
        }

        public int RemoveFromWallet(INotionValue notion, int number)
        {
            if (notion == null)
            {
                throw new ArgumentNullException(nameof(notion));
            }
            if (!Repository.ContainsKey(notion))
            {
                throw new InvalidDataException(nameof(notion));
            }
            if (number < 0 || number > Repository[notion])
            {
                throw new InvalidDataException(nameof(number));
            }
            return Repository[notion] -= number;
        }

        public void ClearMoney()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}