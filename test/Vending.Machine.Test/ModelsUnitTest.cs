using Vending.Machine.Abstraction.Models;
using Xunit;

namespace Vending.Machine.Test
{
    public class ModelsUnitTest
    {
       
        [Fact]
        public void CoinTestEquals()
        {
            Coin m100 = new Coin(100);
            Coin m200 = new Coin(200);
            Coin d100 = new Coin(100);
            Assert.Equal(m100, d100);
            Assert.NotEqual(m100, m200);
        }

    }
}
