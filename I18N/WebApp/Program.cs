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
                new CultureInfo("en"),
                new CultureInfo("ja")
            };

            var reqLocOpts = new RequestLocalizationOptions
            {
                RequestCultureProviders = new IRequestCultureProvider[] {
                    //クッキー:.AspNetCore.Culture
                    //フォーマット:c=<カルチャーコード>|uic=<カルチャーコード>
                    new CookieRequestCultureProvider{CookieName = "lang"},
                    new AcceptLanguageHeaderRequestCultureProvider{MaximumAcceptLanguageHeaderValuesToTry = 10 }
                },
                DefaultRequestCulture = new RequestCulture(""),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            var neutral = new CultureInfo("");
            var invariant = CultureInfo.InvariantCulture;

            //var reqLocOpts = new RequestLocalizationOptions();
            //[0]: { Microsoft.AspNetCore.Localization.QueryStringRequestCultureProvider}
            //[1]: { Microsoft.AspNetCore.Localization.CookieRequestCultureProvider}
            //[2]: { Microsoft.AspNetCore.Localization.AcceptLanguageHeaderRequestCultureProvider}
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