// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2017. All rights reserved.
// Wallsmedia LTD 2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.


using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Vending.Machine.Abstraction;
using Vending.Machine.App.Configuration;
using Vending.Machine.Console;
using Vending.Machine.Console.Abstract;

namespace Vending.Machine.App
{
    /// <summary>
    /// Standard class implementation to run Console Vending Machine Simulator. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            try
            {
                // Launch the machine simulator
                prog.RunApplicarion();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Vending Machine Fatal Failure :");
                if (ex is AggregateException ar)
                {
                    foreach (var ei in ar.InnerExceptions)
                    {
                        System.Console.WriteLine(ei.Message);
                    }
                }
                else
                {
                    System.Console.WriteLine(ex.Message);
                }
                System.Console.WriteLine("Call Service Support : XXX-XXX-XXXXX");
                Thread.Sleep(15000);
            }
        }

        void RunApplicarion()
        {

            ServiceProvider serviceProvider = InitalizeConsoleVendingMachineSimulator();

            LoadConfigurationVendingMachine(serviceProvider);

            // Construct with DI the vending machine controller
            var vending = serviceProvider.GetService<IVendingMachineController>();
            System.Console.WriteLine("Turn it On");
            Thread.Sleep(1500);

            // Start vending
            vending.On();

            // Wait until "turned off"
            vending.PowerOffDone.Wait();
        }

        /// <summary>
        /// Initialize the DI container  for a console vending machine case
        /// </summary>
        private ServiceProvider InitalizeConsoleVendingMachineSimulator()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IWalletRepository, WalletRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IReadKeypadInput, ReadKeypadInput>();
            services.AddSingleton<IDisplayPanel, DisplayPanel>();
            services.AddSingleton<IPaymentReceiver, PaymentReceiver>();
            services.AddSingleton<ISoldRecord, SoldRecord>();
            services.AddSingleton<IOrderPanel, OrderPanel>();
            services.AddSingleton<IVendingMessageRepository, VendingMessageRepository>();
            services.AddSingleton<IVendingMachineController, VendingMachineController>();

            return services.BuildServiceProvider();
        }

        private void LoadConfigurationVendingMachine(ServiceProvider serviceProvider)
        {
            var stock = serviceProvider.GetService<IProductRepository>();
            var change = serviceProvider.GetService<IWalletRepository>();
            var recv = serviceProvider.GetService<IPaymentReceiver>();

            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("VendingSettings.json");
            IConfigurationRoot config = configBuilder.Build();

            VendingConfiguration machineConfig = config.Get<VendingConfiguration>();

            if (machineConfig.ChangeWalletConfig.Count == 0)
            {
                throw new InvalidDataException("Empty Change Wallet");
            }

            if (machineConfig.ProductStockConfig.Count == 0)
            {
                throw new InvalidDataException("Empty Stock to Sell");
            }

            machineConfig.LoadProducts(stock);
            machineConfig.LoadWallet(change,recv);

        }
    }
}
