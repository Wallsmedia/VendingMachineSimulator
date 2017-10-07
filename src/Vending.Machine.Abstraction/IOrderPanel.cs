// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

namespace Vending.Machine.Abstraction
{
    /// <summary>
    /// The events generated purchase panel
    /// </summary>
    public enum OrderCmdEvent
    {
        OutOfStock,
        Select,
    }

    /// <summary>
    /// Represents a contract interface to a purchase ordering panel 
    /// </summary>
    public interface IOrderPanel:ILogicalSwitch
    {
        /// <summary>
        /// Purchase order instruction controller subscription.
        /// </summary>
        event OrderAction OrderAction;

        /// <summary>
        /// The fault of the component signal.
        /// </summary>
        event FailtException FailtException;

    }
    
}
