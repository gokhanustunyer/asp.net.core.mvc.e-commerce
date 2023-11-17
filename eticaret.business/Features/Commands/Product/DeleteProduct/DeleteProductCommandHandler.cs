using eticaret.business.ViewModels.Notice;
using eticaret.data.Abstract.Logs;
using eticaret.data.Abstract.Product;
using eticaret.entity.Log;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity.Product;

namespace eticaret.business.Features.Commands.Product.DeleteProduct
{
    public class DeleteProductCommandHandler
        : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPageLogRepository _pageLogRepository;
        public DeleteProductCommandHandler(IProductRepository productRepository,
                                           IPageLogRepository pageLogRepository)
        {
            _productRepository = productRepository;
            _pageLogRepository = pageLogRepository;
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            List<PageLog> pageLogs = _pageLogRepository.Table.Where(p => p.Product.Id == Guid.Parse(request.Id)).ToList();
            _pageLogRepository.RemoveRange(pageLogs);
            await _pageLogRepository.SaveAsync();

            et.Product product = await _productRepository.Table.Include(p => p.RelatedProducts)
                                                               .FirstOrDefaultAsync(p => p.Id.ToString() == request.Id);
            var removedRelateds = _productRepository.Context.RelatedProducts.Where(rp => rp.ProductId == product.Id.ToString() || rp.RelatedProductId == product.Id.ToString());
            _productRepository.Context.RelatedProducts.RemoveRange(removedRelateds);
            var result = _productRepository.Remove(product);
            await _productRepository.SaveAsync();

            NoticeViewModel noticeViewModel = null;
            if (result)
            {
                noticeViewModel = new NoticeViewModel()
                {
                    Title = "İşlem Başarılı",
                    Message = "Ürün Silme İşlemi Başarıyla Gerçekleştirildi",
                    MessageType = NoticeTypes.Success
                };
            }
            else
            {
                noticeViewModel = new NoticeViewModel()
                {
                    Title = "Beklenmedik Hata!",
                    Message = "İşlem Sırasında Beklenmedik Bir Sorun Oluştu",
                    MessageType = NoticeTypes.Error
                };
            }
            return new() { Notice = noticeViewModel };
        }
    }
}
