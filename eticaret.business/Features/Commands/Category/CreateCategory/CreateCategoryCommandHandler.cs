using eticaret.business.Abstract.Storage.Local;
using eticaret.business.Operations;
using eticaret.data.Abstract.Category;
using eticaret.entity.Category;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Category.CreateCategory
{
    public class CreateCategoryCommandHandler
            : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILocalStorage _localStorage;
        private readonly IConfiguration _configuration;
        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository,
                                            ILocalStorage localStorage,
                                            IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _localStorage = localStorage;
            _configuration = configuration;
        }

        public async Task<CreateCategoryCommandResponse> Handle
            (CreateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            // Creating an Category object with request parameters
            et.Category.Category category = new()
            {
                Name = request.Name,
                TopCategoryId = request.TopCategoryId,
                Url = UrlNameOperation.CharacterRegulatory(request.Name,
                (string url) => _categoryRepository.Table.FirstOrDefault(p => p.Url == url) != null),
            };
            // Uploading category image from request to active storage
            var path = await _localStorage.UploadOneAsync(_configuration["Containers:Azure"], request.CategoryImage);
            // Creating a CategoryImage variable for match our category
            CategoryImage image = new()
            {
                Category = new List<et.Category.Category>() { category },
                FileName = path.fileName,
                Path = path.path,
                Storage = _configuration["ActiveStorage"],
            };
            // Matching our category with image
            category.CategoryImage = image;
            // Commit new category to database
            await _categoryRepository.AddAsync(category);
            // We make the change we make permanent
            await _categoryRepository.SaveAsync();
            return new CreateCategoryCommandResponse();
        }
    }
}
