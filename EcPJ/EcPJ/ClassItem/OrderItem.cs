using System;
using System.Collections.Generic;
using System.Text;

namespace EcPJ.ClassItem
{
    public class OrderItem
    {
        public string _id { get; set; }
        public string userId { get; set; }
        public ProductCart[] products { get; set; }
    }
}
