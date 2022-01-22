using Meloman_clone.Data;
using Meloman_clone.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }
        public bool SaveBookOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool EditOrder(int id, string status)
        {
            try
            {
                var order = GetOrderById(id);
                order.DeliveryStatus = status;
                _context.Orders.Update(order);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Order> GetOrderList(string requestedBy, string userId)
        {
            if(requestedBy == "admin")
            {
                var orders = _context.Orders.ToList();
                return orders;
            }
            else if (requestedBy == "user")
            {
                var orderList = _context.Orders
                    .Include(e => e.Products)
                    .ToList();
                var orders = from order in orderList
                             where order.UserId == userId
                             select order;

                return orders.ToList();
            }
            return null;//change later
        }
        public Order GetOrderById(int id)
        {
            var order = _context.Orders.Include(e => e.Products).FirstOrDefault(e => e.OrderId == id);
            return order;
        }
    }
}
