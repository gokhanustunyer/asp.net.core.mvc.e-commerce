using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Category;
using eticaret.data.Abstract.Filter;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Filter.CreateFilter
{
    public class CreateFilterCommandHandler
        : IRequestHandler<CreateFilterCommandRequest, CreateFilterCommandResponse>
    {
        private readonly IFilterBoxRepository _filterBoxRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;

        public CreateFilterCommandHandler(IFilterBoxRepository filterBoxRepository,
                                          ICategoryRepository categoryRepository,
                                          ICategoryService categoryService)
        {
            _filterBoxRepository = filterBoxRepository;
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
        }

        public async Task<CreateFilterCommandResponse> Handle(CreateFilterCommandRequest request, CancellationToken cancellationToken)
        {
            List<et.Category.Category> categories = new();
            List<et.Filter.Filter> filters = new();
            foreach(string categoryId in request.CategoryIds)
            {
                et.Category.Category category = await _categoryRepository.GetByIdAsync(categoryId);
                categories.Add(category);
                List<et.Category.Category> topCategories = await _categoryService.GetTopCategories(category);
                categories.Add(category);
                categories.AddRange(topCategories);
            }
            foreach(string filterName in request.FilterNames)
            {
                filters.Add(new()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FilterTitle = filterName,
                    Id = Guid.NewGuid(),
                });
            }
            await _filterBoxRepository.AddAsync(new()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                FilterBoxTitle = request.FilterBoxTitle,
                Filters = filters,
                Id = Guid.NewGuid(),
                Categories = categories
            });
            await _filterBoxRepository.SaveAsync();
            return new();
        }
    }
}
