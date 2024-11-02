using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderDTO;
using ECO.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EF.Repository
{
    public class OrderItemRepository : IOrderRepository
    {
        public IntResultDTO Add(string UserId)
        {
            throw new NotImplementedException();
        }

        public IntResultDTO Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ShowOrderDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<ShowOrderDTO> GetOrdersForUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IntResultDTO MakeItReseved(int id)
        {
            throw new NotImplementedException();
        }
    }
}
