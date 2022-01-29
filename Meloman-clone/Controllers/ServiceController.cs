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
        public ServiceController(IPdfService service, IOrderRepository orderRepository, IBookRepository bookRepository)
        {
            _service = service;
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
        }
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public ActionResult DownloadBooksToPdf()
        {
            var books = _bookRepository.GetAllBooks();
            byte[] pdfFile = _service.DownloadBookListToPdf(books);
            string fileName = $"отчет_по_книгам_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.pdf";
            return File(pdfFile, "application/pdf", fileName);
        }
        //[Authorize(AuthenticationSchemes = "MelomanAuthCookie")]
        public ActionResult Report()
        {
            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            using (var doc = new Document(pdf))
            {
                doc.Add(new Paragraph("Hello World!"));
                doc.Close();
                pdfBytes = stream.ToArray();
            }
            return File(pdfBytes, "application/pdf", "file.pdf");
        }
    }
}
