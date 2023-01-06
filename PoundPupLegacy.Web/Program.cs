using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Npgsql;
using PoundPupLegacy.Web.Data;

namespace PoundPupLegacy.Web
{
    public class Program
    {
        const string CONNECTSTRING = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddTransient<NpgsqlConnection>((sp) => new NpgsqlConnection(CONNECTSTRING));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}