using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.Layout.Properties;
using Meloman_clone.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Services
{
    public class PdfService : IPdfService
    {
        private readonly IWebHostEnvironment _env;
        public PdfService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public byte[] DownloadBookListToPdf(List<Book> books)
        {
            
            byte[] pdfFileBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            
            using (var doc = new Document(pdf))
            {
                
                Paragraph newline = new Paragraph(" ");

                string FONT_FILENAME = Path.Combine(_env.ContentRootPath, "assets/fonts/arial.ttf");

                PdfFont font = PdfFontFactory.CreateFont(FONT_FILENAME, PdfEncodings.IDENTITY_H);
                doc.SetFont(font);

                var title = new Paragraph("Список книг в базе данных Meloman-clone" )
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(15);
                doc.Add(title);
                doc.Add(newline);

                var date = new Paragraph($"Дата: {DateTime.Now.ToLongDateString()}")
                    .SetFontSize(10);
                doc.Add(date);
                doc.Add(newline);

                var generalInformationTableHeader = new Table(1, true);
                Cell headerCell = new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Общая информация по книгам"));
                generalInformationTableHeader.AddCell(headerCell);
                doc.Add(generalInformationTableHeader);

                var generalInformationTable = new Table(2, true);
                Cell cell11 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Количество книг"));
                Cell cell12 = new Cell(1, 2)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph($"{books.Count}"));

                generalInformationTable.AddCell(cell11);
                generalInformationTable.AddCell(cell12);

                //Adding rows with book categories and its numbers
                var bookCategoriesList = GetBookCategories(books);
                var bookCategoriesNumberList = GetBookCategoriesNumber(books, bookCategoriesList);
                for(int i = 2; i <= bookCategoriesList.Count+1; i++)
                {               
                    for (int j = 1; j < 3; j++)
                    {
                        if(j == 1)
                        {
                            Cell cell1 = new Cell(i, j)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .Add(new Paragraph(bookCategoriesList[i - 2]));
                            generalInformationTable.AddCell(cell1);
                        }
                        else
                        {
                            Cell cell2 = new Cell(i, j)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .Add(new Paragraph(bookCategoriesNumberList[i - 2]));
                            generalInformationTable.AddCell(cell2);
                        }
                    }
                }

                // Adding price row to the table about book's general information
                string overallPrice = GetOverallBookPrice(books);
                int pricePositionAtTable = bookCategoriesList.Count+2;
                Cell priceCell1 = new Cell(pricePositionAtTable, 1)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph("Суммарная цена всех книг (тенге)"));
                Cell priceCell2 = new Cell(pricePositionAtTable, 2)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph(overallPrice));
                generalInformationTable.AddCell(priceCell1);
                generalInformationTable.AddCell(priceCell2);


                doc.Add(generalInformationTable);
                generalInformationTable.Complete();
                doc.Add(newline);
                doc.Add(newline);

                var mainTable = new Table( new float[] { 20, 100, 80, 95, 100, 30, 50, 50});
                mainTable.SetWidth(525);
                mainTable.SetFontSize(8);
                Cell bookNumberCell = new Cell()
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("№"));
                Cell bookNameCell = new Cell()
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Название"));
                Cell bookAuthorCell = new Cell()
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Автор"));
                Cell bookCategoryCell = new Cell()
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Категория"));
                Cell bookGenreCell = new Cell()
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Жанр"));
                Cell bookYearCell = new Cell()
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Год"));
                Cell bookPriceCell = new Cell()
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Цена"));
                Cell bookLanguageCell = new Cell()
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("Язык"));
                mainTable.AddCell(bookNumberCell);
                mainTable.AddCell(bookNameCell);
                mainTable.AddCell(bookAuthorCell);
                mainTable.AddCell(bookCategoryCell);
                mainTable.AddCell(bookGenreCell);
                mainTable.AddCell(bookYearCell);
                mainTable.AddCell(bookPriceCell);
                mainTable.AddCell(bookLanguageCell);

                int bookNumberCounter = 1;
                foreach(var book in books)
                {
                    Cell bookNumber = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph($"{bookNumberCounter}"));
                    mainTable.AddCell(bookNumber);
                    Cell bookName = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph(book.Name));
                    mainTable.AddCell(bookName);
                    Cell bookAuthor = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph(book.Author));
                    mainTable.AddCell(bookAuthor);
                    Cell bookCategory = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph(book.Category));
                    mainTable.AddCell(bookCategory);
                    Cell bookGenre = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph(book.Genre));
                    mainTable.AddCell(bookGenre);
                    Cell bookYear = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph(book.PublishedYear));
                    mainTable.AddCell(bookYear);
                    Cell bookPrice = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph($"{book.Price}"));
                    mainTable.AddCell(bookPrice);
                    Cell bookLanguage = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new Paragraph(book.Language));
                    mainTable.AddCell(bookLanguage);
                    bookNumberCounter++;
                }
                doc.Add(mainTable);

                doc.Close();
                pdfFileBytes = stream.ToArray();
            }
            return pdfFileBytes;
        }
        private List<string> GetBookCategories(List<Book> books)
        {
            var categoryList = new List<string>();
            foreach(var book in books)
            {
                if(!categoryList.Contains(book.Category))
                {
                    categoryList.Add(book.Category);
                }
            }
            return categoryList;
        }
        private List<string> GetBookCategoriesNumber(List<Book> books, List<string> categories)
        {
            var numbers = new List<string>();
            foreach(var category in categories)
            {
                var list = from book in books
                           where book.Category == category
                           select book;
                string number = $"{list.ToList().Count}";
                numbers.Add(number);
            }
            return numbers;
        }
        private string GetOverallBookPrice(List<Book> books)
        {
            int overallPrice = 0;
            foreach(var book in books)
            {
                overallPrice += book.Price;
            }
            return overallPrice.ToString();
        }

        public byte[] DownloadOrderDetailsToPdf(Order order, string name)
        {
            byte[] pdfFileBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))

            using (var doc = new Document(pdf))
            {
                Paragraph newline = new Paragraph(" ");

                string FONT_FILENAME = Path.Combine(_env.ContentRootPath, "assets/fonts/arial.ttf");

                PdfFont font = PdfFontFactory.CreateFont(FONT_FILENAME, PdfEncodings.IDENTITY_H);
                doc.SetFont(font);

                var title = new Paragraph($"Чек для заказа №{order.OrderId}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(15);
                doc.Add(title);
                doc.Add(newline);

                var date = new Paragraph($"Дата: {DateTime.Now.ToLongDateString()}")
                    .SetPaddingBottom(5)
                    .SetFontSize(12);
                doc.Add(date);

                var customer = new Paragraph($"Заказчик: {name}")
                    .SetPaddingBottom(5)
                    .SetFontSize(12);
                doc.Add(customer);

                var telephone = new Paragraph($"Телефон: {order.Telephone}")
                    .SetPaddingBottom(5)
                    .SetFontSize(12);
                doc.Add(telephone);

                var email = new Paragraph($"Email: {order.Email}")
                    .SetPaddingBottom(5)
                    .SetFontSize(12);
                doc.Add(email);

                var city = new Paragraph($"Город: {order.City}")
                    .SetPaddingBottom(5)
                    .SetFontSize(12);
                doc.Add(city);

                if(order.IsExpressDelivery == "да")
                {
                    var delivery = new Paragraph($"Вид доставки: {order.DeliveryType} (express)")
                        .SetPaddingBottom(5)
                        .SetFontSize(12);
                    doc.Add(delivery);
                }
                else
                {
                    var delivery = new Paragraph($"Вид доставки: {order.DeliveryType} (не express)")
                        .SetPaddingBottom(5)
                        .SetFontSize(12);
                    doc.Add(delivery);
                }

                var address = new Paragraph($"Адрес: {order.Address}")
                    .SetPaddingBottom(5)
                    .SetFontSize(12);
                doc.Add(address);

                var payment = new Paragraph($"Оплата: {order.PaymentType}")
                    .SetFontSize(12)
                    .SetPaddingBottom(5);
                
                doc.Add(payment);

                var comments = new Paragraph($"Комментарий к заказу: {order.Comments}")
                    .SetFontSize(12)
                    .SetPaddingBottom(5);
                doc.Add(comments);

                var orderTime = new Paragraph($"Время оформление заказа: {order.Date}")
                    .SetFontSize(12)
                    .SetPaddingBottom(5);
                doc.Add(orderTime);

                var deliveryStatus = new Paragraph($"Статус доставки: {order.DeliveryStatus}")
                    .SetFontSize(12)
                    .SetPaddingBottom(5);
                doc.Add(deliveryStatus);

                var productsTableHeader = new Table(1, true)
                    .AddCell(new Cell().Add(new Paragraph("Товары")).SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                doc.Add(productsTableHeader);
                var productsTableBody = new Table(2, true);
                Cell productTypeCell = new Cell()
                    .Add(new Paragraph("Категория товара"))
                    .SetTextAlignment(TextAlignment.CENTER);
                Cell productNameCell = new Cell()
                    .Add(new Paragraph("Название"))
                    .SetTextAlignment(TextAlignment.CENTER);
                productsTableBody.AddCell(productTypeCell);
                productsTableBody.AddCell(productNameCell);
                var orderProducts = order.Products;
                foreach(var orderProduct in orderProducts)
                {
                    Cell productType = new Cell()
                        .Add(new Paragraph(orderProduct.ProductType))
                        .SetTextAlignment(TextAlignment.CENTER);
                    Cell productName = new Cell()
                        .Add(new Paragraph(orderProduct.ProductName))
                        .SetTextAlignment(TextAlignment.CENTER);
                    productsTableBody.AddCell(productType);
                    productsTableBody.AddCell(productName);
                }
                doc.Add(productsTableBody);
                productsTableBody.Complete();
                doc.Add(newline);
                    

                Table table = new Table(2, true);
                table.SetBackgroundColor(ColorConstants.GRAY);
                Cell initialPrice = new Cell()
                    .Add(new Paragraph("Цена без учета скидки:"))
                    .SetPadding(5)
                    .SetPaddingLeft(10)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(Border.NO_BORDER);
                table.AddCell(initialPrice);

                Cell initialPriceValue = new Cell()
                    .SetPadding(5)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER);
                if(order.InitialPrice == 0)
                {
                    initialPriceValue.Add(new Paragraph($"{order.FinalPrice} тг"));
                }
                else
                {
                    initialPriceValue.Add(new Paragraph($"{order.InitialPrice} тг"));
                }
                table.AddCell(initialPriceValue);

                Cell discount = new Cell()
                    .Add(new Paragraph("Скидка:"))
                    .SetPadding(5)
                    .SetPaddingLeft(10)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(Border.NO_BORDER);
                table.AddCell(discount);

                Cell discountValue = new Cell()
                    .Add(new Paragraph($"{order.Discount}%"))
                    .SetPadding(5)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER);
                table.AddCell(discountValue);

                Cell finalPrice = new Cell()
                    .Add(new Paragraph("Итоговая цена:"))
                    .SetPadding(5)
                    .SetPaddingLeft(10)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(Border.NO_BORDER);
                table.AddCell(finalPrice);

                Cell finalPriceValue = new Cell()
                    .Add(new Paragraph($"{order.FinalPrice} тг"))
                    .SetPadding(5)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER);
                table.AddCell(finalPriceValue);

                doc.Add(table);


                doc.Close();
                pdfFileBytes = stream.ToArray();
            }
            return pdfFileBytes;
        }
    }
}
