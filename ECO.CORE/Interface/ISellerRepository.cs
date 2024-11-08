using ECO.CORE.DTO.UserDTO;
using ECO.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.CORE.DTO.SellerDTO;
using ECO.CORE.DTO.ProductDTO;

namespace ECO.CORE.Interface
{
    public interface ISellerRepository
    {
        StringResultDTO Edit(EditSellerDTO seller, string id);
        List<ProductOrderedFromSellerDTO> HowManyQuantityNeededFromSellerInEachProduct(string id);
        ProductOrderedFromSellerDTO HowManyQuantityNeededFromProduct(int id);
        IntResultDTO CheckProductOwnership(string id,int productId);
        bool Check(string id);
    }
}
