using Meloman_clone.Data;
using Meloman_clone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookContext _context;

        public HomeController(ILogger<HomeController> logger, BookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public byte[] GetFrontImage(string Base64String)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(Base64String))
            {
                bytes = Convert.FromBase64String(Base64String);
            }
            return bytes;
        }
        public byte[] GetBackImage(string Base64String)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(Base64String))
            {
                bytes = Convert.FromBase64String(Base64String);
            }
            return bytes;
        }
        [HttpGet]
        public JsonResult GetLastFiveBooks()
        {
            var books = _context.Books.ToArray();
            int len = books.Length;
            int range = 5;
            if (len < 5)
            {
                range = len;
            }
            List<Book> fiveBooks = null;

            for (int i = 1; i <= range; i++)
            {
                if (books[len - i].PhotoFront != null)
                {
                    books[len - i].PhotoFront = this.GetFrontImage(Convert.ToBase64String(books[len - i].PhotoFront));
                }
                fiveBooks.Add(books[len - i]);
            }

            if (fiveBooks == null)
            {
                return new JsonResult("empty");
            }
            return new JsonResult(fiveBooks);
        }
        public IActionResult Index()
        {
            var books = _context.Books.ToArray();
            int len = books.Length;
            Book[] fiveBooks = new Book[5];
            List<Book> fiveBooksList = new List<Book>();
            int range = 5;
            if(len < 5)
            {
                range = len;
            }
            for (int i = 1; i <= range; i++)
            {
                if(books[len-i].PhotoFront != null)
                {
                    books[len - i].PhotoFront = this.GetFrontImage(Convert.ToBase64String(books[len - i].PhotoFront));
                }
                fiveBooks[i - 1] = books[len - i];
                fiveBooksList.Add(books[len - i]);
            }
            if (fiveBooks == null)
            {
                ViewBag.Books = null;
            }
            ViewBag.BookNumber = range;
            ViewBag.Books = fiveBooks;
            ViewBag.BookList = fiveBooksList;
            //JsonConvert.SerializeObject(fiveBooksList);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
