using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderDTO;
using ECO.CORE.DTO.OrderItemDTO;
using ECO.CORE.Entities;
using ECO.CORE.Interface;
using ECO.EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EF.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly AppDbContext _context;
        public OrderRepository(IOrderItemRepository orderItemRepository, AppDbContext context)
        {
            _orderItemRepository = orderItemRepository;
            _context = context;
        }
        public IntResultDTO Add(AddOrderDTO order)
        {
            if(_context.Users.Find(order.UserId) is null)
            {
                return new IntResultDTO() { Massage = "No User has this Id" };
            }
            var newOrder = new Order(order.UserId);
            _context.Orders.Add(newOrder);
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    foreach (var item in order.OrderItems)
                    {
                        var result=_orderItemRepository.AddOrderItemInSpacificOrder(new AddOrderItemDTO() { OrderId = newOrder.Id, ProductId = item.ProductId, Quantity = item.Quantity });
                        if(result.Id==0) {
                            return new IntResultDTO { Massage = result.Massage };
                        }
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new IntResultDTO { Massage = ex.Message };
                }
            }
            return new IntResultDTO() { Id = newOrder.Id };
        }

        public IntResultDTO Delete(int id)
        {
            var order = Get(id);
            if(order is null)
            {
                return new IntResultDTO() { Massage = "No Order has this Id" };
            }
            if (order.IsRecieved)
            {
                return new IntResultDTO() { Massage = "The Order Is already Reseaved we could not delete" };
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in order.OrderItems)
                    {
                        var result=_orderItemRepository.DeleteOrderItem(item.Id);
                        if(result.Id==0)
                        {
                            return new IntResultDTO() { Massage = result.Massage };
                        }
                    }
                    _context.Orders.Remove(GetOrder(id));
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new IntResultDTO { Massage = ex.Message };
                }
            }
            return new IntResultDTO() { Id = 1 };

        }

        public ShowOrderDTO Get(int id)
        {
            return _context.Orders.Where(x => x.Id == id).Select(o => new ShowOrderDTO() { Id = o.Id,
                IsRecieved = o.IsRecieved,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                UserName = o.User != null? o.User.Name:"UnKnown",
                OrderItems = o.OrderItems.Select(x => new ShowOrderItemDTO() { Id = x.Id, Price = x.Price,
                ProductName = x.Product!=null? x.Product.Name:"Unknown", Quantity = x.Quantity }).ToList() }).FirstOrDefault();
        }

        public List<ShowOrderDTO> GetOrdersForUser(string userId)
        {
            return _context.Orders.Where(x => x.UserId == userId).Select(o => new ShowOrderDTO()
            {
                Id = o.Id,
                IsRecieved = o.IsRecieved,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                UserName = o.User != null ? o.User.Name : "UnKnown",
                OrderItems = o.OrderItems.Select(x => new ShowOrderItemDTO()
                {
                    Id = x.Id,
                    Price = x.Price,
                    ProductName = x.Product != null ? x.Product.Name : "Unknown",
                    Quantity = x.Quantity
                }).ToList()
            }).ToList();
        }

        public IntResultDTO IsReady(int id)
        {
            var order=_context.Orders.Include(x=>x.OrderItems).FirstOrDefault(x=>x.Id == id);
            if(order is null)
            {
                return new IntResultDTO() { Massage = "No Order has this Id" };
            }
            foreach(var item in order.OrderItems)
            {
                if(!item.IsReady)
                {
                    return new IntResultDTO() { Id = 2 };
                }
            }
            return new IntResultDTO() { Id = 1 };
        }

        public IntResultDTO MakeItReseved(int id)
        {
            var order = GetOrder(id);
            if(order == null)
            {
                return new IntResultDTO(){ Massage="there is no Order has this Id"};
            }
            order.IsRecieved = true;
            return new IntResultDTO() { Id = id };
        }
        Order GetOrder(int id) => _context.Orders.Find(id); 
    }
}
