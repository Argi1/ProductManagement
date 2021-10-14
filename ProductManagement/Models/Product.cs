using System;
using System.Collections.Generic;

namespace ProductManagement.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public long GroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public decimal VatPrice { get; set; }
        public long VatPercentage { get; set; }
        public List<String> Shops { get; set; }
    }
}