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

        [Display(Name = "DetermineProviderCultureResult�̌���")]
        public List<string> DeterminedResult { set; get; } = new();

        public IndexModel(IConfiguration configuration)
        {
            _config = configuration;
            _SupportedUICultures = _config.GetSection("WebApp:supportedUICultures").Get<List<CultureInfo>>()!; //string��CultureInfo�ɂ��Ă����B
            _SupportedUICultures.Add(_NeutralCulture);

            SelectedUICultureValue = _NeutralCulture.Name;
        }

        public async Task OnGetAsync()
        {
            //ctor�ł�Request��HttpContext��null�B
            //var acceptLanguageHeader = Request.Headers["Accept-Language"];
            var acceptLangProvider = new AcceptLanguageHeaderRequestCultureProvider { MaximumAcceptLanguageHeaderValuesToTry = 10 };
            ProviderCultureResult? cultureResult = await acceptLangProvider.DetermineProviderCultureResult(HttpContext);

            CultureInfo acceptLangPrimary; //Accept-Language�̃v���C�}������R�[�h�i�K�{�j
            try
            {
                //acceptLangPrimary = new CultureInfo("fr");
                acceptLangPrimary = new CultureInfo(cultureResult!.UICultures[0].Value!);
            }
            catch (Exception) //CultureNotFoundException
            {
                acceptLangPrimary = _NeutralCulture; //���݂��Ȃ��ꍇ�̊���
            }

            //�m�F�p
            if (cultureResult != null)
            {
                DeterminedResult.AddRange(cultureResult.UICultures.Where(r => r.Value != null).Select(r => r.Value!));
            }
            //select�v�f�ł�.NET��SelectList�𗘗p�ł���B
            //Accept-Language�̃v���C�}������ŕ\���������[�J���C�Y����B
            SupportedUICultureSelectedItems = new SelectList(
                GetCultureDisplayNameForUserCulture(acceptLangPrimary, _SupportedUICultures), dataValueField: "Item1", dataTextField: "Item2");

            Console.WriteLine("DetermineProviderCultureResult��菉���I�����Ă���������������Ă�i���Ή��̏ꍇ�̓j���[�g��������ien�j�j");

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

        //���[�U�[�̃u���E�U���Ή����Ă��錾��ŕ\�����邽��
        // ���݂̃J���`�����ꎞ�I�ɕύX���ADisplayName���擾
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
                CultureInfo.CurrentUICulture = originalCulture; // ���̃J���`���ɖ߂�
            }
            return result;
        }
    }
}
