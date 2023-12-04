using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using System.Security.Cryptography;
using WebApp.Mvc;
using WebApp.Net.Http;

namespace WebApp.Pages.Blob
{
    public class ImageBlobModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;

        public byte[] Binary { set; get; }

        public ImageBlobModel(ILogger<IndexModel> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _env = webHostEnvironment;
        }

        public async Task<IActionResult> OnGet()
        {
            var ms = new MemoryStream();

            using (var stream = System.IO.File.Open(Path.Combine(_env.WebRootPath, "tensai_vegetable.png"), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await stream.CopyToAsync(ms);
                await stream.FlushAsync();
            }
            await ms.FlushAsync();
            ms.Position = 0;
            Binary = ms.ToArray();
            return Page();
        }
    }
}
