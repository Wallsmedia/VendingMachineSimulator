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

namespace Vending.Machine.Abstraction.Models
{
    /// <summary>
    /// Defines the money entity for payment operations.
    /// </summary>
    public struct Money
    {
        /// <summary>
        /// Gets the number.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Fraction { get; }

        /// <summary>
        /// The fraction size.
        /// </summary>
        public static int FractionSize { get; set; } = 100;

        /// <summary>
        /// Returns Zero entity.
        /// </summary>
        public static Money Zero => new Money();

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="number">The value of numbers</param>
        /// <param name="fraction">The value of fractions.</param>
        public Money(int number, int fraction)
        {
            if (number < 0)
            {
                throw new InvalidDataException(nameof(number));
            }

            if (fraction < 0)
            {
                throw new InvalidDataException(nameof(fraction));
            }

            Number = number;
            Number += fraction / FractionSize;
            Fraction = fraction % FractionSize;
        }

        /// <summary>
        /// Converts into fraction units .
        /// </summary>
        public int ToFractionValue() => Number * FractionSize + Fraction;


        /// <summary>
        /// Defines operator  &lt;
        /// </summary>
        public static bool operator <(Money m1, Money m2) => m1.ToFractionValue() < m2.ToFractionValue();

        /// <summary>
        /// Defines operator  &gt;
        /// </summary>
        public static bool operator >(Money m1, Money m2) => m1.ToFractionValue() > m2.ToFractionValue();

        /// <summary>
        /// Defines operator &lt;=
        /// </summary>
        public static bool operator <=(Money m1, Money m2) => m1.ToFractionValue() <= m2.ToFractionValue();

        /// <summary>
        /// Defines operator &gt;=
        /// </summary>
        public static bool operator >=(Money m1, Money m2) => m1.ToFractionValue() >= m2.ToFractionValue();

        /// <summary>
        /// Defines operator +
        /// </summary>
        public static Money operator +(Money m1, Money m2)
        {
            var fraction = m1.ToFractionValue() + m2.ToFractionValue();
            return new Money(0, fraction);
        }

        /// <summary>
        /// Defines operator -
        /// </summary>
        public static Money operator -(Money m1, Money m2)
        {
            var fraction = m1.ToFractionValue() - m2.ToFractionValue();
            return new Money(0, fraction);
        }

        /// <summary>
        /// Defines operator ==
        /// </summary>
        public static bool operator ==(Money m1, Money m2) => m1.Equals(m2);

        /// <summary>
        /// Defines operator !=
        /// </summary>
        public static bool operator !=(Money m1, Money m2) => !m1.Equals(m2);

        /// <summary>
        /// Performs equation.
        /// </summary>
        public override bool Equals(object o)
        {
            if (o is Money m)
            {
                return Number == m.Number && Fraction == m.Fraction;
            }
            return false;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        public override int GetHashCode()
        {
            return Number.GetHashCode() ^ Fraction.GetHashCode();
        }

        /// <summary>
        /// Converts to a string.
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0}.{1}", Number, Fraction.ToString("D2"));
        }

    }
}