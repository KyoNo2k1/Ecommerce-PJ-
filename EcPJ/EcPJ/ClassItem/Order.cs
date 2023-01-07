using System;
using System.Collections.Generic;
using System.Text;

namespace EcPJ.ClassItem
{
    public class Order
    {
        public string _id { get; set; }
        public string userId { get; set; }
        public ProductCart[] products { get; set; }
        public int amount { get; set; }
        public Address address { get; set; }
        public string status { get; set; }
    }
}
