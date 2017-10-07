using System.IO;
using Vending.Machine.Abstraction.Models;
using Xunit;

namespace Vending.Machine.Test
{
    public class MoneyUnitTest
    {
        [Fact]
        public void TestFractionsZero()
        {
            Money money = new Money();
            Assert.Equal(0, money.Fraction);
            Assert.Equal(Money.Zero, money);
        }

        [Fact]
        public void TestFractionsNonZero()
        {
            int n1 = 1;
            int f1 = 100;
            Money money = new Money(n1, f1);
            Assert.Equal(new Money(2, 0), money);
        }

        [Fact]
        public void TestFractionsOneToOne()
        {
            int n1 = 1;
            int f1 = 1;
            Money money = new Money(n1, f1);
            Assert.Equal(new Money(1, 1), money);
        }


        [Fact]
        public void TestSum()
        {
            int n1 = 1;
            int f1 = 1;
            Money money = new Money(n1, f1);
            money = money + money;
            Assert.Equal(new Money(2, 2), money);
        }

        [Fact]
        public void TestSubstract()
        {
            int n1 = 3;
            int f1 = 0;
            Money money = new Money(n1, f1);
            money = money - new Money(0, 1);
            Assert.Equal(new Money(2, 99), money);
        }

        [Fact]
        public void TestLogical()
        {
            Money money1 = new Money(2, 22);
            Money money1E = new Money(2, 22);
            Money money2 = new Money(3, 33);
            Assert.True(money1 < money2);
            Assert.True(money2 > money1);
            Assert.True(money2 != money1);
            Assert.True(money1 == money1E);
            Assert.True(money1 >= money1E);
            Assert.True(money1 <= money1E);
            Assert.False(money1.Equals(null));
        }

        [Fact]
        public void TestException()
        {
            Assert.Throws<InvalidDataException>(()=>new Money(1, -1));
            Assert.Throws<InvalidDataException>(()=>new Money(-1, 0));
        }


    }
}
