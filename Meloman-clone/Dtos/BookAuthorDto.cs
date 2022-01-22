using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Dtos
{
    public class BookAuthorDto
    {
        public IFormFile file { get; set; }
        public string Author { get; set; }
    }
}
