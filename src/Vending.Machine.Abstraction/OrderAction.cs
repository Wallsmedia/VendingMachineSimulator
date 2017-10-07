// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using Vending.Machine.Abstraction.Models;

namespace Vending.Machine.Abstraction
{
    /// <summary>
    /// Defines the purchase ordering panel delegate.
    /// </summary>
    /// <param name="cmd">The panel command.</param>
    /// <param name="obj">The command context object.</param>
    public delegate void OrderAction(OrderCmdEvent cmd, Product obj);
}