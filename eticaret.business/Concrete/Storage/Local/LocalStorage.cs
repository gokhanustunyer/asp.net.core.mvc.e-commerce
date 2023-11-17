using eticaret.business.Abstract.Storage;
using eticaret.business.Abstract.Storage.Local;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly IHostEnvironment _webHostEnvironment;

        public LocalStorage(IHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
            => File.Delete($"{pathOrContainerName}\\{fileName}");

        public List<string> GetFiles(string pathOrContainerName)
        {
            DirectoryInfo directory = new(pathOrContainerName);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainerName, IFormFileCollection formFileCollection)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", pathOrContainerName);
            if (!Directory.Exists(uploadPath)) 
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in formFileCollection)
            {
                string extension = Path.GetExtension(file.FileName);
                string newFileName = Guid.NewGuid().ToString() + extension;
                await CopyFileAsync($"{uploadPath}\\{newFileName}", file);
                datas.Add((newFileName, $"{pathOrContainerName}\\{newFileName}"));
            }
            return datas;

        }

        public bool HasFile(string path, string fileName)
            => File.Exists($"{path}\\{fileName}");

        
        public async Task<(string fileName, string path)> UploadOneAsync(string pathOrContainerName, IFormFile formFile)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", pathOrContainerName);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string extension = Path.GetExtension(formFile.FileName);
            string newFileName = Guid.NewGuid().ToString() + extension;
            await CopyFileAsync($"{uploadPath}\\{newFileName}", formFile);

            return (newFileName, $"{pathOrContainerName}\\{newFileName}");
        }

        private async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using (FileStream fileStream = new(path, FileMode.Create,
                    FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false))
                {
                    await file.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
