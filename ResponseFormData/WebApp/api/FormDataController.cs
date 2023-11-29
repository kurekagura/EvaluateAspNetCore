using System.Net.Mime;
using WebApp.Mvc;
using WebApp.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.FormData.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormDataController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public FormDataController(IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            //var stream1 = System.IO.File.Open(Path.Combine(_env.WebRootPath, "text.txt"), FileMode.Open, FileAccess.Read, FileShare.Read);
            var stream2 = System.IO.File.Open(Path.Combine(_env.WebRootPath, "tensai_vegetable.png"), FileMode.Open, FileAccess.Read, FileShare.Read);

            //var content = JsonContent.Create()

            //var result = new MyMultipartResult()
            //{
            //    new MultipartHttpContent(){ContentType = MediaTypeNames.Text.Plain,Name = "name1",HttpContent = new StreamContent(stream1)},
            //    new MultipartHttpContent(){ContentType = MediaTypeNames.Text.Plain,Name = "name2",HttpContent = new StreamContent(stream2)}
            //};

            var jsonContent = JsonContent.Create(new { width = 100, height = 200 });
            var result = new MultipartActionResult()
            {
                //new MultipartHttpContent(){ContentType = "image/png", Name = "name1",HttpContent = new StreamContent(stream3)},
                new MultipartHttpContent(){ContentType = "image/png", Name = "name1",HttpContent = new StreamContent(stream2)},
                new MultipartHttpContent(){ContentType = MediaTypeNames.Application.Json, Name = "name2",HttpContent = jsonContent}
            };

            return result;
        }
    }
}
