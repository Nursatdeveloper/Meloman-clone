using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Models
{
    public class Book
    {
        [Display(Name = "Id")]
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        [Display(Name = "Old price")]
        public int OldPrice { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }

        //Specific characteristics
        public string Genre { get; set; }
        public string Isbn { get; set; }
        public string Publisher { get; set; }
        [Display(Name = "Published year")]
        public string  PublishedYear { get; set; }
        [Display(Name = "Suggested age")]
        public string SuggestedAge { get; set; }
        public string Cover { get; set; }
        public string Weight { get; set; }
        public string Size { get; set; }
        public string Language { get; set; }
        [Display(Name = "Page")]
        public string PageAmount { get; set; }
        public byte[] PhotoFront { get; set; }
        public byte[] PhotoBack { get; set; }
        public byte[] AuthorPhoto { get; set; }
        public decimal Rating { get; set; }
        public int PeopleRated { get; set; }
        public int Amount { get; set; }
    }
}
