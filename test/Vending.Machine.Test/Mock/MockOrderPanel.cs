// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.

using System;
using Vending.Machine.Abstraction;
using Vending.Machine.Abstraction.Models;

namespace Vending.Machine.Test.Mock
{
    public class MockOrderPanel : IOrderPanel
    {
        public event OrderAction OrderAction;
        public event FailtException FailtException;

        public void InvokeCoinAction(OrderCmdEvent cmd, Product obj)
        {
            OrderAction?.Invoke(cmd, obj);
        }

        public void InvokeFailtException(Exception ex)
        {
            FailtException?.Invoke(ex);
        }

        public bool OffCalled { get; set; }
        public void Off()
        {
            OffCalled = true;
        }

        public bool OnCalled { get; set; }
        public void On()
        {
            OnCalled = true;
        }
    }


}
