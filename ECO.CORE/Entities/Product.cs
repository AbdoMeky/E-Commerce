using ECO.CORE.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int Quantity { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string SellerId { get; set; }
        public Seller Seller { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Product()
        {
            
        }
        public Product(AddProductDTO product,string imagePath)
        {
            this.Name = product.Name;
            this.Description = product.Description;
            this.Price = product.Price;
            this.Quantity = product.Quantity;
            this.CategoryId = product.CategoryId;
            this.SellerId = product.SellerId;
            this.ImagePath = imagePath;
        }
    }
}
