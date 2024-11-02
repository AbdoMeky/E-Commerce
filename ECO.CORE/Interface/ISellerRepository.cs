using ECO.CORE.DTO.UserDTO;
using ECO.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.CORE.DTO.SellerDTO;

namespace ECO.CORE.Interface
{
    public interface ISellerRepository
    {
        StringResultDTO Edit(EditSellerDTO seller, string id);
    }
}
