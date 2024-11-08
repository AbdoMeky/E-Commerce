using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderItemDTO;
using ECO.CORE.DTO.ProductDTO;
using ECO.CORE.Entities;
using ECO.CORE.Interface;
using ECO.EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EF.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IntResultDTO> Add(AddProductDTO product)
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            if(_context.Sellers.Find(product.SellerId) is null)
            {
                return new IntResultDTO() { Massage = "Id not valid" };
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(product.Image.FileName).ToLower();//extintion like .png or .jpg
            var contentType = product.Image.ContentType.ToLower();//mime type like image/jpeg
            if (!allowedExtensions.Contains(fileExtension) || !contentType.StartsWith("image/"))
            {
                return new IntResultDTO() { Massage= "Invalid file type, Only images are allowed" };
            }
            var fileName = Path.GetRandomFileName() + Path.GetExtension(product.Image.FileName);
            var filePath = Path.Combine(_storagePath, fileName);
            var newProduct = new Product(product, filePath);
            _context.Products.Add(newProduct);
            try
            {
                await _context.SaveChangesAsync();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.Image.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                return new IntResultDTO() { Massage = ex.Message };
            }
            return new IntResultDTO() { Id=newProduct.Id};
        }

        public ItemPriceDTO Decrease(int productId, int quantityNeeded)
        {
            var product=GetProduct(productId);
            if(product == null)
            {
                return new ItemPriceDTO() { Massage = "No Product has this Id" };
            }
            if(product.Quantity-quantityNeeded < 0) 
            {
                return new ItemPriceDTO() { Massage = "There just " + product.Quantity +" of " + product.Name };
            }
            product.Quantity-=quantityNeeded;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ItemPriceDTO() { Massage = ex.Message };
            }
            return new ItemPriceDTO() { price = product.Price*quantityNeeded };
        }

        public IntResultDTO Delete(int productId)
        {
            var product = GetProduct(productId);
            if(product is null)
            {
                return new IntResultDTO() { Massage = "No product with this id" };
            }
            _context.Products.Remove(product);
            try
            {
                _context.SaveChanges();
                if(File.Exists(product.ImagePath))
                {
                    File.Delete(product.ImagePath);
                }
            }
            catch(Exception ex) { return new IntResultDTO() { Massage=ex.Message }; }
            return new IntResultDTO() { Id =1};
        }

        public ShowOneProductDTO GetProductById(int productId)
        {
            var product = _context.Products.Where(x => x.Id == productId).Select(x => new ShowOneProductDTO()
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                CategoryName = x.Category.Name,
                SellerName = x.Seller.Name
            }).FirstOrDefault();
            if(product is null)
            {
                return null;
            }
            return product;
        }

        public List<ShowProductDTO> GetProductsInPaginationsNoCategory(int page, int pageSize)
        {
            page = Math.Max(page, 1);
            if(pageSize == 0)
            {
                pageSize = 10;
            }
            var numOfProduct = _context.Products.Count();
            var numOfPage=(int)Math.Ceiling((decimal)numOfProduct/pageSize);
            page= Math.Min(page, numOfPage);
            var products = _context.Products.Skip(Math.Max((page - 1) * pageSize, 0)).Take(pageSize).Select(x => new ShowProductDTO() { Id = x.Id, Description = x.Description, Name = x.Name, Price = x.Price, ImagePath = x.ImagePath }).ToList();
            return products;
        }

        public List<ShowProductDTO> GetProductsInPaginationsWithCategory(int page, int pageSize, int categoryId)
        {
            page = Math.Max(page, 1);
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            var numOfProduct = _context.Products.Count(x => x.CategoryId == categoryId);
            var numOfPage = (int)Math.Ceiling((decimal)numOfProduct / pageSize);
            page = Math.Min(page, numOfPage);
            var products = _context.Products.Where(x => x.CategoryId == categoryId).Skip(Math.Max((page - 1)* pageSize,0)).Take(pageSize).Select(x => new ShowProductDTO() { Id = x.Id, Description = x.Description, Name = x.Name, Price = x.Price, ImagePath = x.ImagePath }).ToList().ToList();
            return products;
        }

        public async Task<IntResultDTO> Update(EditProductDTO product, int id)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(product.Image.FileName).ToLower();//extintion like .png or .jpg
            var contentType = product.Image.ContentType.ToLower();//mime type like image/jpeg
            if (!allowedExtensions.Contains(fileExtension) || !contentType.StartsWith("image/"))
            {
                return new IntResultDTO() { Massage = "Invalid file type, Only images are allowed" };
            }
            var fileName = Path.GetRandomFileName() + Path.GetExtension(product.Image.FileName);
            var filePath = Path.Combine(_storagePath, fileName);
            var productDB=GetProduct(id);
            var oldPath = productDB.ImagePath;
            productDB.Price = product.Price;
            productDB.CategoryId = product.CategoryId;
            productDB.Description = product.Description;
            productDB.Name = product.Name;
            productDB.Quantity = product.Quantity;
            productDB.ImagePath = filePath;
            try
            {
                await _context.SaveChangesAsync();
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.Image.CopyToAsync(stream);
                }
            }
            catch(Exception ex)
            {
                return new IntResultDTO() { Massage=ex.Message};
            }
            return new IntResultDTO() { Id=productDB.Id};
        }
        public Product GetProductWithItems(int productId)
        {
            return _context.Products.Include(x=>x.OrderItems).FirstOrDefault(x=>x.Id == productId);
        }
        public string ShowProductSeller(int productId)
        {
            var product=GetProduct(productId);
            if(product is null)
            {
                return string.Empty;
            }
            return product.SellerId;
        }
        Product GetProduct(int id) => _context.Products.Find(id);
    }
}
