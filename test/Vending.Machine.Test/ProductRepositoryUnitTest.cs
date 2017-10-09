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
            // setup tested class instance
            ProductRepository productRepository = new ProductRepository();

            // Test subject Action - registration A
            productRepository.RegisterOrUpdateProduct(PrdA);

            // verify - registration A
            Assert.Single(productRepository.ProductList);
            Assert.Equal(0, productRepository.CountProduct(PrdA.Code));
            Assert.Equal(0, productRepository.CountProduct(PrdB.Code));

            // Test subject Action - add to stock
            productRepository.AddToStock(PrdA.Code, 10);

            // verify - add to stock 
            Assert.Equal(10, productRepository.CountProduct(PrdA.Code));

            // Test subject Action - update of registration
            productRepository.RegisterOrUpdateProduct(PrdA);

            // verify -  update of registration
            Assert.Single(productRepository.ProductList);
            Assert.Equal(0, productRepository.CountProduct(PrdA.Code));
            Assert.Single(productRepository.ProductList);

            // Test subject Action - add second type of product
            productRepository.RegisterOrUpdateProduct(PrdB);

            // verify - add second type of product
            Assert.Equal(2, productRepository.ProductList.Count);
        }

        [Fact]
        public void OperationsStockTest()
        {
            // setup tested class instance
            ProductRepository productRepository = new ProductRepository();

            // Test subject Action - register and add to stock
            productRepository.RegisterOrUpdateProduct(PrdA);
            productRepository.AddToStock(PrdA.Code, 10);

            // verify - register and add to stock
            Assert.Equal(10, productRepository.CountProduct(PrdA.Code));

            // Test subject Action - add to stock
            productRepository.AddToStock(PrdA.Code, 5);
            
            // verify - add to stock
            Assert.Equal(15, productRepository.CountProduct(PrdA.Code));

            // Test subject Action - remove from stock
            productRepository.RemoveSellFromStock(PrdA.Code);

            // verify - remove from stock
            Assert.Equal(14, productRepository.CountProduct(PrdA.Code));
        }

        [Fact]
        public void ExceptionsStockTest()
        {
            // setup tested class instance
            ProductRepository productRepository = new ProductRepository();

            // Test subject Action and verify
            Assert.Throws<ArgumentNullException>(() => { productRepository.RegisterOrUpdateProduct(null); });

            productRepository.RegisterOrUpdateProduct(PrdA);

            // Test subject Action and verify
            Assert.Throws<InvalidDataException>(() => { productRepository.AddToStock(PrdB.Code, 1); });

            // Test subject Action and verify
            Assert.Throws<InvalidDataException>(() => { productRepository.AddToStock(PrdA.Code, -1); });

            // Test subject Action and verify
            Assert.Throws<InvalidDataException>(() => { productRepository.RemoveSellFromStock(PrdA.Code); });

            // Test subject Action and verify
            Assert.Throws<InvalidDataException>(() => { productRepository.RemoveSellFromStock(PrdB.Code); });
        }

    }
}
