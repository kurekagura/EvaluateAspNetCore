using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApp.Mvc;
using WebApp.Net.Http;
using IO = System.IO;

namespace WebApp.Pages.AspForm;

[IgnoreAntiforgeryToken] //【重要】検証中なので無視
public class IndexModel : PageModel
{
    [Display(Name = "選択肢")]
    public SelectList FigureItems { get; set; }

    //public APluginForm APlugin = new();
    public BPluginForm BPlugin = new();

    private readonly IWebHostEnvironment _env;

    public IndexModel(IWebHostEnvironment webHostEnvironment)
    {
        _env = webHostEnvironment;

        //第二引数以降の指定に注目
        FigureItems = new SelectList(new List<MyFigure>()
        {
            new MyFigure { Value = "circle", Text = "まる" },
            new MyFigure { Value = "rectangle", Text = "四角" }
        }, dataValueField: nameof(MyFigure.Value), dataTextField: nameof(MyFigure.Text), selectedValue: "rectangle");

        //APlugin.name_YesNoItems = new(){
        //    new MyYesNo { id = "yes", caption = "はい" },
        //    new MyYesNo { id = "no", caption = "いいえ" },
        //    new MyYesNo { id = "neither", caption = "どっちゃでもない" }
        //};
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostFormDataAsync(List<string> plugins, IFormFile binary)
    {
        foreach (var p in plugins)
        {
            var sp = p.Split(':', 2);
            var plugin = sp[0];
            string pluginJstr = sp[1];
            switch (plugin)
            {
                case "my-plugin-a":
                    {
                        APluginForm? deserializedForm = JsonSerializer.Deserialize<APluginForm>(pluginJstr);
                    }
                    continue;
                case "my-plugin-b":
                    {
                        BPluginForm? deserializedForm = JsonSerializer.Deserialize<BPluginForm>(pluginJstr);
                    }
                    continue;
            }
        }

        var ms = new MemoryStream();
        using (var input = binary.OpenReadStream())
        {
            await input.CopyToAsync(ms);
            await input.FlushAsync();
            await ms.FlushAsync();
        }
        ms.Position = 0;
        return File(ms, "image/png");
    }
}

public class MyFigure
{
    public string Value { get; set; } = default!;
    public string Text { get; set; } = default!;
}

public class MyYesNo
{
    public string id { get; set; } = default!;
    public string caption { set; get; } = default!;
}

public class APluginForm
{
    [Display(Name = "はい・いいえ")]
    public List<MyYesNo> name_YesNoItems = new();

    [Display(Name = "1-5の入力")]
    public int name_MyInteger { set; get; } = 2; //label asp-for対応するにはpropが必須

    [Display(Name = "1.0-2.0で0.1刻み")]
    public double name_MyDouble { set; get; } = 1.2;

    public MyFigure name_SelectedFigureItem { set; get; } = default!;
}

public class BPluginForm
{
    [Display(Name = "チェックしてください。")]
    public bool name_MyCheckbox { set; get; }
}

//public class APluginFormData
//{
//    [JsonPropertyName("APluginForm.name_YesNoItems")]
//    public string name_YesNoItems { set; get; } = default!;
//    [JsonPropertyName("APluginForm.name_MyInteger")]
//    public int name_MyInteger { set; get; }
//    [JsonPropertyName("APluginForm.name_MyDouble")]
//    public double name_MyDouble { set; get; }
//    [JsonPropertyName("APluginForm.name_SelectedFigureItem")]
//    public string name_SelectedFigureItem { set; get; } = default!;
//}