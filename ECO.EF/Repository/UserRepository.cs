using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderDTO;
using ECO.CORE.DTO.UserDTO;
using ECO.CORE.Entities;
using ECO.CORE.Interface;
using ECO.EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EF.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IOrderRepository _orderRepository;
        public UserRepository(AppDbContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }
        public StringResultDTO Edit(EditUserDTO user, string id)
        {
            var userDB = _context.Users.Find(id);
            if (userDB == null)
            {
                return new StringResultDTO() { Massage = "No User has this id" };
            }
            userDB.Name = user.Name;
            userDB.PhoneNumber = user.PhoneNumber;
            userDB.City = user.City;
            userDB.Address = user.Address;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new StringResultDTO() { Massage = ex.Message };
            }
            return new StringResultDTO() { Id = id };
        }
        public bool Check(string Id)
        {
            return _context.Users.Find(Id) is not null;
        }
        public List<ShowOrderDTO> OrderUserDo(string id)
        {
            return _orderRepository.GetOrdersForUser(id);
        }
    }
}
