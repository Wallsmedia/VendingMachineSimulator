﻿// \\     |/\  /||
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
    /// Component contract represents the turn on off interface.
    /// </summary>
    public interface ILogicalSwitch
    {
        /// <summary>
        /// Activates the component.
        /// </summary>
        void On();

        /// <summary>
        /// Deactivates the component.
        /// </summary>
        void Off();
    }


}
