using ECO.CORE.DTO;
using ECO.CORE.DTO.OrderDTO;
using ECO.CORE.DTO.OrderItemDTO;
using ECO.CORE.Entities;
using ECO.CORE.Interface;
using ECO.EF.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EF.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        public readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        public OrderItemRepository(AppDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;

        }

        public IntResultDTO AddOrderItemInOrderDidnotReseived(AddOrderItemInOrderDTO item,string userId)
        {
            if (_context.Users.Find(userId) is null)
            {
                return new IntResultDTO() { Massage = "No User has this Id" };
            }
            var user = _context.Users.Include(x=>x.Orders).FirstOrDefault(x=>x.Id==userId);
            if(user is null)
            {
                return new IntResultDTO() { Massage="No User has this Id"};
            }
            var order = user.Orders.Where(x=>!x.IsRecieved).FirstOrDefault();
            if(order is null)
            {
                order = new Order(userId);
                _context.Orders.Add(order);
            }
            order.OrderDate = DateTime.Now;
            var resultId=0;
            using (var transaction=_context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    var result = AddOrderItemInSpacificOrder(new AddOrderItemDTO() { OrderId = order.Id, ProductId = item.ProductId, Quantity = item.Quantity });
                    if (result.Id==0) 
                    {
                        return new IntResultDTO() { Massage = result.Massage };
                    }
                    resultId = result.Id;
                    transaction.Commit();
                }catch(Exception ex)
                {
                    return new IntResultDTO() { Massage = ex.Message };
                }
            }
            return new IntResultDTO() { Id = resultId };
        }
        public IntResultDTO MakeItReady(int id)
        {
            var item = Get(id);
            if (item is null)
            {
                return new IntResultDTO() { Massage = "No Order Item has this Id" };
            }
            item.IsReady = true;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResultDTO() { Massage = ex.Message };
            }
            return new IntResultDTO() { Id = item.Id };
        }
        public IntResultDTO AddOrderItemInSpacificOrder(AddOrderItemDTO item)
        {
            var newItem = new OrderItem(item);
            var order = _context.Orders.Find(item.OrderId);
            if(order is null)
            {
                return new IntResultDTO() { Massage = "No Order has this Id" };
            }
            if(order.IsRecieved)
            {
                return new IntResultDTO() { Massage = "The Order has reseived ,we could not change it" };
            }
            try
            {
                var result = _productRepository.Decrease((int)item.ProductId, item.Quantity);
                if (!string.IsNullOrEmpty(result.Massage))
                {
                    return new IntResultDTO() { Massage = result.Massage };
                }
                newItem.Price = (decimal)result.price;
                _context.OrderItems.Add(newItem);
                order.TotalPrice += newItem.Price;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResultDTO { Massage = ex.Message };
            }
            return new IntResultDTO { Id = newItem.Id };
        }
        public IntResultDTO DeleteOneOrderItem(int id)
        {
            var item = Get(id);
            if (item == null)
            {
                return new IntResultDTO() { Massage = "No Order Item has this Id" };
            }
            _context.OrderItems.Remove(item);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = _productRepository.Decrease((int)item.ProductId, -1 * (int)item.Quantity);
                    if (!string.IsNullOrEmpty(result.Massage) && result.Massage != "No Product has this Id")
                    {
                        return new IntResultDTO() { Massage = result.Massage };
                    }
                    var order = _context.Orders.Find(item.OrderId);
                    if (order is not null)
                    {
                        order.TotalPrice -= item.Price;
                    }
                    _context.SaveChanges();
                    transaction.Commit(); 
                }
                catch (Exception ex)
                {
                    return new IntResultDTO() { Massage = ex.Message };
                }
            }
            return new IntResultDTO() { Id = 1 };
        }
        public IntResultDTO DeleteOrderItem(int id)
        {
            var item = Get(id);
            if (item == null)
            {
                return new IntResultDTO() { Massage = "No Order Item has this Id" };
            }
            _context.OrderItems.Remove(item);
            try
            {
                var result = _productRepository.Decrease((int)item.ProductId, -1 * (int)item.Quantity);
                if (!string.IsNullOrEmpty(result.Massage) && result.Massage != "No Product has this Id")
                {
                    return new IntResultDTO() { Massage = result.Massage };
                }
                var order = _context.Orders.Find(item.OrderId);
                if (order is not null)
                {
                    order.TotalPrice -= item.Price;
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResultDTO() { Massage = ex.Message };
            }
            return new IntResultDTO() { Id = 1 };
        }

        public ShowOrderItemDTO GetById(int id)
        {
            var item = _context.OrderItems.Where(x => x.Id == id).Select(x => new ShowOrderItemDTO() { IsReady=x.IsReady,Id = x.Id, Price = x.Price, ProductName = x.Product.Name, Quantity = x.Quantity }).FirstOrDefault();
            return item;
        }

        public decimal GetPrice(int id)
        {
            return Get(id).Price;
        }

        public IntResultDTO UpdateOrderItem(EditOrderItemDTO item,int id)
        {
            var oldItem = _context.OrderItems.Include(x=>x.Order).FirstOrDefault(x=>x.Id==id);
            if(oldItem == null)
            {
                return new IntResultDTO { Massage = "No Order Item has this Id" };
            }
            if (oldItem.Order.IsRecieved)
            {
                return new IntResultDTO { Massage = "The Order is already Reseaved" };
            }
            var changedQuantity=item.Quantity -oldItem.Quantity;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = _productRepository.Decrease((int)oldItem.ProductId, changedQuantity);
                    if (!string.IsNullOrEmpty(result.Massage))
                    {
                        return new IntResultDTO() { Massage = result.Massage };
                    }
                    oldItem.Price -= (decimal)result.price;
                    oldItem.Order.TotalPrice -= (decimal)result.price;
                    oldItem.IsReady = false;
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return new IntResultDTO { Massage = ex.Message };
                }
            }
            return new IntResultDTO { Id = oldItem.Id };
        }
        OrderItem Get(int id) => _context.OrderItems.Find(id); 
    }
}
