using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Data.Entities;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Statistics;
using HouseRentingSystem.Services.Users;

using HouseRentingSystem.Web.Infrastructure;
using HouseRentingSystem.Web.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HouseRentingDbContext>(options =>
    options.UseSqlServer(connectionString));

//https://stackoverflow.com/questions/65007086/services-adddatabasedeveloperpageexceptionfilter-error-code-cs1061
//Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<HouseRentingDbContext>();

builder.Services.AddAutoMapper(typeof(IHouseService).Assembly, typeof(HomeController).Assembly);

builder.Services.AddControllersWithViews(option =>
{
    option.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

builder.Services.AddTransient<IAgentService, AgentService>();
builder.Services.AddTransient<IHouseService, HouseService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.SeedAdmin();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "House Details",
    pattern: "/Houses/Details/{id}/{information}",
    defaults: new { controller = "Houses", action = "Details" });

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();