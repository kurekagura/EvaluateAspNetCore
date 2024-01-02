using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using System.Reflection;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var mvcBuilder = builder.Services.AddRazorPages();
            //builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resx"); //↓何れかでOK
            mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => opts.ResourcesPath = "Resx");
            mvcBuilder.AddDataAnnotationsLocalization(opts =>
            {
                opts.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var asmName = typeof(WebApp.Resx.All).Assembly.GetName();
                    //return factory.Create(typeof(WebApp.Resx.All));
                    return factory.Create(nameof(WebApp.Resx.All), asmName.Name!);
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
            //reqLocOpts.SetDefaultCulture("en");

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