using eticaret.business.Abstract.Storage.Azure;
using eticaret.data.Abstract.Product;
using eticaret.data.Contexts;
using eticaret.entity.Identity;
using eticaret.entity.Order;
using eticaret.entity.Product;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using et = eticaret.entity.Product;

namespace eticaret.business.Features.Commands.Product.GetComment
{
    public class GetCommentCommandHandler
            : IRequestHandler<GetCommentCommandRequest, GetCommentCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAzureStorage _azureStorage;
        private readonly ETicaretDbContext _eTicaretDbContext;
        private readonly IConfiguration _configuration;

        public GetCommentCommandHandler(IProductRepository productRepository,
                                        UserManager<AppUser> userManager,
                                        ETicaretDbContext eTicaretDbContext,
                                        IAzureStorage azureStorage,
                                        IConfiguration configuration)
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _eTicaretDbContext = eTicaretDbContext;
            _azureStorage = azureStorage;
            _configuration = configuration;
        }

        public async Task<GetCommentCommandResponse> Handle(GetCommentCommandRequest request, CancellationToken cancellationToken)
        {
            if (!IsInPasteOrders(request.UserName, request.ProductId)) { return new(); }
            et.Product product = await _productRepository.Table.Include(p => p.Rates).Include(p => p.Comments).ThenInclude(c => c.User).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.ProductId));
            var user = await _userManager.FindByNameAsync(request.UserName);
            DateTime dateTime = DateTime.Now;
            ProductComment comment = new()
            {
                Comment = request.Comment,
                CreateDate = dateTime,
                UpdateDate = dateTime,
                Id = Guid.NewGuid(),
                Product = product,
                Rate = new() { Id = Guid.NewGuid(), Rate = request.Rate, Product = product, User = user, CreateDate = dateTime, UpdateDate = dateTime },
                User = user,
                isHavePhoto = (request.CommentImages != null) ? true : false,
                Images = new()
            };
            List<(string, string)> productCommentImages = new();
            if (request.CommentImages != null)
            {
                productCommentImages = await _azureStorage.UploadAsync(_configuration["Containers:Azure"], request.CommentImages);
                List<CommentImage> commentImages = new();
                for (var i = 0; i < productCommentImages.Count; i++)
                {
                    comment.Images.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Path = productCommentImages[i].Item2,
                        FileName = productCommentImages[i].Item1,
                        Storage = "Azure",
                    });

                }
            }
            _productRepository.Context.ProductComments.Add(comment);
            await _productRepository.SaveAsync();
            return new();
        }
        private bool IsInPasteOrders(string username, string productId)
        {
            AppUser? user = _eTicaretDbContext.Users
                                              .Include(u => u.Orders)
                                              .ThenInclude(o => o.OrderItem)
                                              .FirstOrDefault(u => u.UserName == username);
            bool isInPasteOrders = false;
            foreach(Order order in user.Orders)
            {
                foreach(OrderItem orderItem in order.OrderItem)
                {
                    if (orderItem.ProductId == productId) { isInPasteOrders = true; }
                }
            }

            return isInPasteOrders;
        }
    }

}
