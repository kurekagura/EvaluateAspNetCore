using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class SwitchLangModel : PageModel
    {
        public void OnGet()
        {
            string? culture = Request.Query["culture"];
            if (culture != null)
            {
                Response.Cookies.Append(
                    //CookieRequestCultureProvider.DefaultCookieName,
                    "lang",
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture))
                    );
            }
            else
            {
                // 現在のクッキーを削除するために同じ名前のクッキーを新しく設定し、有効期限を過去に設定
                Response.Cookies.Append(
                    "lang",
                    "", // 空文字列または null で新しい値を設定
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(-1), // 有効期限を過去の日時に設定
                    }
                );

            }

            string returnUrl = Request.Headers["Referer"].ToString() ?? "/";
            Response.Redirect(returnUrl);
        }
    }
}
