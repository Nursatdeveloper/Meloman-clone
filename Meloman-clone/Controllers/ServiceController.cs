using iText.Kernel.Pdf;
using iText.Layout;
using Meloman_clone.Repository;
using Meloman_clone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;


namespace Meloman_clone.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IPdfService _service;
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        public ServiceController(IPdfService service, IOrderRepository orderRepository, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _service = service;
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public ActionResult DownloadBooksToPdf()
        {
            var books = _bookRepository.GetAllBooks();
            byte[] pdfFile = _service.DownloadBookListToPdf(books);
            string fileName = $"отчет_по_книгам_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            return File(pdfFile, "application/pdf", fileName);
        }
        [Authorize(AuthenticationSchemes = "MelomanAuthCookie")]
        public ActionResult DownloadOrderToPdf(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            string userName = _userRepository.FindById(Int32.Parse(order.UserId)).Name;
            byte[] orderPdfFile = _service.DownloadOrderDetailsToPdf(order, userName);
            string fileName = $"чек_заказа_№{id}.pdf";
            return File(orderPdfFile, "application/pdf", fileName);
        }

    }
}
