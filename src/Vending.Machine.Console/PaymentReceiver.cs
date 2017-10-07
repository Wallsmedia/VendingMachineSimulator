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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vending.Machine.Abstraction;
using Vending.Machine.Abstraction.Models;
using Vending.Machine.Console.Abstract;

namespace Vending.Machine.Console
{
    /// <summary>
    /// Implements the wallet coin repository.
    /// </summary>
    public class PaymentReceiver : IPaymentReceiver
    {
        /// <summary>
        /// Gets or sets status flag.
        /// </summary>
        public bool IsStateOn { get; set; }

        readonly IDisplayPanel _displayPanel;
        readonly IReadKeypadInput _readKeypadInput;
        readonly IVendingMessageRepository _vendingMessageRepository;
        CancellationTokenSource _cancellationTokenSource;
        CancellationToken _cancellationToken;

        // ReSharper disable once NotAccessedField.Local
        Task _readKeypadTask;

        /// <summary>
        /// Payment coin receiver simulator
        /// </summary>
        /// <param name="displayPanel">The display panel.</param>
        /// <param name="readKeypadInput">The key pad input.</param>
        /// <param name="vendingMessageRepository">The message repositories.</param>
        public PaymentReceiver(IDisplayPanel displayPanel, IReadKeypadInput readKeypadInput, IVendingMessageRepository vendingMessageRepository)
        {
            _vendingMessageRepository = vendingMessageRepository;
            _displayPanel = displayPanel;
            _readKeypadInput = readKeypadInput;
        }

        #region IPaymentReceiver interface

        public event PaymentAction CoinAction;
        public event FailtException FailtException;

        public void Off()
        {
            _cancellationTokenSource?.Cancel();
            IsStateOn = false;
            _readKeypadTask = null;
        }

        public void On()
        {
            try
            {
                IsStateOn = true;
                _cancellationTokenSource = new CancellationTokenSource();
                _cancellationToken = _cancellationTokenSource.Token;
                _readKeypadTask = Task.Run((Action)ReadPaymentInstructions, _cancellationToken);
            }
            catch (Exception e)
            {
                FailtException?.Invoke(e);
            }
        }

        public List<Coin> AcceptedCoins => _acceptedCoins.Values.ToList();
        #endregion

        private readonly Dictionary<char, Coin> _acceptedCoins = new Dictionary<char, Coin>()
        {
            {'1', new Coin(5)},
            {'2', new Coin(10)},
            {'3', new Coin(20)},
            {'4', new Coin(50)},
            {'5', new Coin(100)}
        };

        private string msgAccepted = " 1 -[5p]; 2 -[10p]; 3 -[20p]; 4 -[50p]; 5 -[£1.00]";

        /// <summary>
        /// Communicate with payment keypad simulator.
        /// </summary>
        private void ReadPaymentInstructions()
        {
            try
            {
                do
                {
                    string template = _vendingMessageRepository.GetMessage(MessageCode.MakeYourPayment);
                    if (!string.IsNullOrWhiteSpace(template))
                    {
                        string msg = string.Format(template, msgAccepted);
                        _displayPanel.DisplayMessage(msg);
                    }
                    // get the key code
                    char code = _readKeypadInput.ReadInput(_cancellationToken);

                    if (code == '#')
                    {
                        // cancel simulation key
                        DisplayMessageByCode(MessageCode.Ok);
                        CoinAction?.Invoke(PaymentCmdEvent.Cancel, null);
                    }
                    else if (_acceptedCoins.ContainsKey(code))
                    {
                        // valid coin code
                        var coin = _acceptedCoins[code];
                        DisplayMessageByCode(MessageCode.Ok);
                        // signal about payment
                        CoinAction?.Invoke(PaymentCmdEvent.Payment, coin);
                    }
                    else
                    {
                        // invalid keypad simulation key
                        DisplayMessageByCode(MessageCode.InvalidInput);
                    }
                }
                while (IsStateOn);
            }
            catch (Exception e)
            {
                FailtException?.Invoke(e);
            }
        }

        private void DisplayMessageByCode(MessageCode messageCode)
        {
            _displayPanel.DisplayMessage(_vendingMessageRepository.GetMessage(messageCode));
        }

    }
}

