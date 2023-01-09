using System;
using System.Collections.Generic;
using System.Text;

namespace EcPJ.ClassItem
{
    public class ProductQuan
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string img { get; set; }
        public string[] categories { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public int price { get; set; }
        public int quantity { get; set; } = 1;
        public int total { get; set; } = 0;
    }
}
