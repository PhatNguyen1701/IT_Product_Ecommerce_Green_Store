using ITProductECommerce.Data;
using ITProductECommerce.Helpers;
using ITProductECommerce.Services.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ITProductCommerceContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("ITProductCommerce"));
});

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/AccessDenied";
});



var app = builder.Build();
//Seeding Admin user for the web when its first run
SeedDatabase();

void SeedDatabase()
{
    using(var scope = app.Services.CreateScope())
    {
        try
        {
            var scopeContext = scope.ServiceProvider.GetRequiredService<ITProductCommerceContext>();
            Seeder.Initialize(scopeContext);
        }
        catch
        {
            throw;
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
