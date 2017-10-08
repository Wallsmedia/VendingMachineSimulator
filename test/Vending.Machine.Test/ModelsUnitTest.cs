// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using Vending.Machine.Abstraction.Models;
using Xunit;

namespace Vending.Machine.Test
{
    public class ModelsUnitTest
    {
       
        [Fact]
        public void CoinTestEquals()
        {
            Coin m100 = new Coin(100);
            Coin m200 = new Coin(200);
            Coin d100 = new Coin(100);
            Assert.Equal(m100, d100);
            Assert.NotEqual(m100, m200);
        }

    }
}
