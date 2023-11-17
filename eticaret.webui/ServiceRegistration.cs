using eticaret.data.Contexts;
using eticaret.entity.Identity;
using Microsoft.AspNetCore.Identity;

namespace eticaret.webui
{
    public static class ServiceRegistration
    {
        public static void AddCookieSettings(this IServiceCollection services)
        {
            _ = services.AddIdentity<AppUser, AppRole>
                (options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ETicaretDbContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/accessdenied";
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".eticaret.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });
        }
    }
}
