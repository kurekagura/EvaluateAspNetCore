using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages.Validate
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        public void OnGet() { }

        [BindProperty]
        [Required(ErrorMessage ="�K�{�ł��B�m�����\�[�X")]
        public string InputText1 { get; set; } = default!;

        [Required(ErrorMessage = "�K�{�ł��B�m�����\�[�X")]
        public string InputText2 { get; set; } = default!;

        public async Task<PageResult> OnPost()
        {
            await Task.Delay(10);
            return Page();
        }

        public async Task<IActionResult> OnPostPageHandlerAsync(
            [Required(ErrorMessage = "�����ɏ�����̂�")]string input)
        {
            await Task.Delay(10);
            return Page();
        }
    }
}
