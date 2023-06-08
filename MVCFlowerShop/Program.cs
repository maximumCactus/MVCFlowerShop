using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCFlowerShop.Data;
using MVCFlowerShop.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MVCFlowerShopContextConnection") ?? throw new InvalidOperationException("Connection string 'MVCFlowerShopContextConnection' not found.");

builder.Services.AddDbContext<MVCFlowerShopContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MVCFlowerShopUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<MVCFlowerShopContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

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
app.UseAuthentication();;

app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});


app.Run();