using Meloman_clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Repository
{
    public interface IOrderRepository
    {
        bool SaveBookOrder(Order order);
        bool EditOrder(int id, string status);
        List<Order> GetOrderList(string requestedBy, string userId);
        Order GetOrderById(int id);
    }
}
