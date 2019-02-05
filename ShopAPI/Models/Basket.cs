using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.Models
{
    public class Basket
    {
        public int CustomerId { get; set; }
        public ICollection<int> ProductsId { get; set; } = new List<int>();
    }
}
