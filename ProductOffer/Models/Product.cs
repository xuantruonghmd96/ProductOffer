using System;
using System.Collections.Generic;
using System.Text;

namespace ProductOffer.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int TagId { get; set; }
    }
}
