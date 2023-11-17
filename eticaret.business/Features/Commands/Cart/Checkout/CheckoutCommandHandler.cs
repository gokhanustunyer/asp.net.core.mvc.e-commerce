using eticaret.business.Abstract.Service;
using eticaret.data.Abstract.Cart;
using eticaret.data.Abstract.Order;
using eticaret.data.Abstract.Product;
using eticaret.data.Abstract.Shipping;
using eticaret.data.Contexts;
using eticaret.entity.Cargo;
using eticaret.entity.Cart;
using eticaret.entity.Offer;
using eticaret.entity.Order;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using et = eticaret.entity;

namespace eticaret.business.Features.Commands.Cart.Checkout
{
    public class CheckoutCommandHandler
        : IRequestHandler<CheckoutCommandRequest, CheckoutCommandResponse>
    {
        private readonly ICartService _cartService;
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly ETicaretDbContext _eTicaretDbContext;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<et.Identity.AppUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IShippingRepository _shippingRepository;

        public CheckoutCommandHandler(ICartService cartService,
                                      IConfiguration configuration,
                                      IProductService productService,
                                      IProductRepository productRepository,
                                      ETicaretDbContext eTicaretDbContext,
                                      IOrderRepository orderRepository,
                                      UserManager<et.Identity.AppUser> userManager,
                                      ICartRepository cartRepository,
                                      IOrderStatusRepository orderStatusRepository,
                                      IShippingRepository shippingRepository)
        {
            _cartService = cartService;
            _configuration = configuration;
            _productService = productService;
            _productRepository = productRepository;
            _eTicaretDbContext = eTicaretDbContext;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _cartRepository = cartRepository;
            _orderStatusRepository = orderStatusRepository;
            _shippingRepository = shippingRepository;
        }

        public async Task<CheckoutCommandResponse> Handle
            (CheckoutCommandRequest checkoutRequest, CancellationToken cancellationToken)
        {
            et.Cart.Cart cart = _cartService.GetByUserName(checkoutRequest.UserName);
            et.Identity.AppUser user = await _eTicaretDbContext.Users
                                          .Include(u => u.Addresses)
                                          .FirstOrDefaultAsync
                                          (u => u.UserName == checkoutRequest.UserName);
            et.Identity.Address selectedAddress = user.Addresses.FirstOrDefault
                                    (a => a.Id.ToString() == checkoutRequest.AddressId);

            Options options = new Options();
            options.ApiKey = "sandbox-LCZcq0h0PAicmjsBbWU90Ux8n6BTAoBD";
            options.SecretKey = "sandbox-0GOSc1r3mUDivvsyowzE5nl6SCgGPQE3";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            double totalPrice = 0;
            List<BasketItem> basketItems = new List<BasketItem>();
            List<et.Order.OrderItem> orderItems = new();
            for (var i = 0; i < cart.CartItems.Count; i++)
            {
                et.Product.Product? product = await _productRepository.Table
                                             .Include(p => p.ProductSizes)
                                             .Include(p => p.Campaigns)
                                             .FirstOrDefaultAsync(p=> p.Id.ToString() == cart.CartItems[i].ProductId);
                double price = Math.Round(product.ProductSizes
                                                 .FirstOrDefault(ps =>
                                                 ps.SizeId.ToString() ==
                                                 cart.CartItems[i].SizeId).Price,2) * cart.CartItems[i].Quantity;
                if (product.Campaigns != null)
                {
                    Campaign maxOffer = product.Campaigns[0];
                    foreach(Campaign campaign in product.Campaigns)
                    {
                        if (campaign.DiscountRate > maxOffer.DiscountRate)
                        {
                            maxOffer = campaign;
                        }
                    }
                    price *= (100 - (double)maxOffer.DiscountRate) / 100;
                }

                int stock = product.ProductSizes.FirstOrDefault(p => p.SizeId.ToString() == cart.CartItems[i].SizeId).Stock;
                if (stock - cart.CartItems[i].Quantity < 0)
                {
                    return new();
                }
                product.ProductSizes.FirstOrDefault(p => p.SizeId.ToString() == cart.CartItems[i].SizeId).Stock -= cart.CartItems[i].Quantity;
                _productRepository.Update(product);
                totalPrice += price;
                orderItems.Add(new()
                {
                    ProductId = cart.CartItems[i].ProductId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Quantity = cart.CartItems[i].Quantity,
                    SizeId = cart.CartItems[i].SizeId,
                    TotalPrice = price,
                });

                basketItems.Add(new BasketItem()
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Category1 = "Ust Kategori",
                    Category2 = "Alt Kategori",
                    Price = price.ToString(),
                });
            }

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = totalPrice.ToString();
            request.PaidPrice = totalPrice.ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = checkoutRequest.NameOnCart;
            paymentCard.CardNumber = checkoutRequest.CartNumber;
            paymentCard.ExpireMonth = checkoutRequest.Month;
            paymentCard.ExpireYear = checkoutRequest.Year;
            paymentCard.Cvc = checkoutRequest.CVCCode;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = user.Id;
            buyer.Name = user.FirstName;
            buyer.Surname = user.LastName;
            buyer.GsmNumber = user.PhoneNumber;
            buyer.Email = user.Email;
            buyer.IdentityNumber = user.Id;
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = selectedAddress.DetailedAddress;
            buyer.Ip = "85.34.78.112";
            buyer.City = selectedAddress.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = $"{user.FirstName} {user.LastName}";
            shippingAddress.City = selectedAddress.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = selectedAddress.DetailedAddress;
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = $"{user.FirstName} {user.LastName}";
            billingAddress.City = selectedAddress.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = selectedAddress.DetailedAddress;
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;
            request.BasketItems = basketItems;
            Payment payment = Payment.Create(request, options);

            OrderStatus orderStatus = _orderStatusRepository.Table.FirstOrDefault(os => os.Title == "Onay Bekliyor");
            Shipping shippingCompany = _shippingRepository.Table.FirstOrDefault(s => s.IsDefault);
            totalPrice = (cart.DiscountCode != null) ? (cart.DiscountCode.DiscountRate == 0) ? totalPrice - cart.DiscountCode.DiscountNumber : totalPrice * (1 - (cart.DiscountCode.DiscountRate / 100)) : totalPrice;
            totalPrice += shippingCompany.Price;
            totalPrice = Math.Round(totalPrice, 2);
            et.Order.Order order = new()
            {
                City = selectedAddress.City,
                District = selectedAddress.District,
                Neighborhood = selectedAddress.Neighborhood,
                DetailedAddress = selectedAddress.DetailedAddress,
                PostCode = selectedAddress.PostCode,
                AddressTitle = selectedAddress.Title,
                IsConfirmed = false,
                DeliveryStatus = false,
                Price = totalPrice,
                User = user,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                OrderItem = orderItems,
                OrderStatus = orderStatus,
                ShippingCompany = shippingCompany
            };

            await _orderRepository.AddAsync(order);
            user.Orders.Add(order);
            _cartRepository.Remove(cart);
            await _orderRepository.SaveAsync();

            return new();
        }
    }
}
