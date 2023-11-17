using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Category;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity.Category;

namespace eticaret.business.Features.Commands.Category.DeleteCategory
{
    public class DeleteCategoryCommandHandler
        : IRequestHandler<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository,
                                            ICategoryService categoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
        }

        public async Task<DeleteCategoryCommandResponse> Handle
            (DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
        {
             List<et.Category> subCategories = _categoryRepository
                .Table.Where(c => c.TopCategoryId == request.Id).ToList();
            for (var i = 0; i < subCategories.Count; i++)
                { subCategories[i].TopCategoryId = null; }
            
            await _categoryRepository.RemoveAsync(request.Id);
            await _categoryRepository.SaveAsync();
            return new();
        }
    }
}
