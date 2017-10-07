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
    /// Implements the Vending machine product repository.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        readonly Dictionary<char, (Product, int)> _repository = new Dictionary<char, (Product, int)>();

        public List<Product> ProductList => _repository.Values.Select(p => p.Item1).ToList();

        #region IProductRepository interface

        /// <summary>
        /// Registers/update a product for vending, reset stock to zero.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>True if it was updated.</returns>
        public bool RegisterOrUpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            bool update = !_repository.ContainsKey(product.Code);
            _repository[product.Code] = (product, 0);
            return update;
        }
        
        public int AddToStock(char code, int number)
        {
            code = char.ToUpper(code);
            if (number < 0)
            {
                throw new InvalidDataException(nameof(number));
            }
            if (!_repository.ContainsKey(code))
            {
                throw new InvalidDataException(nameof(code));

            }
            (Product p, int oldnumber) = _repository[code];
            oldnumber += number;
            _repository[code] = (p, oldnumber);
            return oldnumber;
        }

        public int CountProduct(char code)
        {
            code = char.ToUpper(code);
            if (_repository.ContainsKey(code))
            {
                return _repository[code].Item2;
            }
            return 0;
        }

        public int RemoveSellFromStock(char code)
        {
            code = char.ToUpper(code);
            if (!_repository.ContainsKey(code))
            {
                throw new InvalidDataException(nameof(code));

            }
            (Product p, int number) = _repository[code];
            if (number == 0)
            {
                throw new InvalidDataException(nameof(number));
            }
            number--;
            _repository[code] = (p, number);
            return number;
        }

        #endregion

    }
}