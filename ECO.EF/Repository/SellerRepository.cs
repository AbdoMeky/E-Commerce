using ECO.CORE.DTO;
using ECO.CORE.DTO.ProductDTO;
using ECO.CORE.DTO.SellerDTO;
using ECO.CORE.Interface;
using ECO.EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EF.Repository
{
    public class SellerRepository : ISellerRepository
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        public SellerRepository(AppDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;

        }
        public StringResultDTO Edit(EditSellerDTO seller, string id)
        {
            var sellerDB = _context.Sellers.Find(id);
            if (sellerDB == null)
            {
                return new StringResultDTO() { Massage = "No Seller has this id" };
            }
            sellerDB.Name = seller.Name;
            sellerDB.PhoneNumber = seller.PhoneNumber;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new StringResultDTO() { Massage=ex.Message};
            }
            return new StringResultDTO() { Id = id };
        }
        public ProductOrderedFromSellerDTO HowManyQuantityNeededFromProduct(int id)
        {
            var product=_productRepository.GetProductWithItems(id);
            if(product == null)
            {
                return null;
            }
            return new ProductOrderedFromSellerDTO(product);
        }
        public List<ProductOrderedFromSellerDTO> HowManyQuantityNeededFromSellerInEachProduct(string id)
        {
            var seller = _context.Sellers.Include(x => x.Products).ThenInclude(x => x.OrderItems).FirstOrDefault(x => x.Id == id);
            if(seller == null)
            {
                return null;
            }
            var result =new List<ProductOrderedFromSellerDTO>();
            foreach (var product in seller.Products)
            {
                result.Add(new ProductOrderedFromSellerDTO(product));
            }
            return result;
        }
        public IntResultDTO CheckProductOwnership(string id, int productId)
        {
            var productOwner= _productRepository.ShowProductSeller(productId);
            if(string.IsNullOrEmpty(productOwner))
            {
                return new IntResultDTO() { Massage = "No Product has this Id" };
            }
            if(productOwner == id)
            {
                return new IntResultDTO() { Id = 1 };
            }
            return new IntResultDTO() { Massage = "The Product not in your Products" };
        }
        public bool Check(string id) => _context.Sellers.Find(id) is not null;
    }
}
