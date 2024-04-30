
using BookStore.Data;
using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
=======
using BookStore.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.DataAccess.Repository;
using BookStore.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

>>>>>>> swarnajit

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add services to the container.
builder.Services.AddControllersWithViews();
<<<<<<< HEAD
builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
=======
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRazorPages(); //this is for the Razor pages which we added for Identificaiton       

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe")); //this qill auto set the propery of StripeSettings fatch form the Stripe Secton of the appsettings.json


//if we want define user characterestics we need to add e model which must need to InharitThe IdentityUser ->(BookStore.Modes->ApplicationUser)
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath="/Identity/Account/Login";
    options.LogoutPath="/Identity/Account/Logout";
    options.AccessDeniedPath= "/Identity/Account/AccessDenied";
}); //this are to redirect the user to the correct pages for their invalid access request
//this must be add after the identity
>>>>>>> swarnajit

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDBContext>();

//for unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
