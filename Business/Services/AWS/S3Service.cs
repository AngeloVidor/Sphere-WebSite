using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SphereWebsite.Business.Interfaces;
using SphereWebsite.Business.Interfaces.S3Interface;

namespace SphereWebsite.Business.Services.AWS
{
    public class S3Service : IS3Service
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;

        public S3Service(IConfiguration configuration)
        {
            _configuration = configuration;
            _s3Client = new AmazonS3Client(
                _configuration["AWS:AccessKey"],
                _configuration["AWS:SecretKey"],
                Amazon.RegionEndpoint.GetBySystemName(_configuration["AWS:Region"])
            );
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            // Verifique se o arquivo não é nulo
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.");
            }

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = file.OpenReadStream(),
                BucketName = "adotehoje.dev",
                Key = file.FileName,
                ContentType = file.ContentType
            };

            var fileTransferUtility = new TransferUtility(_s3Client);

            await fileTransferUtility.UploadAsync(uploadRequest);

            return $"https://{uploadRequest.BucketName}.s3.amazonaws.com/{uploadRequest.Key}";
        }
    }
}
