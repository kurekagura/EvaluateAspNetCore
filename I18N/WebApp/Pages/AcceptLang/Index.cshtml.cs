using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WebApp.Pages.AcceptLang
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;

        public SelectList SupportedUICultureSelectedItems { get; set; } = default!;
        [Display(Name = "LANGUAGE_SELECTS")]
        public string SelectedUICultureValue { set; get; }

        private readonly List<CultureInfo> _SupportedUICultures;
        private readonly CultureInfo _NeutralCulture = new("en");

        [Display(Name = "DetermineProviderCultureResultの結果")]
        public List<string> DeterminedResult { set; get; } = new();

        public IndexModel(IConfiguration configuration)
        {
            _config = configuration;
            _SupportedUICultures = _config.GetSection("WebApp:supportedUICultures").Get<List<CultureInfo>>()!; //stringをCultureInfoにしてくれる。
            _SupportedUICultures.Add(_NeutralCulture);

            SelectedUICultureValue = _NeutralCulture.Name;
        }

        public async Task OnGetAsync()
        {
            //ctorではRequestやHttpContextはnull。
            //var acceptLanguageHeader = Request.Headers["Accept-Language"];
            var acceptLangProvider = new AcceptLanguageHeaderRequestCultureProvider { MaximumAcceptLanguageHeaderValuesToTry = 10 };
            ProviderCultureResult? cultureResult = await acceptLangProvider.DetermineProviderCultureResult(HttpContext);

            CultureInfo acceptLangPrimary; //Accept-Languageのプライマリ言語コード（必須）
            try
            {
                //acceptLangPrimary = new CultureInfo("fr");
                acceptLangPrimary = new CultureInfo(cultureResult!.UICultures[0].Value!);
            }
            catch (Exception) //CultureNotFoundException
            {
                acceptLangPrimary = _NeutralCulture; //存在しない場合の既定
            }

            //確認用
            if (cultureResult != null)
            {
                DeterminedResult.AddRange(cultureResult.UICultures.Where(r => r.Value != null).Select(r => r.Value!));
            }
            //select要素では.NETのSelectListを利用できる。
            //Accept-Languageのプライマリ言語で表示名をローカライズする。
            SupportedUICultureSelectedItems = new SelectList(
                GetCultureDisplayNameForUserCulture(acceptLangPrimary, _SupportedUICultures), dataValueField: "Item1", dataTextField: "Item2");

            Console.WriteLine("DetermineProviderCultureResultより初期選択しておく言語を引き当てる（未対応の場合はニュートラル言語（en））");

            foreach (var uiCulture in cultureResult!.UICultures)
            {
                if (uiCulture.Value == null)
                    continue;

                if (((List<Tuple<string, string>>)SupportedUICultureSelectedItems.Items).Exists(t => t.Item1 == uiCulture.Value))
                {
                    SelectedUICultureValue = uiCulture.Value;
                    break;
                }
            }
        }

        //ユーザーのブラウザが対応している言語で表示するため
        // 現在のカルチャを一時的に変更し、DisplayNameを取得
        public static List<Tuple<string, string>> GetCultureDisplayNameForUserCulture(CultureInfo userCulture, List<CultureInfo> targetCultures)
        {
            var result = new List<Tuple<string, string>>();
            CultureInfo originalCulture = CultureInfo.CurrentUICulture;
            try
            {
                CultureInfo.CurrentUICulture = userCulture;

                foreach (var targetCulture in targetCultures)
                {
                    result.Add(Tuple.Create(targetCulture.Name, targetCulture.DisplayName));
                }
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture; // 元のカルチャに戻す
            }
            return result;
        }
    }
}
