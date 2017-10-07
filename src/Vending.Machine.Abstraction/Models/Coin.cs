// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System;

namespace Vending.Machine.Abstraction.Models
{
    /// <summary>
    /// Represents the coin entity
    /// </summary>
    public class Coin : INotionValue
    {
        /// <summary>
        /// Get the coin nominal in fractions.
        /// </summary>
        public uint Nominal { get; }

        /// <summary>
        /// Get the coin value as <see cref="Money"/>.
        /// </summary>
        public Money MoneyValue { get; }
        
        /// <summary>
        /// Constructs the class.
        /// </summary>
        /// <param name="nominal"></param>
        public Coin(uint nominal)
        {
            Nominal = nominal;
            MoneyValue = new Money(0,(int)nominal);
        }

        /// <summary>
        /// For functional logics it is used only the Nominal as a Key. 
        /// </summary>
        public override bool Equals(object o)
        {
            if (o is Coin m)
            {
                return Nominal == m.Nominal;
            }
            return false;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        public override int GetHashCode()
        {
            return  Nominal.GetHashCode();
        }

        /// <summary>
        /// Converts to a string.
        /// </summary>
        public override string ToString()
        {
            return String.Format(".{1}",  Nominal.ToString("D2"));
        }
    }
}