using Meloman_clone.Models;
using Meloman_clone.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Meloman_clone.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository)
        {
            _bookRepository = bookRepository;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAuthCookie")]
        public JsonResult AddBookOrder(string[] order, int[] bookIds)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            for(int i = 0; i < bookIds.Length; i++)
            {
                var book = _bookRepository.Find(bookIds[i]);
                var orderProduct = new OrderProduct
                {
                    ProductId = book.BookId,
                    ProductType = "Книга",
                    ProductName = $"{book.Author}: {book.Name}"
                };
                orderProducts.Add(orderProduct);
            }
            var newOrder = new Order();
            newOrder.Telephone = order[0];
            newOrder.Name = order[1];
            newOrder.Email = order[2];
            newOrder.City = order[3];
            newOrder.DeliveryType = order[4];
            newOrder.Address = order[5];
            newOrder.IsExpressDelivery = order[6];
            newOrder.PaymentType = order[7];
            newOrder.InitialPrice = Convert.ToInt32(order[8]);
            newOrder.Discount = Decimal.Parse(order[9], CultureInfo.InvariantCulture);
            newOrder.FinalPrice = Convert.ToInt32(order[10]);
            newOrder.Comments = order[11];
            newOrder.Date = DateTime.Now.ToShortDateString();
            newOrder.Products = orderProducts;
            newOrder.UserId = User.Identities.FirstOrDefault().Claims.FirstOrDefault(e => e.Type == "UserId").Value;
            newOrder.DeliveryStatus = "в пути";
            if (_orderRepository.SaveBookOrder(newOrder))
            {
                return new JsonResult("Ваш заказ принят!");
            }
            return new JsonResult("Ваш заказ не удался!");
        }
        
        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAuthCookie")]
        public JsonResult GetOrderList(string requestedBy)
        {
            if(requestedBy == "admin")
            {
                return new JsonResult(_orderRepository.GetOrderList(requestedBy, null));
            }
            else if(requestedBy == "user")
            {
                string userId = User.Identities.FirstOrDefault().Claims.FirstOrDefault(e => e.Type == "UserId").Value;
                var orders = _orderRepository.GetOrderList(requestedBy, userId);
                return new JsonResult(orders);
            }
            return new JsonResult("fail");
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAuthCookie")]
        public JsonResult GetOrder(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            return new JsonResult(order);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public JsonResult EditOrder(int id, string status)
        {
            if(_orderRepository.EditOrder(id, status))
            {
                return new JsonResult("Изменение прошло успешно!");
            }
            return new JsonResult("Изменение не удалось!");
        }

    }
}
