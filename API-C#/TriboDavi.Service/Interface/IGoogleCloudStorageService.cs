using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace TriboDavi.Service
{
    public interface IGoogleCloudStorageService
    {
        Task<string> UploadFileToGcsAsync(IFormFile file, string objectName);
        Task DeleteFileFromGcsAsync(string objectName);
    }
}
