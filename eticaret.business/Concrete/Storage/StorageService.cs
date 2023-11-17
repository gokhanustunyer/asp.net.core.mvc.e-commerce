using eticaret.business.Abstract.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Concrete.Storage
{
    public class StorageService : IStorageService
    {
        readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
            => await _storage.DeleteAsync(pathOrContainerName, fileName);


        public List<string> GetFiles(string pathOrContainerName)
            => _storage.GetFiles(pathOrContainerName);


        public bool HasFile(string pathOrContainerName, string fileName)
            => _storage.HasFile(pathOrContainerName, fileName);


        public Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
            => _storage.UploadAsync(pathOrContainerName, files);

        public Task<(string fileName, string path)> UploadOneAsync(string pathOrContainerName, IFormFile formFile)
            => _storage.UploadOneAsync(pathOrContainerName, formFile);

    }
}
