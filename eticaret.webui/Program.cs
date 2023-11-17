using eticaret.business;
using eticaret.data;
using eticaret.data.Concrete;
using eticaret.webui;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddCookieSettings();
builder.Services.AddBusinessServices();
builder.Services.AddDataServices();
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = builder.Configuration["ExternalLogins:Facebook:AppId"];
    options.AppSecret = builder.Configuration["ExternalLogins:Facebook:AppSecret"];
}).AddGoogle(optitons =>
{
    optitons.ClientId = builder.Configuration["ExternalLogins:Google:AppId"];
    optitons.ClientSecret = builder.Configuration["ExternalLogins:Google:AppSecret"];
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



SeedDatabase.Seed();
SeedDatabase.Seed(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "/",
//    defaults: new {controller = "Home", Action = "Index"}
//);

//app.MapControllerRoute(
//    name: "default",
//    pattern: "/{category}",
//    defaults: new { controller = "Home", Action = "Products" }
//);

//app.MapControllerRoute(
//    name: "default",
//    pattern: "/{category}/{url?}",
//    defaults: new { controller = "Home", Action = "ProductDetails" }
//);


//app.MapControllerRoute(
//    name: "sitemap",
//    pattern: "/sitemap.xml",
//    defaults: new { controller = "Home", Action = "SiteMap" }
//);


//app.MapControllerRoute(
//    name: "checkout",
//    pattern: "/user/basket/DeleteFromBasket",
//    defaults: new { controller = "User", Action = "DeleteFromBasket" }
//);


//app.MapControllerRoute(
//    name: "checkout",
//    pattern: "/user/basket/checkout",
//    defaults: new { controller = "User", Action = "CheckOut" }
//);

//app.MapControllerRoute(
//    name: "login",
//    pattern: "/login",
//    defaults: new { controller = "Auth", Action = "Login" }
//);


//app.MapControllerRoute(
//    name: "logout",
//    pattern: "/logout",
//    defaults: new { controller = "Auth", Action = "Logout" }
//);


//app.MapControllerRoute(
//    name: "register",
//    pattern: "/register",
//    defaults: new { controller = "Users", Action = "CreateUser" }
//);

//app.MapControllerRoute(
//    name: "adminProducts",
//    pattern: "/adminproducts",
//    defaults: new { controller = "Admin", Action = "AdminProducts" }
//);

//app.MapControllerRoute(
//    name: "editProduct",
//    pattern: "/editProduct/{id?}",
//    defaults: new { controller = "Admin", Action = "EditProduct" }
//);

//app.MapControllerRoute(
//    name: "editProduct",
//    pattern: "/AddImage/{id?}",
//    defaults: new { controller = "Admin", Action = "AddImage" }
//);

app.MapControllerRoute(
    name: "supportform",
    pattern: "/SupportForm",
    defaults: new { controller = "Home", Action = "SupportForm" }
);

app.MapControllerRoute(
    name: "adminPanel",
    pattern: "/accessdenied",
    defaults: new { controller = "Auth", Action = "AccessDenied" }
);

app.MapControllerRoute(
    name: "notFound",
    pattern: "/NotFounded",
    defaults: new { controller = "Home", Action = "NotFounded" }
);

app.MapControllerRoute(
    name: "login",
    pattern: "/login",
    defaults: new { controller = "Auth", Action = "Login" }
);

app.MapControllerRoute(
    name: "notFound",
    pattern: "/Register",
    defaults: new { controller = "Auth", Action = "Register" }
);


app.MapControllerRoute(
    name: "notFound",
    pattern: "/Logout",
    defaults: new { controller = "Auth", Action = "Logout" }
);

app.MapControllerRoute(
    name: "help",
    pattern: "/yardim",
    defaults: new { controller = "Home", Action = "Help" }
);

app.MapControllerRoute(
    name: "adminPanel",
    pattern: "/Admin/{action}/{id?}",
    defaults: new { controller = "Admin", Action = "Index" }
);

app.MapControllerRoute(
    name: "favorites",
    pattern: "/User/{action}",
    defaults: new { controller = "User", Action = "Index" }
);

app.MapControllerRoute(
    name: "adminPanel",
    pattern: "/auth/{action}",
    defaults: new { controller = "Auth", Action = "Login" }
);


app.MapControllerRoute(
    name: "productdetails",
    pattern: "/{category}/{url}",
    defaults: new { controller = "Home", Action = "ProductDetails" }
);

app.MapControllerRoute(
    name: "getByCategory",
    pattern: "/{category}",
    defaults: new { controller = "Home", Action = "Products" }
);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

//app.MapControllerRoute(
//    name: "productDetails",
//    pattern: "/{url?}",
//    defaults: new { controller = "Home", Action = "ProductDetails" }
//);



app.Run();

