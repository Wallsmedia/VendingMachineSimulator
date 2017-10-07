# Vending Machine Console Simulator 
Have done it as a challenge with a second goal to create for future some practical material to use for a junior member problem solving approach.

## Intorduction
The project implementation, i.e. source code requires that you should be have C# developer expertise in the latest (just hot baked) .NET technologies.
So, on the moment of the code writing here is the links (the moment on writing this text): 

### C#

What's new in C# 7
https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7

What's New in C# 6
https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-6

Relationships between language features and library types
https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/relationships-between-language-and-library

### Visual Studio 2017

Visual Studio 2017
https://docs.microsoft.com/en-us/visualstudio/

Additions to the csproj format for .NET Core
https://docs.microsoft.com/en-us/dotnet/core/tools/csproj

### .NET  

https://docs.microsoft.com/en-gb/dotnet/

NET architectural components
https://docs.microsoft.com/en-us/dotnet/standard/components

.NET Standard
https://docs.microsoft.com/en-us/dotnet/standard/net-standard

### Developemnt values, practices, principals

http://deviq.com/category/values/
http://deviq.com/category/principles/
http://deviq.com/category/patterns/
http://deviq.com/category/practices/
http://deviq.com/category/antipatterns/

### .NET Core Github / Nuget packages

The project implementation has been used the DI and Configuration which is a part of ASPNET project.
In order to understand implementation logics, you have to be familiar with them.
Here is the links + source to have a look:
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration?tabs=basicconfiguration
If doesn't help see the source: https://github.com/aspnet/Configuration

https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
If doesn't help see the source:https://github.com/aspnet/DependencyInjection


## Check your self 

Answer the easy question:
The console application has been build with targets .net 462 and .net core app 2.0 from libraries that support net standard 2.0.
What makes them to be build and run on Windows OS?

Verify your answer, just go to 
 ....\bin\Debug\net462\ 
 delete all System.*.dll except  "System.ValueTuple.dll" (c#7.0)
 and run exe again is it works? Ensure your self that you got right answer.
 
 ## Vending Machine (Simulator) Descriptinon
 
I have defined the function design of Vending Machine as following:

1. Machine sells "vending type" products, one product per order + payment transaction.

2. Machine has a stock repository; 
    - it has sections;
    - the number of sections has limited to 20;
    - each section has a code case-insensitive letter from 'A' till 'A'+20; 
    - each sections can have product only one type with virtual unlimited number to int.MaxInt;  

3. Machine has a coin "wallet" which is loaded with initial coins; 
    - it accumulates payment;
    - it is a source of change;    

4. Machine has a coin "receiver" witch is hard configured to exact accepted nominals. It simulate the real coin receivers.
   - current implementation is to accept UK coins, and accepts 5p,10p,20,50p and £1 coins.
  
5. Configuration file for Vending Machine defines:
   - initial load for coins "wallet"; coins from configuration checks for nominals that can be accepted by coin "receiver";
   - the product stock repository load and allow to load only 20 types of vending products.

6. Vending process:
- Turn "On"(simulator app. start): vending machine initialize components and load the configuration. Vending machine switch to state "Order"

"Order":
- Checks if there is stock to sell if it's not switch to state "OutOfStock"
- State "Order" allows to select product via the order panel.
  - Selected product triggers change state to "Payment"; 
  - Invalid selection cause error remark and remain in state to get order.
  - 
 "Payment":
 - Product has been "reserved" from stock.
 - State allows to make payment or cancel order.
 - Cancel of order: release payed coins, release reservation of product and switch back to state "Order".
 - Entering coin increase "payed" balance against "total" to pay for a product.
    - Payment is accumulated in the special wallet.
    - If "payed" balance is less than "total", state doesn't change and wait continue payment.
    - If "payed" balance is equal to "total", the payment add to the machine wallet. The sold product has been released.
    - If "payed" balance is greater than "total", the change is going to be combined:
        - If it is impossible to give change, the order has been canceled: release payed coins, release reservation of product and switch back to state "Order". 
        - If it is possible to give change: the change given, the payment add to the machine wallet. The sold product has been released and switch back to state "Order".

"Fault":
 -If any faults, errors go to state "Fault" state display message( terminate app. after 15 sec)

"OutOfStock":
- Machine display message like "out of stock"
 
"TurnedOff":
- The machine has been turned off.


## Vending Machine Simulator Solution

### Projects:
- Vending.Machine.Abstraction - implementation independent interfaces, models and so on.
- Vending.Machine.Console - the implementation of "Abstractions" into console simulator.
- Vending.Machine.App - the concrete aggregation into console based simulator, platform independent runtime "dotnet" and windows application. 

## Components role/use cases description:

### Vending.Machine.Console

#### DisplayPanel
- It implements the interface Vending.Machine.Abstraction.IDisplayPanel and have only one purpose to write a message to a console.

#### OrderPanel
- It implements the interface Vending.Machine.Abstraction.IOrderPanel and has only one purpose to get a product order from available in the stock.
- It uses and depend on via abstractions:
  - IDisplayPanel;
  - IReadKeypadInput;
  - IProductRepository;
  - IVendingMessageRepository;
  - 
#### IPaymentReceiver
- It implements the interface Vending.Machine.Abstraction.IPaymentReceiver and has only one purpose to get a coin payment from acceptable internal set.
- It uses and depend on via abstractions:
  - IDisplayPanel;
  - IReadKeypadInput;
  - IVendingMessageRepository;

#### ProductRepository
- It implements the interface Vending.Machine.Abstraction.IProductRepository and has only one purpose to hold a vending machine product stock.

#### WalletRepository
- It implements the interface Vending.Machine.Abstraction.IWalletRepository and has only one purpose to hold a vending machine money wallet.

####  ReadKeypadInput
- It implements the interface Vending.Machine.Console.Abstraction.IReadKeypadInput and has only one purpose to provide/simulate the keypad input.

#### VendingMessageRepository 
- It implements the interface Vending.Machine.Console.Abstraction.IVendingMessageRepository and has only one purpose to provide message repository.

#### VendingMachineController
- It implements the interface Vending.Machine.Abstraction.IVendingMachineController and has only one purpose to provide vending machine selling functional logics.
- It uses and depend on via abstractions:
  - IDisplayPanel;
  - IProductRepository;
  - IPaymentReceiver;
  - IOrderPanel;
  - IWalletRepository;
  - IVendingMessageRepository;
  - ISoldRecord;

#### SoldRecord 
- It implements the interface Vending.Machine.Console.Abstraction.ISoldRecord and has only one purpose to record sale operations. It is a stub in the simulator. 

### Vending.Machine.Console

#### Program
- The main purpose of the class is to aggregate components into a console application, handle initialization, configuration and errors.
- it is used the DI container to build and bind all dependencies.
- 
#### VendingConfiguration, CoinChangeDeposit, ProductToSell, VendingConfigurationExt
- The main purpose of the class is to be bind with vending machine simulator configuration file and provide population of the ProductRepository and WalletRepository at the program start up.
#### VendingSettings.json
- The configuration file  for the vending machine.


## Test
See project Vending.Machine.Test

