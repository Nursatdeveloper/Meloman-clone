using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Services
{
    public interface IService
    {
        bool DownloadExcel(string downloadItem);
    }
}
