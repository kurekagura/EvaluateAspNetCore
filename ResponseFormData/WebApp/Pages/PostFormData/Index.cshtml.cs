using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using System.Security.Cryptography;
using WebApp.Mvc;
using WebApp.Net.Http;
using IO = System.IO;

namespace WebApp.Pages.PostFormData
{
    [IgnoreAntiforgeryToken] //ÅyèdóvÅzåüèÿíÜÇ»ÇÃÇ≈ñ≥éã
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

        //Å´NG
        //public async Task<IActionResult> OnPostFormDataAsync(MultipartFormDataContent formDataContent)
        //Å´OK
        public async Task<IActionResult> OnPostFormDataAsync(MyForm fd)
        {
            var outFilePath = Path.Combine(_env.WebRootPath, "out_tensai_vegetable.png");

            using (var input = fd.Key4_FormFile.OpenReadStream())
            using (var gray = UtilSkia.OpenGrayscale(input))
            using (var output = IO.File.OpenWrite(outFilePath))
            {
                await input.FlushAsync();
                await gray.FlushAsync();
                gray.Position = 0;
                await gray.CopyToAsync(output);
                await gray.FlushAsync();
                await output.FlushAsync();
            }

            var stream = System.IO.File.Open(Path.Combine(_env.WebRootPath, "out_tensai_vegetable.png"), FileMode.Open, FileAccess.Read, FileShare.Read);

            var jsonContent = JsonContent.Create(new { width = 100, height = 200 });
            var result = new MultipartActionResult()
            {
                new MultipartHttpContent(){ContentType = fd.Key4_FormFile.ContentType, Name = "name1",HttpContent = new StreamContent(stream)},
                new MultipartHttpContent(){ContentType = MediaTypeNames.Application.Json, Name = "name2",HttpContent = jsonContent}
            };
            return result;
        }
    }

    public class MyForm
    {
        public string Key1_string { get; set; }
        public int Key2_int { get; set; }
        public double Key3_double { get; set; }
        public IFormFile Key4_FormFile { set; get; }
    }
}


