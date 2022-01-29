using Meloman_clone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Services
{
    public interface IPdfService
    {
        byte[] DownloadBookListToPdf(List<Book> books);
    }
}
