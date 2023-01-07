using System;
using System.Collections.Generic;
using System.Text;

namespace EcPJ
{
    public class Product
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string img { get; set; }
        public string[] categories { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public int price { get; set; }
    }
}
