using eticaret.business.Abstract.Storage.Local;
using eticaret.business.Operations;
using eticaret.data.Abstract.Category;
using eticaret.entity.Category;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Category.EditCategory
{
    public class EditCategoryCommandHandler
        : IRequestHandler<EditCategoryCommandRequest, EditCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILocalStorage _localStorage;
        private readonly IConfiguration _configuration;
        public EditCategoryCommandHandler(ICategoryRepository categoryRepository,
                                          ILocalStorage localStorage, 
                                          IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _localStorage = localStorage;
            _configuration = configuration;
        }

        public async Task<EditCategoryCommandResponse> Handle
            (EditCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            et.Category.Category category = await _categoryRepository.Table.Include(c => c.CategoryImage).FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.Id));
            if (category == null)
            {
                throw new Exception("Category not founded");
            }
            List<et.Category.Category> allCategories = _categoryRepository.Table
                                .Where(c => c.TopCategoryId == request.Id).ToList();

            for (var i=0;i < allCategories.Count; i++)
                { allCategories[i].TopCategoryId = null; }

            if (request.SubCategoryIds != null)
            {
                for (var i = 0; i < request.SubCategoryIds.Count(); i++) 
                {
                    et.Category.Category subCategory = await _categoryRepository.GetByIdAsync(request.SubCategoryIds[i].ToString());
                    subCategory.TopCategoryId = request.Id;
                    _categoryRepository.Update(subCategory);
                }
            }
            if (request.TopCategoryIds != null)
            {
                category.TopCategoryId = request.TopCategoryIds.ToString();
            }


            if (request.CategoryImage != null)
            {
                var path = await _localStorage.UploadOneAsync(_configuration["Containers:Azure"], request.CategoryImage);
                CategoryImage newImage = new()
                {
                    Category = new List<et.Category.Category>() { category },
                    FileName = path.fileName,
                    Path = path.path,
                    Storage = _configuration["ActiveStorage"],
                };
                _categoryRepository.Context.CategoryImages.Add(newImage);
                category.CategoryImage = newImage;
            }

            category.UpdateDate = DateTime.Now;
            category.Name = request.Name;
            if (request.Url != category.Url)
            {
                category.Url = UrlNameOperation.CharacterRegulatory(request.Url,
                    (string url) => _categoryRepository.Table.FirstOrDefault(p => p.Url == url) != null);
            }

            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();
            return new();
        }
    }
}
