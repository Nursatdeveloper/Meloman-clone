using Meloman_clone.Data;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Services
{
    public class Service : IService
    {
        private readonly ApplicationContext _appContext;
        private readonly BookContext _bookContext;
        public Service(ApplicationContext appContext, BookContext bookContext)
        {
            _appContext = appContext;
            _bookContext = bookContext;
        }
        public bool DownloadExcel(string downloadItem)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if(downloadItem == "Review")
            {
                try
                {
                    var file = new FileInfo($"{path}\\Отзывы.xlsx");
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    using var package = new ExcelPackage(file);
                    var worksheet = package.Workbook.Worksheets.Add("Отзывы");
                    var reviews = _appContext.Reviews.ToList();
                    var range = worksheet.Cells["A1"].LoadFromCollection(reviews, true);
                    range.AutoFitColumns();
                    package.Save();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else if (downloadItem == "Book")
            {
                try
                {
                    var file = new FileInfo($"{path}\\Книги.xlsx");
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    using var package = new ExcelPackage(file);
                    var worksheet = package.Workbook.Worksheets.Add("Книги");
                    var books = _bookContext.Books.ToList();
                    var range = worksheet.Cells["A1"].LoadFromCollection(books, true);
                    range.AutoFitColumns();
                    package.Save();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else if (downloadItem == "Order")
            {
                try
                {
                    var file = new FileInfo($"{path}\\Заказы.xlsx");
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    using var package = new ExcelPackage(file);
                    var worksheet = package.Workbook.Worksheets.Add("Заказы");
                    var orders = _appContext.Orders.Include(e => e.Products).ToList();
                    var range = worksheet.Cells["A1"].LoadFromCollection(orders, true);
                    range.AutoFitColumns();
                    int i = 2;
                    foreach (var order in orders)
                    {
                        var products = order.Products.ToList();
                        string productsString = "";
                        foreach (var product in products)
                        {
                            productsString += $"Type: {product.ProductType}, Name: {product.ProductName}\n";
                        }
                        worksheet.Cells[i, 16].Value = productsString;
                        i++;
                    }
                    worksheet.Columns[16].Width = 100;
                    package.Save();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
