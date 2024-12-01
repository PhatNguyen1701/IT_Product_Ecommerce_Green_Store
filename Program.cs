using ITProductECommerce.CustomTokenProviders;
using ITProductECommerce.Data;
using ITProductECommerce.Helpers;
using ITProductECommerce.Hubs;
using ITProductECommerce.Services;
using ITProductECommerce.Services.EmailServices;
using ITProductECommerce.Services.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ITProductCommerceContext>(option =>
{
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

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
})
    .AddEntityFrameworkStores<ITProductCommerceContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(2);
});

builder.Services.Configure<EmailConfirmationTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromDays(3);
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    //Configure some options for remember me function
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Auth/Login";
//});

//Add SignalR for chat
builder.Services.AddSignalR();

//Add email confi for sending mail
var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailSender, EmailSender>();

//Add Singleton for Paypal payment function
builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["PaypalOptions:AppId"],
    builder.Configuration["PaypalOptions:AppSerect"],
    builder.Configuration["PaypalOptions:Mode"]
    ));

var app = builder.Build();

//Seeding data
try
{
    var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ITProductCommerceContext>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    context.Database.EnsureCreated();

    if (!context.Webs.Any())
    {
        var adminPage = new Web
        {
            WebName = "Admin Panel",
            URL = "N/A",
        };

        var homePage = new Web
        {
            WebName = "Home Panel",
            URL = "N/A",
        };
        context.Webs.AddRange(adminPage, homePage);
    }

    if (!context.Departments.Any())
    {
        var hodDE = new Department
        {
            DepartmentId = "HOD",
            DepartmentName = "Head of department",
            Description = "Has ultimate authority and manages all other departments and employees"
        };

        var customerDE = new Department
        {
            DepartmentId = "CUS",
            DepartmentName = "Customer management",
            Description = "Customer of the store, has many functions to comment, rate and order"
        };
        context.Departments.AddRange(hodDE, customerDE);
    }

    var adminRole = new Role
    {
        Name = "admin",
        DepartmentId = "HOD",
        WebId = 1,
        IsAdd = false,
        IsEdit = false,
        IsDelete = false,
        IsViewed = false
    };

    var staffRole = new Role
    {
        Name = "staff",
        DepartmentId = "HOD",
        WebId = 1,
        IsAdd = false,
        IsEdit = false,
        IsDelete = false,
        IsViewed = false
    };

    var customerRole = new Role
    {
        Name = "customer",
        DepartmentId = "CUS",
        WebId = 2,
        IsAdd = false,
        IsEdit = false,
        IsDelete = false,
        IsViewed = false
    };


    if (!context.Roles.Any())
    {
        roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
        roleMgr.CreateAsync(staffRole).GetAwaiter().GetResult();
        roleMgr.CreateAsync(customerRole).GetAwaiter().GetResult();
    }

    if (!context.Users.Any(u => u.UserName == "sysadmin"))
    {
        var adminUser = new User
        {
            UserName = "sysadmin",
            FirstName = "System",
            LastName = "Administrator",
            Gender = false,
            DoB = DateTime.Now,
            Address = "N/A",
            AvatarURL = "admin-avatar-img.jpg",
            Email = "gssysadmin@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "0000 xxx y01",
            IsAdmin = true,
            IsStaff = true,
        };
        var result = userMgr.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
        userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
    }

    context.SaveChanges();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
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

app.MapHub<ChatHub>("/chatHub");

app.Run();
