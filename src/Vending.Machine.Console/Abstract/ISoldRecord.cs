using Vending.Machine.Abstraction.Models;

namespace Vending.Machine.Console.Abstract
{
    /// <summary>
    /// Log of sale Records 
    /// </summary>
    public interface ISoldRecord  
    {
        /// <summary>
        /// Records the sale of a product.
        /// </summary>
        /// <param name="product">The sold product.</param>
        void Sold(Product product);
    }
}