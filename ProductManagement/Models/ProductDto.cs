using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public decimal VatPrice { get; set; }
        public long VatPercentage { get; set; }
        public List<string> Shops { get; set; }
    }
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public long GroupId { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "The Price cannot be negative.")]
        public decimal Price { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "The VatPrice cannot be negative.")]
        public decimal VatPrice { get; set; }

        [Range(0, 100, ErrorMessage = "The Vat% must be between 0 and 100.")]
        public long VatPercentage { get; set; }
        public List<int> ShopId { get; set; }
    }
}


