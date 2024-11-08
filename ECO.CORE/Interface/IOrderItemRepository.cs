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
        IntResultDTO AddOrderItemInSpacificOrder(AddOrderItemDTO item);
        IntResultDTO UpdateOrderItem(EditOrderItemDTO item,int id);
        IntResultDTO DeleteOrderItem(int id);
        decimal GetPrice(int id);
        IntResultDTO AddOrderItemInOrderDidnotReseived(AddOrderItemInOrderDTO item,string userId);
        IntResultDTO MakeItReady(int id);
    }
}
