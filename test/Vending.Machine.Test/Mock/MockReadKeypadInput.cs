using System.Threading;
using Vending.Machine.Console.Abstract;

namespace Vending.Machine.Test.Mock
{
        class MockReadKeypadInput : IReadKeypadInput
        {
            public char SimInputAs { get; set; } = ' ';

            public char ReadInput(CancellationToken cancellationToken)
            {
                return SimInputAs;
            }
        }

    
}
