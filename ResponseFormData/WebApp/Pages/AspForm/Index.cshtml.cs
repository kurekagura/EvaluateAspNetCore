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

[IgnoreAntiforgeryToken] //�y�d�v�z���ؒ��Ȃ̂Ŗ���
public class IndexModel : PageModel
{
    private readonly IWebHostEnvironment _env;

    public IndexModel(IWebHostEnvironment webHostEnvironment)
    {
        _env = webHostEnvironment;
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
                        var seriOpts = new JsonSerializerOptions();
                        seriOpts.Converters.Add(new JsonIntConverter());
                        seriOpts.Converters.Add(new JsonDoubleConverter());
                        var deserializedForm = JsonSerializer.Deserialize<APluginFormInput>(pluginJstr, seriOpts);
                    }
                    continue;
                case "my-plugin-b":
                    {
                        var seriOpts = new JsonSerializerOptions();
                        seriOpts.Converters.Add(new JsonBoolConverter());
                        BPluginFormInput? deserializedForm = JsonSerializer.Deserialize<BPluginFormInput>(pluginJstr, seriOpts);
                    }
                    continue;
                case "my-plugin-c":
                    {
                        //var seriOpts = new JsonSerializerOptions();
                        //seriOpts.Converters.Add(new JsonBoolConverter());
                        CPluginFormInput? deserializedForm = JsonSerializer.Deserialize<CPluginFormInput>(pluginJstr);
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
    public SelectList FigureItems { get; set; } = new SelectList(new List<MyFigure>()
        {
            new MyFigure { Value = "circle", Text = "�܂�" },
            new MyFigure { Value = "rectangle", Text = "�l�p" }
        }, dataValueField: nameof(MyFigure.Value), dataTextField: nameof(MyFigure.Text), selectedValue: "rectangle");

    [Display(Name = "�͂��E������")]
    public List<MyYesNo> name_YesNoItems = new();

    [Display(Name = "1-5�̓���")]
    public int name_MyInteger { set; get; } = 2; //label asp-for�Ή�����ɂ�prop���K�{

    [Display(Name = "1.0-2.0��0.1����")]
    public double name_MyDouble { set; get; } = 1.2;

    [Display(Name = "�I����(LOC��)")]
    public MyFigure name_SelectedFigureItem { set; get; } = default!;
}

public class BPluginForm
{
    [Display(Name = "�`�F�b�N���Ă�������(LOC��)")]
    public bool name_MyCheckbox { set; get; }
}

public class CPluginForm
{
    //[Display(Name = "���W�I�{�^��(LOC��)")]
    //public SelectList name_FigureItems { get; set; } = new SelectList(new List<MyFigure>()
    //{
    //    new MyFigure { Value = "circle", Text = "�܂�ilabel for�ŃN���b�N�ł���j" },
    //    new MyFigure { Value = "rectangle", Text = "�l�p�ilabel for�ŃN���b�N�ł���j" },
    //    new MyFigure { Value = "triangle", Text = "�O�p�ilabel for�ŃN���b�N�ł���j" }
    //}, dataValueField: nameof(MyFigure.Value), dataTextField: nameof(MyFigure.Text), selectedValue: "rectangle");

    [Display(Name = "���W�I�{�^��(LOC��)")]
    public List<MyFigure> name_FigureItems { get; set; } = new List<MyFigure>()
    {
        new MyFigure { Value = "circle", Text = "�܂�ilabel for�ŃN���b�N�ł���j" },
        new MyFigure { Value = "rectangle", Text = "�l�p�ilabel for�ŃN���b�N�ł���j" },
        new MyFigure { Value = "triangle", Text = "�O�p�ilabel for�ŃN���b�N�ł���j" }
    };
}

//JSON�V���A���C�Y�p
public class APluginFormInput
{
    //[JsonPropertyName("APluginForm.name_YesNoItems")]
    public string name_YesNoItems { set; get; } = default!;
    //[JsonPropertyName("APluginForm.name_MyInteger")]
    public int name_MyInteger { set; get; }
    //[JsonPropertyName("APluginForm.name_MyDouble")]
    public double name_MyDouble { set; get; }
    //[JsonPropertyName("APluginForm.name_SelectedFigureItem")]
    public string name_SelectedFigureItem { set; get; } = default!;
}

public class BPluginFormInput
{
    public bool name_MyCheckbox { set; get; }
}

public class CPluginFormInput
{
    public string name_FigureItems { set; get; }
}

public class JsonIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();
            if (int.TryParse(stringValue, out int value))
            {
                return value;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        throw new System.Text.Json.JsonException();
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

public class JsonDoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();
            if (double.TryParse(stringValue, out double value))
            {
                return value;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
        }

        throw new System.Text.Json.JsonException();
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

public class JsonBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();
            if (bool.TryParse(stringValue, out bool value))
            {
                return value;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetBoolean();
        }

        throw new System.Text.Json.JsonException();
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}