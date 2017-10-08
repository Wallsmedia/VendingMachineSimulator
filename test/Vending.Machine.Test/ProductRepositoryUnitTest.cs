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
using Vending.Machine.Abstraction.Models;
using Vending.Machine.Console;
using Xunit;

namespace Vending.Machine.Test
{
    public class ProductRepositoryUnitTest
    {
        private Product PrdA = new Product() { Code = 'A', Price = new Money(1, 0), DisplayName = "Name-A" };
        private Product PrdB = new Product() { Code = 'B', Price = new Money(1, 0), DisplayName = "Name-B" };

        [Fact]
        public void RegisterStockTest()
        {
            ProductRepository productRepository = new ProductRepository();
            productRepository.RegisterOrUpdateProduct(PrdA);
            Assert.Single(productRepository.ProductList);
            Assert.Equal(0, productRepository.CountProduct(PrdA.Code));
            Assert.Equal(0, productRepository.CountProduct(PrdB.Code));

            productRepository.AddToStock(PrdA.Code, 10);
            Assert.Equal(10, productRepository.CountProduct(PrdA.Code));

            productRepository.RegisterOrUpdateProduct(PrdA);
            Assert.Single(productRepository.ProductList);
            Assert.Equal(0, productRepository.CountProduct(PrdA.Code));
            Assert.Single(productRepository.ProductList);

            productRepository.RegisterOrUpdateProduct(PrdB);
            Assert.Equal(2, productRepository.ProductList.Count);
        }

        [Fact]
        public void OperationsStockTest()
        {
            ProductRepository productRepository = new ProductRepository();
            productRepository.RegisterOrUpdateProduct(PrdA);

            productRepository.AddToStock(PrdA.Code, 10);
            Assert.Equal(10, productRepository.CountProduct(PrdA.Code));

            productRepository.AddToStock(PrdA.Code, 5);
            Assert.Equal(15, productRepository.CountProduct(PrdA.Code));

            productRepository.RemoveSellFromStock(PrdA.Code);
            Assert.Equal(14, productRepository.CountProduct(PrdA.Code));
        }

        [Fact]
        public void ExceptionsStockTest()
        {
            ProductRepository productRepository = new ProductRepository();
            Assert.Throws<ArgumentNullException>(() => { productRepository.RegisterOrUpdateProduct(null); });

            productRepository.RegisterOrUpdateProduct(PrdA);
            Assert.Throws<InvalidDataException>(() => { productRepository.AddToStock(PrdB.Code, 1); });
            Assert.Throws<InvalidDataException>(() => { productRepository.AddToStock(PrdA.Code, -1); });
            Assert.Throws<InvalidDataException>(() => { productRepository.RemoveSellFromStock(PrdA.Code); });
            Assert.Throws<InvalidDataException>(() => { productRepository.RemoveSellFromStock(PrdB.Code); });
        }

    }
}
