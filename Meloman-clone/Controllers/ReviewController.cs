using Meloman_clone.Dtos;
using Meloman_clone.Models;
using Meloman_clone.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Meloman_clone.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        [HttpPost]
        public JsonResult ReviewList(int productId, string productType)
        {
            var reviews = _reviewRepository.GetReviews(productId, productType);
            if(reviews.Count == 0)
            {
                return new JsonResult("Empty");
            }
            return new JsonResult(reviews);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "MelomanAuthCookie", Policy = "OnlyUsers")]
        public JsonResult AddReview(int productId, string productType, string adv, string disadv, string generalReview, decimal rating)
        {
            if(User.Identity.IsAuthenticated)
            {
                //ReviewDto reviewDto = JsonConvert.DeserializeObject<ReviewDto>(reviewDTO.Review);
                var review = new Review();
                review.Advantages = adv;
                review.Disadvantages = disadv;
                review.ProductType = productType;
                review.GeneralReview = generalReview;
                review.Rating = rating;
                review.Time = DateTime.Now;
                review.ProductId = productId;
                review.Username = User.Identity.Name;
                if (_reviewRepository.SaveReview(review))
                {
                    return new JsonResult(review);
                }
                else
                {
                    return new JsonResult("fail");
                }
            }
            else
            {
                return new JsonResult("IsNotAuthenticated");
            }
            

        }
        public IActionResult DownloadToExcel()
        {
            var list = _reviewRepository.GetReviews(0, "All");
            var stream = new MemoryStream();
            //required using OfficeOpenXml;
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Отзывы");
                workSheet.Cells.LoadFromCollection(list, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Отзывы-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/octet-stream", excelName);
        }
    }
}
