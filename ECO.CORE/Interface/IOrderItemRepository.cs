using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderItemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Interface
{
    public interface IOrderItemRepository
    {
        ShowOrderItemDTO GetById(int id);
        IntResultDTO AddOrderItem(AddOrderItemDTO item);
        IntResultDTO UpdateOrderItem(EditOrderItemDTO item);
        IntResultDTO DeleteOrderItem(int id);
    }
}
