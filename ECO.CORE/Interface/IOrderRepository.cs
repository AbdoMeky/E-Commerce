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
        List<ShowOrderDTO> GetOrdersForUser(string userId);
        IntResultDTO Add(AddOrderDTO order);
        IntResultDTO MakeItReseved(int id);
        IntResultDTO Delete(int id);
        IntResultDTO IsReady(int id);
    }
}
