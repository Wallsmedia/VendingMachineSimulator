// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System.IO;
using System.Linq;
using Vending.Machine.Abstraction;
using Vending.Machine.Abstraction.Models;

namespace Vending.Machine.App.Configuration
{
    /// <summary>
    /// Configuration extension methods
    /// </summary>
    public static class VendingConfigurationExt
    {
        /// <summary>
        /// Loads product repository.
        /// </summary>
        /// <param name="machineConfig">The configuration.</param>
        /// <param name="productRepository">The product repository.</param>
        public static void LoadProducts(this VendingConfiguration machineConfig, IProductRepository productRepository)
        {
            char currCode = 'A';
            int sectionNumber = 0;
            foreach (var p in machineConfig.ProductStockConfig)
            {
                if (sectionNumber >= machineConfig.MaxSections)
                {
                    throw new InvalidDataException($"Exceeded number of sections {machineConfig.MaxSections}");
                }

                var product = new Product()
                {
                    Code = currCode,
                    DisplayName = p.Title,
                    Price = new Money(0, (int)p.Cost)
                };
                productRepository.RegisterOrUpdateProduct(product);
                productRepository.AddToStock(currCode, (int)p.NumberInStock);
                currCode++;
                sectionNumber++;
            }
        }


        /// <summary>
        /// Loads coins into wallet. 
        /// </summary>
        /// <param name="walletRepository">The coin repository.</param>
        /// <param name="paymentReceiver">The payment receiver.</param>
        /// <param name="machineConfig">The configuration.</param>
        public static void LoadWallet(this VendingConfiguration machineConfig, IWalletRepository walletRepository, IPaymentReceiver paymentReceiver)
        {
            foreach (var ch in machineConfig.ChangeWalletConfig)
            {
                if (paymentReceiver.AcceptedCoins.Any(c => c.Nominal == ch.Nominal))
                {
                    walletRepository.AddToWallet(new Coin(ch.Nominal), (int)ch.Number);
                }
                else
                {
                    throw new InvalidDataException($"Invalid coin type of nominal {ch.Nominal}");
                }

            }
        }

    }
}