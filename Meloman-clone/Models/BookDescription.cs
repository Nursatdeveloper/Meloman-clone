using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Models
{
    public class BookDescription
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Citation { get; set; }
    }
}
