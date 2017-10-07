// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.


using System.Threading;
using Vending.Machine.Console.Abstract;

namespace Vending.Machine.Console
{

    /// <summary>
    /// Windows simulator implementation of keypad input panel.
    /// </summary>
    public class ReadKeypadInput : IReadKeypadInput
    {

        /// <summary>
        /// Reads string from input device
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The text string.</returns>
        public char ReadInput(CancellationToken cancellationToken)
        {
            do
            {
                if (!System.Console.KeyAvailable)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    var key = System.Console.ReadKey();
                    char cki = key.KeyChar;
                    if (char.IsLetterOrDigit(cki) || cki == '#' || cki == '*')
                    {
                        return char.ToUpper(cki);
                    }
                }

            } while (!cancellationToken.IsCancellationRequested);

            return (char)0;
        }
    }
}

