using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Abstract.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainerName, IFormFileCollection formFileCollection);
        Task<(string fileName, string path)> UploadOneAsync(string pathOrContainerName, IFormFile formFile);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName, string fileName);
    }
}
