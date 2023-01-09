using Microsoft.Extensions.FileProviders;
using Npgsql;
using PoundPupLegacy.Web.Services;

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
            builder.Services.AddTransient<NpgsqlConnection>((sp) => new NpgsqlConnection(CONNECTSTRING));
            builder.Services.AddTransient<FetchNodeService>();
            builder.Services.AddTransient<FetchBlogService>();
            builder.Services.AddTransient<RazorViewToStringService>();
            builder.Services.AddTransient<StringToDocumentService>();

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
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider("d:\\ppl\\files"),
                RequestPath = "/files"
            }); 

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}