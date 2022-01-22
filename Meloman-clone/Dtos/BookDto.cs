using Microsoft.AspNetCore.Http;

namespace Meloman_clone.Dtos
{
    public class BookDto
    {
        public IFormFile FrontPhoto { get; set; }
        public IFormFile BackPhoto { get; set; }
        public string Book { get; set; }
    }
}