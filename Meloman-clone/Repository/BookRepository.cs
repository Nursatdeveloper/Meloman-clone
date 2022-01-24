using Meloman_clone.Data;
using Meloman_clone.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Meloman_clone.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _context;
        public BookRepository(BookContext context)
        {
            _context = context;
        }

        public Book AddAuthorPhoto(Book book)
        {
            string authorName = book.Author.Trim(' ');
            var bookAuthor = _context.BookAuthors.FirstOrDefault(e => e.Name == book.Author);
            if(bookAuthor == null)
            {
                return book;
            }
            book.AuthorPhoto = bookAuthor.Photo;
            return book;
        }

        public Book Find(int id)
        {
            return _context.Books.Find(id);
        }

        public byte[] GetImageBytes(string Base64String)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(Base64String))
            {
                bytes = Convert.FromBase64String(Base64String);
            }
            return bytes;
        }

        public Book GetBase64Image(Book book)
        {
            book.PhotoFront = GetImageBytes(Convert.ToBase64String(book.PhotoFront));
            if (book.PhotoBack != null)
            {
                book.PhotoBack = GetImageBytes(Convert.ToBase64String(book.PhotoBack));
            }
            if(book.AuthorPhoto != null)
            {
                book.AuthorPhoto = GetImageBytes(Convert.ToBase64String(book.AuthorPhoto));
            }
            return book;
        }


        public Book Save(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book;
        }

        public BookAuthor SaveAuthor(BookAuthor author)
        {
            _context.BookAuthors.Add(author);
            _context.SaveChanges();
            return author;
        }

        public Book Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
            return book;
        }

        public bool Delete(Book book)
        {
            try
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Book> GetBookSortedByGenre(int size, string genre)
        {
            List<Book> booksList = new List<Book>();
            var allBooks = _context.Books.ToArray();
            int counter = 0;
            int allBooksLength = allBooks.Length;
            for(int i = allBooksLength - 1; i >= 0; i--)
            {     
                if (allBooks[i].Genre.Trim(' ') == genre.Trim(' '))
                {
                    if (allBooks[i].PhotoFront != null)
                    {
                        allBooks[i].PhotoFront = GetImageBytes(Convert.ToBase64String(allBooks[i].PhotoFront));
                    }
                    booksList.Add(allBooks[i]);
                    counter++;
                    if(counter == size)
                    {
                        break;
                    }
                }
            }
            return booksList;
        }

        public int GetBookNumberByGenre(string genre)
        {
            var books = _context.Books.ToArray();
            int bookNumSortedByGenre = 0;
            for (int i = 0; i < books.Length; i++)
            {
                if(books[i].Genre.Trim() == genre.Trim())
                {
                    bookNumSortedByGenre++;
                }
            }
            int reducer = bookNumSortedByGenre % 4;
            bookNumSortedByGenre -= reducer;
            return bookNumSortedByGenre;
        }

        public BookDescription SaveBookDescription(BookDescription bookDescription)
        {
            _context.BookDescriptions.Add(bookDescription);
            _context.SaveChanges();
            return bookDescription;
        }

        public List<BookAuthor> GetAllBookAuthors()
        {
            var bookAuthors = _context.BookAuthors.ToList();
            foreach(var author in bookAuthors)
            {
                if(author.Photo != null)
                {
                    author.Photo = GetImageBytes(Convert.ToBase64String(author.Photo));
                }
            }
            return bookAuthors;
        }

        public List<Book> GetAllBooks()
        {
            var books = _context.Books.ToList();
            foreach(var book in books)
            {
                if(book.PhotoFront != null)
                {
                    book.PhotoFront = GetImageBytes(Convert.ToBase64String(book.PhotoFront));
                }
            }
            return books;
        }
        public List<Book> GetBooksSortedByCategory(string category)
        {
            var books = from book in _context.Books.ToList()
                        where book.Category == category
                        select book;
            foreach(var book in books)
            {
                if(book.PhotoFront != null)
                {
                    book.PhotoFront = GetImageBytes(Convert.ToBase64String(book.PhotoFront));
                }
            }
            return books.ToList();
        }
        public List<Book> GetBooksSortedByPrice(int fromPrice, int toPrice, string category)
        {
            if(toPrice == 0)
            {
                if(category == "All")
                {
                    var books = from book in _context.Books.ToList()
                                where book.Price >= fromPrice
                                select book;
                    books = GetBase64FrontImage(books.ToList());
                    return books.ToList();
                }
                else
                {
                    var books = from book in _context.Books.ToList()
                                where book.Price >= fromPrice && book.Category == category
                                select book;
                    books = GetBase64FrontImage(books.ToList());
                    return books.ToList();
                }
            }
            else
            {
                if(category == "All")
                {
                    var books = from book in _context.Books.ToList()
                                where book.Price >= fromPrice && book.Price <= toPrice
                                select book;
                    books = GetBase64FrontImage(books.ToList());
                    return books.ToList();
                }
                else
                {
                    var books = from book in _context.Books.ToList()
                                where book.Price >= fromPrice && book.Price <= toPrice && book.Category == category
                                select book;
                    books = GetBase64FrontImage(books.ToList());
                    return books.ToList();
                }

            }

        }
        public List<Book> GetBooksSortedByAuthor(string[] authors, string category)
        {
            List<Book> bookList = new List<Book>();
            if (category == "All")
            {
                if (authors.Length == 0)
                {
                    return GetBase64FrontImage(_context.Books.ToList());
                }
                for (int i = 0; i < authors.Length; i++)
                {
                    var books = from book in _context.Books.ToList()
                                where book.Author == authors[i]
                                select book;
                    foreach (var book in books)
                    {
                        bookList.Add(book);
                    }
                }
                bookList = GetBase64FrontImage(bookList);
                return bookList;
            }
            else
            {
                if (authors.Length == 0)
                {
                    var books = from book in _context.Books.ToList()
                                where book.Category == category
                                select book;
                    return GetBase64FrontImage(books.ToList());
                }
                for (int i = 0; i < authors.Length; i++)
                {
                    var books = from book in _context.Books.ToList()
                                where book.Author == authors[i] && book.Category == category
                                select book;
                    foreach (var book in books)
                    {
                        bookList.Add(book);
                    }
                }
                bookList = GetBase64FrontImage(bookList);
                return bookList;
            }

        }
        public List<Book> GetBase64FrontImage(List<Book> books)
        {
            foreach (var book in books)
            {
                if (book.PhotoFront != null)
                {
                    book.PhotoFront = GetImageBytes(Convert.ToBase64String(book.PhotoFront));
                }
            }
            return books.ToList();
        }

        public List<string> GetExistingBookAuthors(string category)
        {
            List<string> authorList = new List<string>();
            if(category == "All")
            {
                foreach(var book in _context.Books.ToList())
                {
                    if (!authorList.Contains(book.Author))
                    {
                        authorList.Add(book.Author);
                    }
                }
                return authorList;
            }
            else
            {
                foreach(var book in _context.Books.ToList())
                {
                    if (!authorList.Contains(book.Author))
                    {
                        if(book.Category == category)
                        {
                            authorList.Add(book.Author);
                        }
                    }
                }
                return authorList;
            }
        }

        public List<string> GetExistingBookGenres(string category)
        {
            List<string> genreList = new List<string>();
            if (category == "All")
            {
                foreach (var book in _context.Books.ToList())
                {
                    string genre = book.Genre.Trim(' ');
                    if (!genreList.Contains(genre))
                    {
                        genreList.Add(book.Genre);
                    }
                }
                return genreList;
            }
            else
            {
                foreach (var book in _context.Books.ToList())
                {
                    string genre = book.Genre.Trim(' ');
                    if (!genreList.Contains(genre))
                    {
                        if (book.Category == category)
                        {
                            genreList.Add(book.Genre);
                        }
                    }
                }
                return genreList;
            }
        }
        public List<Book> GetSortedBooks(string category, params string[][] sortingDetails)
        {
            List<Book> bookList = new List<Book>();
            List<Book>[] booksArray = new List<Book>[6];
            int count = 0;
            var allBooks = _context.Books.ToList();

            if(category == "All")
            {
                booksArray[0] = allBooks;

                for (int i = 0; i < sortingDetails.Length; i++)
                {
                    
                    if (sortingDetails[i].Length != 0)
                    {
                        List<Book> list = new List<Book>();
                        for (int j = 0; j < sortingDetails[i].Length; j++)
                        {
                            if(i == 0)
                            {
                                var books = from book in booksArray[count]
                                            where book.Author == sortingDetails[i][j]
                                            select book;
                                foreach(var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                            if(i == 1)
                            {
                                var books = from book in booksArray[count]
                                            where book.SuggestedAge == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                            if(i == 2)
                            {
                                var books = from book in booksArray[count]
                                            where book.PublishedYear == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                            if(i == 3)
                            {
                                var books = from book in booksArray[count]
                                            where book.Cover == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                            if (i == 4)
                            {
                                var books = from book in booksArray[count]
                                            where book.Genre == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                        }
                        count++;
                        booksArray[count] = list;
                    }
                }
                bookList = booksArray[count];
                return GetBase64FrontImage(bookList);
            }
            else
            {
                var booksSortedByCategory = from book in allBooks
                                            where book.Category == category
                                            select book;
                booksArray[0] = booksSortedByCategory.ToList();

                for (int i = 0; i < sortingDetails.Length; i++)
                {
                    if (sortingDetails[i].Length != 0)
                    {
                        List<Book> list = new List<Book>();
                        for (int j = 0; j < sortingDetails[i].Length; j++)
                        {
                            if (i == 0)
                            {
                                var books = from book in booksArray[count]
                                            where book.Author == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }

                            }
                            if (i == 1)
                            {
                                var books = from book in booksArray[count]
                                            where book.SuggestedAge == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                            if (i == 2)
                            {
                                var books = from book in booksArray[count]
                                            where book.PublishedYear == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                            if (i == 3)
                            {
                                var books = from book in booksArray[count]
                                            where book.Cover == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                            if (i == 4)
                            {
                                var books = from book in booksArray[count]
                                            where book.Genre == sortingDetails[i][j]
                                            select book;
                                foreach (var book in books)
                                {
                                    list.Add(book);
                                }
                            }
                        }
                        count++;
                        booksArray[count] = list;
                    }
                }
                bookList = booksArray[count];
                return GetBase64FrontImage(bookList);
            }
        }

        public List<Book> SortByIncreasingPrice(List<Book> books)
        {
            var bookArray = books.ToArray();
            for (int i = 0; i < bookArray.Length; i++)
            {
                int price = bookArray[i].Price;
                int index = 0;
                for (int j = i; j < bookArray.Length; j++)
                {
                    if (bookArray[j].Price < price)
                    {
                        price = bookArray[j].Price;
                        index = j;
                    }
                }
                if (price != bookArray[i].Price)
                {
                    Book temp = bookArray[i];
                    bookArray[i] = bookArray[index];
                    bookArray[index] = temp;
                }
            }
            return bookArray.ToList();
        }
        public List<Book> SortByDecreasingPrice(List<Book> books)
        {
            var bookArray = books.ToArray();
            for (int i = 0; i < bookArray.Length; i++)
            {
                int price = bookArray[i].Price;
                int index = 0;
                for (int j = i; j < bookArray.Length; j++)
                {
                    if (bookArray[j].Price > price)
                    {
                        price = bookArray[j].Price;
                        index = j;
                    }
                }
                if (price != bookArray[i].Price)
                {
                    Book temp = bookArray[i];
                    bookArray[i] = bookArray[index];
                    bookArray[index] = temp;
                }
            }
            return bookArray.ToList();
        }
        public List<Book> SortByIncreasingDiscount(List<Book> books)
        {
            var bookArray = books.ToArray();
            for (int i = 0; i < bookArray.Length; i++)
            {
                int discount = bookArray[i].Discount;
                int index = 0;
                for (int j = i; j < bookArray.Length; j++)
                {
                    if (bookArray[j].Discount < discount)
                    {
                        discount = bookArray[j].Discount;
                        index = j;
                    }
                }
                if (discount != bookArray[i].Discount)
                {
                    Book temp = bookArray[i];
                    bookArray[i] = bookArray[index];
                    bookArray[index] = temp;
                }
            }
            return bookArray.ToList();
        }
        public List<Book> SortByDecreasingDiscount(List<Book> books)
        {
            var bookArray = books.ToArray();
            for (int i = 0; i < bookArray.Length; i++)
            {
                int discount = bookArray[i].Discount;
                int index = 0;
                for (int j = i; j < bookArray.Length; j++)
                {
                    if (bookArray[j].Discount > discount)
                    {
                        discount = bookArray[j].Discount;
                        index = j;
                    }
                }
                if (discount != bookArray[i].Discount)
                {
                    Book temp = bookArray[i];
                    bookArray[i] = bookArray[index];
                    bookArray[index] = temp;
                }
            }
            return bookArray.ToList();
        }
        public decimal[] GetFinalPrice(int[] bookIds, string operation)
        {
            int oldPrice = 0;
            decimal decrement = 0;
            int price = 0;
            for(int i = 0; i < bookIds.Length; i++)
            {
                var book = Find(bookIds[i]);
                if(book.OldPrice == 0) 
                { 
                    oldPrice += book.Price;
                }
                else { 
                    oldPrice += book.OldPrice;
                    decrement += (book.OldPrice - book.Price);
                }
                
                price += book.Price;
            }
            if(operation == "minus")
            {
                var book = Find(bookIds[bookIds.Length - 1]);
                if(book.OldPrice == 0)
                {
                    oldPrice -= book.Price;
                }
                else
                {
                    oldPrice -= book.OldPrice;
                    decrement -= (book.OldPrice - book.Price);
                }
                price -= book.Price;
            }

            decimal percent = (decrement / oldPrice) * 100;
            decimal[] data = { oldPrice, percent, price };
            return data;
        }

        public bool DeleteAuthor(int id)
        {
            try
            {
                var author = _context.BookAuthors.Find(id);
                _context.BookAuthors.Remove(author);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}