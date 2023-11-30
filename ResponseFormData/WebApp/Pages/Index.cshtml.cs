using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using WebApp.Mvc;
using WebApp.Net.Http;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _env = webHostEnvironment;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetFormDataAsync()
        {
            var stream2 = System.IO.File.Open(Path.Combine(_env.WebRootPath, "tensai_vegetable.png"), FileMode.Open, FileAccess.Read, FileShare.Read);

            var jsonContent = JsonContent.Create(new { width = 100, height = 200 });
            var result = new MultipartActionResult()
            {
                new MultipartHttpContent(){ContentType = "image/png", Name = "name1",HttpContent = new StreamContent(stream2)},
                new MultipartHttpContent(){ContentType = MediaTypeNames.Application.Json, Name = "name2",HttpContent = jsonContent}
            };
            return result;
        }
    }
}