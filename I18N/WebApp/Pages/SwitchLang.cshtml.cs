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
                // ���݂̃N�b�L�[���폜���邽�߂ɓ������O�̃N�b�L�[��V�����ݒ肵�A�L���������ߋ��ɐݒ�
                Response.Cookies.Append(
                    "lang",
                    "", // �󕶎���܂��� null �ŐV�����l��ݒ�
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(-1), // �L���������ߋ��̓����ɐݒ�
                    }
                );

            }

            string returnUrl = Request.Headers["Referer"].ToString() ?? "/";
            Response.Redirect(returnUrl);
        }
    }
}
