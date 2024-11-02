using ECO.CORE.DTO;
using ECO.CORE.DTO.ProductDTO;
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
        IntResultDTO Update(EditProductDTO product,int id);
        
    }
}
