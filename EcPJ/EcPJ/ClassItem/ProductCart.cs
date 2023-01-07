using System;
using System.Collections.Generic;
using System.Text;

namespace EcPJ.ClassItem
{
    public class ProductCart
    {
        public string _id { get; set; }
        public string productId { get; set; }
        public int quantity { get; set; } = 1;
    }
}
