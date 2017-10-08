// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System;
using System.IO;
using System.Linq;
using Vending.Machine.Abstraction.Models;
using Vending.Machine.Console;
using Xunit;

namespace Vending.Machine.Test
{
    public class WalletRepositoryUnitTest
    {
        Coin p5 = new Coin(5);
        Coin p10 = new Coin(10);
        Coin p20 = new Coin(20);
        Coin p50 = new Coin(50);
        Coin p100 = new Coin(100);

        [Fact]
        public void AddWalletTest()
        {
            WalletRepository wallet = new WalletRepository();

            // Add 
            wallet.AddToWallet(p5, 1);
            wallet.AddToWallet(p10, 2);
            wallet.AddToWallet(p20, 3);
            wallet.AddToWallet(p50, 4);
            wallet.AddToWallet(p100, 5);

            // verify
            var content = wallet.WalletList;

            Assert.Single(content.Where(c => c.Item1.MoneyValue == p5.MoneyValue));
            Assert.Single(content.Where(c => c.Item1.MoneyValue == p10.MoneyValue));
            Assert.Single(content.Where(c => c.Item1.MoneyValue == p20.MoneyValue));
            Assert.Single(content.Where(c => c.Item1.MoneyValue == p50.MoneyValue));
            Assert.Single(content.Where(c => c.Item1.MoneyValue == p100.MoneyValue));


            Assert.Equal(1, content.Single(c => c.Item1.Nominal == p5.Nominal).Item2);
            Assert.Equal(2, content.Single(c => c.Item1.Nominal == p10.Nominal).Item2);
            Assert.Equal(3, content.Single(c => c.Item1.Nominal == p20.Nominal).Item2);
            Assert.Equal(4, content.Single(c => c.Item1.Nominal == p50.Nominal).Item2);
            Assert.Equal(5, content.Single(c => c.Item1.Nominal == p100.Nominal).Item2);
        }



        [Fact]
        public void RemoveWalletTest()
        {

            WalletRepository wallet = new WalletRepository();

            // Add 
            wallet.AddToWallet(p5, 1);
            wallet.AddToWallet(p10, 2);
            wallet.AddToWallet(p20, 3);
            wallet.AddToWallet(p50, 4);
            wallet.AddToWallet(p100, 5);

            // remove

            wallet.RemoveFromWallet(p5, 1);
            wallet.RemoveFromWallet(p10, 2);
            wallet.RemoveFromWallet(p20, 3);
            wallet.RemoveFromWallet(p50, 4);
            wallet.RemoveFromWallet(p100, 5);

            // verify
            var content = wallet.WalletList;
            Assert.Empty(content);
        }

        [Fact]
        public void ExceptionsWalletTest()
        {

            WalletRepository wallet = new WalletRepository();

            Assert.Throws<InvalidDataException>(() => { wallet.AddToWallet(p10, -1); });
            Assert.Throws<ArgumentNullException>(() => { wallet.AddToWallet(null, 1); });
            Assert.Throws<ArgumentNullException>(() => { wallet.AddToWallet(null, 0); });
            Assert.Throws<ArgumentNullException>(() => { wallet.AddToWallet(null, -1); });

            Assert.Throws<InvalidDataException>(() => { wallet.RemoveFromWallet(p10, 1); });
            wallet.AddToWallet(p10, 1);
            Assert.Throws<InvalidDataException>(() => { wallet.RemoveFromWallet(p10, -1); });
            Assert.Throws<InvalidDataException>(() => { wallet.RemoveFromWallet(p10, 2); });
            wallet.RemoveFromWallet(p10, 1);
            Assert.Throws<InvalidDataException>(() => { wallet.RemoveFromWallet(p10, 1); });


            Assert.Throws<ArgumentNullException>(() => { wallet.RemoveFromWallet(null, -1); });
            Assert.Throws<ArgumentNullException>(() => { wallet.RemoveFromWallet(null, 0); });
            Assert.Throws<ArgumentNullException>(() => { wallet.RemoveFromWallet(null, 1); });

            Assert.Throws<NotImplementedException>(() => { wallet.ClearMoney(); });

            try
            {
                PaymentWalletRepository pwallet = new PaymentWalletRepository();
                pwallet.ClearMoney();
            }
            catch
            {
                Assert.True(false);
            }

        }
    }
}
