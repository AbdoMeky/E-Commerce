using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderItemDTO;
using ECO.CORE.DTO.ProductDTO;
using ECO.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Interface
{
    public interface IProductRepository
    {
        List<ShowProductDTO> GetProductsInPaginationsNoCategory(int page, int pageSize);
        List<ShowProductDTO> GetProductsInPaginationsWithCategory(int page, int pageSize,int categoryId);
        ShowOneProductDTO GetProductById(int productId);
        Task<IntResultDTO> Add(AddProductDTO product);
        IntResultDTO Delete(int productId);
        Task<IntResultDTO> Update(EditProductDTO product,int id);
        ItemPriceDTO Decrease(int productId,int quantityNeeded);
        Product GetProductWithItems(int productId); 
        string ShowProductSeller(int productId);
    }
}
