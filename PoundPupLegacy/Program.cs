using Microsoft.Extensions.FileProviders;
using Npgsql;
using PoundPupLegacy.Services;
using PoundPupLegacy.Web.Services;

namespace PoundPupLegacy;

public class Program
{
    public static void Main(string[] args)
    {
        const string CONNECTSTRING = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<NpgsqlConnection>((sp) => new NpgsqlConnection(CONNECTSTRING));
        builder.Services.AddTransient<FetchNodeService>();
        builder.Services.AddTransient<FetchBlogService>();
        builder.Services.AddTransient<RazorViewToStringService>();
        builder.Services.AddTransient<StringToDocumentService>();

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
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider("d:\\ppl\\files"),
            RequestPath = "/files"
        });


        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}