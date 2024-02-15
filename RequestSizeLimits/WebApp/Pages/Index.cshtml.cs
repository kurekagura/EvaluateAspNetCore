using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

[IgnoreAntiforgeryToken]
[RequestSizeLimit(LimitSize)]
//[RequestFormLimits(MultipartBodyLengthLimit = LimitSize)]
public class IndexModel : PageModel
{
    private const long LimitSize = 1024L * 1024L * 1024L * 1024L;

    private readonly ILogger<IndexModel> _logger;
    private readonly IHostEnvironment _env;

    public IndexModel(ILogger<IndexModel> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostUploadFileAsync(IFormFile uploadedFile)
    {
        if (uploadedFile == null || uploadedFile.Length == 0)
        {
            return BadRequest("Please select a file to upload.");
        }

        var wwwRootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
        var uploadPath = Path.Combine(wwwRootPath, "uploads");
        Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, uploadedFile.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await uploadedFile.CopyToAsync(stream);
        }

        return StatusCode(StatusCodes.Status200OK);
    }
}