using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderDTO;
using ECO.CORE.DTO.PaymentDTO;
using ECO.CORE.DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Interface
{
    public interface IUserRepository
    {
        StringResultDTO Edit(EditUserDTO user,string id);
        List<ShowOrderDTO> OrderUserDo(string id);
        bool Check(string Id);
    }
}
