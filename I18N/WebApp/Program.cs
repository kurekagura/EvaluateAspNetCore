using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var mvcBuilder = builder.Services.AddRazorPages();
            mvcBuilder.AddDataAnnotationsLocalization(o =>
            {
                o.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(typeof(WebApp.Resx.All));
                };
            });

            var app = builder.Build();

            var supportedCultures = new[]
            {
                new CultureInfo("ja"),
            };

            var reqLocOpts = new RequestLocalizationOptions
            {
                RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() },
                DefaultRequestCulture = new RequestCulture(""),
                //SupportedCultures = supportedCultures,Å@// Formatting numbers, dates, etc.
                SupportedUICultures = supportedCulturesÅ@// UI strings that we have localized.
            };
            app.UseRequestLocalization(reqLocOpts);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}