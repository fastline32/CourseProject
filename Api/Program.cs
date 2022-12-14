using Api.Extensions;
using Api.Helpers;
using Api.Helpers.BrainTree;
using Core;
using Core.Data.EntryDbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfiles));

// Add services to the container.
builder.Services.AddApplicationServices();

builder.Services.Configure<BrainTreeSettings>(builder.Configuration.GetSection("BrainTree"));
builder.Services.AddSingleton<IBrainTreeGate, BrainTreeGate>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1583062832141502";
    options.AppSecret = "6996ee554c88c3aa6869701af0698eb1";
});
builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));
builder.Services.AddControllersWithViews();



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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoint =>
    {
        endpoint.MapRazorPages();
        endpoint.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    }
);

app.Run();
