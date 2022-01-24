using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Meloman_clone.Data;
using Meloman_clone.Models;
using Meloman_clone.Dtos;
using Newtonsoft.Json;
using System.IO;
using Meloman_clone.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Meloman_clone.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookContext _context;
        private readonly IBookRepository _bookRepository;
        public BooksController(BookContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        // GET: Books
        public IActionResult Index()
        {
            return View(_bookRepository.GetAllBooks());
        }
        [HttpPost]
        public JsonResult FindBook(int id)
        {
            var book = _bookRepository.Find(id);
            if (book == null)
            {
                return new JsonResult("Does not exist!");
            }
            book = _bookRepository.GetBase64Image(book);

            return new JsonResult(book);
        }



        // GET: Books/Create
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public string SaveBook(BookDto bookDto)
        {
            Book book = JsonConvert.DeserializeObject<Book>(bookDto.Book);
            book = _bookRepository.AddAuthorPhoto(book);
            if (book.Discount != 0)
            {
                book.OldPrice = (book.Price * 100) / (100 - book.Discount);
            }
            if (bookDto.FrontPhoto.Length > 0)
            {
                using (var ms1 = new MemoryStream())
                {
                    bookDto.FrontPhoto.CopyTo(ms1);
                    var fileBytesFront = ms1.ToArray();
                    book.PhotoFront = fileBytesFront;
                }

                using (var ms2 = new MemoryStream())
                {
                    if (bookDto.BackPhoto != null)
                    {
                        bookDto.BackPhoto.CopyTo(ms2);
                        var fileBytesBack = ms2.ToArray();
                        book.PhotoBack = fileBytesBack;
                    }
                }
                book = _bookRepository.Save(book);
                if (book.BookId > 0)
                {
                    return "Saved";
                }
            }
            return "Failed";
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public string UpdateBook(BookDto bookDto)
        {
            Book book = JsonConvert.DeserializeObject<Book>(bookDto.Book);
            book = _bookRepository.AddAuthorPhoto(book);
            if (book.Discount != 0)
            {
                book.OldPrice = (book.Price * 100) / (100 - book.Discount);
            }
            if (bookDto.FrontPhoto.Length > 0)
            {
                using (var ms1 = new MemoryStream())
                {
                    bookDto.FrontPhoto.CopyTo(ms1);
                    var fileBytesFront = ms1.ToArray();
                    book.PhotoFront = fileBytesFront;
                }

                using (var ms2 = new MemoryStream())
                {
                    if (bookDto.BackPhoto != null)
                    {
                        bookDto.BackPhoto.CopyTo(ms2);
                        var fileBytesBack = ms2.ToArray();
                        book.PhotoBack = fileBytesBack;
                    }
                }
                book = _bookRepository.Update(book);
                if (book.BookId > 0)
                {
                    return "Updated";
                }
            }
            return "Failed";
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public JsonResult Delete(int id)
        {
            var book = _bookRepository.Find(id);
            if (_bookRepository.Delete(book))
            {
                return new JsonResult("success");
            }
            else
            {
                return new JsonResult("failure");
            }
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
            List<Book> fiveBooks = new List<Book>();

            for (int i = 1; i <= range; i++)
            {
                if (books[len - i].PhotoFront != null)
                {
                    books[len - i].PhotoFront = _bookRepository.GetImageBytes(Convert.ToBase64String(books[len - i].PhotoFront));
                }
                fiveBooks.Add(books[len - i]);
            }

            if (fiveBooks == null)
            {
                return new JsonResult("empty");
            }
            return new JsonResult(fiveBooks);
        }
        [HttpPost]
        public JsonResult BookNumberSortedByGenre(string genre)
        {
            int bookNumberFound = _bookRepository.GetBookNumberByGenre(genre);
            return new JsonResult(bookNumberFound);
        }
        [HttpPost]
        public JsonResult SortByGenre(int size, string genre)
        {
            List<Book> books = _bookRepository.GetBookSortedByGenre(size, genre);
            if(books != null)
            {
                return new JsonResult(books);
            }
            return new JsonResult("empty");
        }
        [HttpPost]
        public JsonResult RedirectToDetails(int id)
        {
            var book = _bookRepository.Find(id);
            if(book != null)
            {
                string fiction = "Художественная литература";
                string nonfiction = "Научно-популярная литература";
                string business = "Книги по бизнесу";
                string psychology = "Психологическая литература";
                string childbooks = "Детская литература";
                string category = book.Category.Trim(' ');
                if (category.Equals(fiction))
                {
                    return new JsonResult($"/Books/Fiction/{book.BookId}");
                }
                else if (category.Equals(nonfiction))
                {
                    return new JsonResult($"/Books/NonFiction/{book.BookId}");
                }
                else if (category.Equals(business))
                {
                    return new JsonResult($"/Books/Business/{book.BookId}");
                }
                else if(category.Equals(psychology))
                {
                    return new JsonResult($"/Books/Psychology/{book.BookId}");
                }
                else if(category.Equals(childbooks))
                {
                    return new JsonResult($"/Books/ForChildren/{book.BookId}");
                }
            }
            return new JsonResult("#");
        }
        public IActionResult Fiction(int id)
        {
            if(id == 0)
            {
                return View("Fiction", _bookRepository.GetBooksSortedByCategory("Художественная литература"));
            }
            var book = _bookRepository.Find((id));
            return View("Details", _bookRepository.GetBase64Image(book));
        }
        public IActionResult NonFiction(int id)
        {
            if (id == 0)
            {
                return View("NonFiction", _bookRepository.GetBooksSortedByCategory("Научно-популярная литература"));
            }
            var book = _bookRepository.Find((id));
            return View("Details", _bookRepository.GetBase64Image(book));
        }
        public IActionResult Business(int id)
        {
            if (id == 0)
            {
                return View("Business", _bookRepository.GetBooksSortedByCategory("Книги по бизнесу"));
            }
            var book = _bookRepository.Find((id));
            return View("Details", _bookRepository.GetBase64Image(book));
        }
        public IActionResult Psychology(int id)
        {
            if (id == 0)
            {
                return View("Psychology", _bookRepository.GetBooksSortedByCategory("Психологическая литература"));
            }
            var book = _bookRepository.Find((id));
            return View("Details", _bookRepository.GetBase64Image(book));
        }
        public IActionResult ForChildren(int id)
        {
            if (id == 0)
            {
                return View("ForChildren", _bookRepository.GetBooksSortedByCategory("Детская литература"));
            }
            var book = _bookRepository.Find((id));
            return View("Details", _bookRepository.GetBase64Image(book));
        }
 

        public JsonResult GetAllBooks()
        {
            return new JsonResult(_bookRepository.GetAllBooks());
        }
        [HttpPost]
        public JsonResult GetBooksSortedByCategory(string category)
        {
            var books = _bookRepository.GetBooksSortedByCategory(category);
            return new JsonResult(books);
        }
        [HttpPost]
        public JsonResult GetBooksSortedByPrice(int fromPrice, int toPrice, string category)
        {
            var books = _bookRepository.GetBooksSortedByPrice(fromPrice, toPrice, category);
            return new JsonResult(books);
        }
        [HttpPost]
        public JsonResult GetBooksSortedByAuthor(string[] authors, string category)
        {
            var books = _bookRepository.GetBooksSortedByAuthor(authors, category);
            return new JsonResult(books);
        }
        [HttpPost]
        public JsonResult GetSortedBooks(string category, string[] authors, string[] suggestedAges, string[] publishedYears, string[] covers, string[] genres )
        {
            var books = _bookRepository.GetSortedBooks(category, authors, suggestedAges, publishedYears, covers, genres);
            return new JsonResult(books);
        }
        [HttpPost]
        public JsonResult SortByIncreasingPrice(string category, string[] authors, string[] suggestedAges, string[] publishedYears, string[] covers, string[] genres)
        {
            var books = _bookRepository.GetSortedBooks(category, authors, suggestedAges, publishedYears, covers, genres);
            var sortedBooks = _bookRepository.SortByIncreasingPrice(books);
            return new JsonResult(sortedBooks);
        }

        [HttpPost]
        public JsonResult SortByDecreasingPrice(string category, string[] authors, string[] suggestedAges, string[] publishedYears, string[] covers, string[] genres)
        {
            var books = _bookRepository.GetSortedBooks(category, authors, suggestedAges, publishedYears, covers, genres);
            var sortedBooks = _bookRepository.SortByDecreasingPrice(books);
            return new JsonResult(sortedBooks);
        }

        [HttpPost]
        public JsonResult SortByIncreasingDiscount(string category, string[] authors, string[] suggestedAges, string[] publishedYears, string[] covers, string[] genres)
        {
            var books = _bookRepository.GetSortedBooks(category, authors, suggestedAges, publishedYears, covers, genres);
            var sortedBooks = _bookRepository.SortByIncreasingDiscount(books);
            return new JsonResult(sortedBooks);
        }

        [HttpPost]
        public JsonResult SortByDecreasingDiscount(string category, string[] authors, string[] suggestedAges, string[] publishedYears, string[] covers, string[] genres)
        {
            var books = _bookRepository.GetSortedBooks(category, authors, suggestedAges, publishedYears, covers, genres);
            var sortedBooks = _bookRepository.SortByDecreasingDiscount(books);
            return new JsonResult(sortedBooks);
        }

        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public IActionResult AddAuthor()
        {
            return View();
        }
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public IActionResult AddBookDescription(int id)
        {
            if(id == 0)
            {
                return View();
            }
            ViewBag.Id = id;
            return View();
            
        }
        [HttpPost]
        public JsonResult GetBookDescription(int bookid)
        {
            var bookDescription = _context.BookDescriptions.FirstOrDefault(e => e.BookId == bookid);
            if(bookDescription != null)
            {
                return new JsonResult(bookDescription.Description);
            }
            return new JsonResult("empty");
        }
        [HttpPost]
        public string SaveBookDescription(int bookid, string citation, string description, string about)
        {
            BookDescription bookDescription = new BookDescription();
            bookDescription.BookId = bookid;
            bookDescription.Citation = citation;
            bookDescription.Description = description;
            bookDescription.About = about;
            bookDescription = _bookRepository.SaveBookDescription(bookDescription);
            if(bookDescription.Id > 0)
            {
                return "Saved";
            }
            return "Failed";
        }
        [HttpPost]
        public string SaveAuthor(BookAuthorDto bookAuthorDto)
        {
            BookAuthor author = JsonConvert.DeserializeObject<BookAuthor>(bookAuthorDto.Author);
            author.Name = author.Name.Trim(' ');
            if(bookAuthorDto.file.Length > 0)
            {
                using(var ms = new MemoryStream())
                {
                    bookAuthorDto.file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    author.Photo = fileBytes;
                    author = _bookRepository.SaveAuthor(author);
                    if(author.BookAuthorId > 0)
                    {
                        return "Saved";
                    }
                }
            }
            return "Failed";
        }
        public JsonResult GetAuthors()
        {
            return new JsonResult(_bookRepository.GetAllBookAuthors());
        }
        [HttpPost]
        public JsonResult GetAuthorsOfExistingBooks(string category)
        {
            return new JsonResult(_bookRepository.GetExistingBookAuthors(category));
        }
        [HttpPost]
        public JsonResult GetGenresOfExistingBooks(string category)
        {
            return new JsonResult(_bookRepository.GetExistingBookGenres(category));
        }
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
        [HttpPost]
        public JsonResult GetFinalPrice(int[] bookIds, string operation)
        {
            return new JsonResult(_bookRepository.GetFinalPrice(bookIds, operation));
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAdminCookie")]
        public string DeleteAuthor(int id)
        {
            if (_bookRepository.DeleteAuthor(id))
            {
                return "success";
            }
            return "fail";
        }
    }
}
