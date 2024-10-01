using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SphereWebsite.Business.Interfaces.S3Interface
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
