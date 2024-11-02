using ECO.CORE.DTO;
using ECO.CORE.DTO.ProductDTO;
using ECO.CORE.Entities;
using ECO.CORE.Interface;
using ECO.EF.Data;
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

        public IntResultDTO Delete(int productId)
        {
            var product = GetProduct(productId);
            _context?.Products.Remove(product);
            try
            {
                _context.SaveChanges();
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
            if(pageSize == 0)
            {
                pageSize = 10;
            }
            if(page == 0) 
            { 
                page = 1;
            }
            var numOfPage=_context.Products.Count()/pageSize;
            page= Math.Min(page, numOfPage);
            var products = _context.Products.Skip(page * pageSize).Take(pageSize).ToList();
            var result =new List<ShowProductDTO>();
            foreach(var product in products)
            {
                result.Add(new ShowProductDTO(product));
            }
            return result;
        }

        public List<ShowProductDTO> GetProductsInPaginationsWithCategory(int page, int pageSize, int categoryId)
        {
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            if (page == 0)
            {
                page = 1;
            }
            var numOfPage = _context.Products.Where(x=>x.CategoryId==categoryId).Count() / pageSize;
            page = Math.Min(page, numOfPage);
            var products = _context.Products.Where(x=>x.CategoryId==categoryId).Skip(page * pageSize).Take(pageSize).ToList();
            var result = new List<ShowProductDTO>();
            foreach (var product in products)
            {
                result.Add(new ShowProductDTO(product));
            }
            return result;
        }

        public IntResultDTO Update(EditProductDTO product, int id)
        {
            var productDB=GetProduct(id);
            productDB.Price = product.Price;
            productDB.CategoryId = product.CategoryId;
            productDB.ImagePath = product.ImagePath;
            productDB.Description = product.Description;
            productDB.Name = product.Name;
            productDB.Quantity = product.Quantity;
            try
            {
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return new IntResultDTO() { Massage=ex.Message};
            }
            return new IntResultDTO() { Id=productDB.Id};
        }
        Product GetProduct(int id) => _context.Products.Find(id);
    }
}
