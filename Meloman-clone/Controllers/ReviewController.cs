using Meloman_clone.Dtos;
using Meloman_clone.Models;
using Meloman_clone.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}
