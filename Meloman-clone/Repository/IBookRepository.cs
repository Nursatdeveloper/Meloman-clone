using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meloman_clone.Models;

namespace Meloman_clone.Repository
{
    public interface IBookRepository
    {
        Book Save(Book book);
        Book Update(Book book);
        bool Delete(Book book);
        Book Find(int id);
        Book GetBase64Image(Book book);
        byte[] GetImageBytes(string Base64String);
        BookAuthor SaveAuthor(BookAuthor author);
        BookDescription SaveBookDescription(BookDescription bookDescription);
        Book AddAuthorPhoto(Book book);
        List<Book> GetBookSortedByGenre(int size, string genre);
        int GetBookNumberByGenre(string genre);
        List<BookAuthor> GetAllBookAuthors();
        List<string> GetExistingBookAuthors(string category);
        List<string> GetExistingBookGenres(string category);
        List<Book> GetAllBooks();
        List<Book> GetBooksSortedByCategory(string category);
        List<Book> GetBooksSortedByPrice(int fromPrice, int toPrice, string category);
        List<Book> GetBooksSortedByAuthor(string[] authors, string category);
        List<Book> GetSortedBooks(string category, params string[][] sortingDetails);
        List<Book> SortByIncreasingPrice(List<Book> books);
        List<Book> SortByDecreasingPrice(List<Book> books);
        List<Book> SortByIncreasingDiscount(List<Book> books);
        List<Book> SortByDecreasingDiscount(List<Book> books);
        List<Book> GetBase64FrontImage(List<Book> books);
        decimal[] GetFinalPrice(int[] bookIds, string operation);
        bool DeleteAuthor(int id);
    }
}
