using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Interface
{
    public interface IOrderRepository
    {
        ShowOrderDTO Get(int id);
        List<ShowOrderDTO> GetOrdersForUser(int userId);
        IntResultDTO Add(string UserId);
        IntResultDTO MakeItReseved(int id);
        IntResultDTO Delete(int id);
    }
}
