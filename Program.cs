using Biluthyrning.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace Biluthyrning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<CarRentalContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppContext")));
            builder.Services.AddScoped<IUser, UserRepository>();
            builder.Services.AddScoped<IBooking, BookingRepository>();
            builder.Services.AddScoped<ICar, CarRepository>();
            builder.Services.AddScoped<ICarCategory, CarCategoryRepository>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddHttpClient<MyHttpClient>(client => client.BaseAddress = new Uri("https://localhost:7203/"));

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}