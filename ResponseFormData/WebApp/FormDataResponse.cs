using System.Net.Http.Headers;
using WebApp.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Net.Http
{
    /// <summary>
    /// HttpContent(StreamContent,JSonContent(.Create)など)が保持しないName/ContentTypeを保持するためのクラス。
    /// MultipartContent(.Add(HttpContent))の構築ヘルパーとなる。
    /// </summary>
    public class MultipartHttpContent
    {
        public string ContentType { get; set; } = default!;
        public string Name { get; set; } = default!;
        public HttpContent HttpContent { get; set; } = default!;
    }
}

namespace WebApp.Mvc
{
    public class MultipartActionResult : System.Collections.ObjectModel.Collection<MultipartHttpContent>, IActionResult
    {
        //multipart/*仕様
        private readonly MultipartContent _multipartContent;

        public MultipartActionResult(string subType = "form-data", string? boundary = null)
        {
            if (boundary == null)
                _multipartContent = new MultipartContent(subType); //自動生成される
            else
                _multipartContent = new MultipartContent(subType, boundary);
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            foreach (var item in this)
            {
                item.HttpContent.Headers.ContentType = new MediaTypeHeaderValue(item.ContentType);
                //Content-Disposition: disposition-type;の部分
                item.HttpContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                item.HttpContent.Headers.ContentDisposition.Name = item.Name;

                if (item.HttpContent is StreamContent)
                {
                    //response.formData()を成功させるには以下のようにfilenameが必須。
                    //Content-Type: application/octet-stream
                    //Content-Disposition: form-data; name = name1; filename = name1
                    item.HttpContent.Headers.ContentDisposition.FileName = item.Name;
                }

                _multipartContent.Add(item.HttpContent);
            }

            context.HttpContext.Response.ContentLength = _multipartContent.Headers.ContentLength;
            //{multipart/form-data; boundary="x"}がMultipartContentインスタンスで生成・保持されている。
            context.HttpContext.Response.ContentType = _multipartContent.Headers.ContentType!.ToString();

            await _multipartContent.CopyToAsync(context.HttpContext.Response.Body);
        }
    }
}
