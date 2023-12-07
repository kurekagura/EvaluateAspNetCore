using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApp.Pages.AspForm;

[IgnoreAntiforgeryToken] //【重要】検証中なので無視
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

    //public async Task<IActionResult> OnPostFormDataAsync(List<KeyValuePair<string, JsonElement>> plugins, IFormFile binary)
    public async Task<IActionResult> OnPostFormDataAsync(string plugins, IFormFile binary)
    {
        //foreach (var p in plugins)
        //{
        //    var sp = p.Split(':', 2);
        //    var plugin = sp[0];
        //    string pluginJstr = sp[1];
        //    switch (plugin)
        //    {
        //        case "my-plugin-a":
        //            {
        //                var seriOpts = new JsonSerializerOptions();
        //                seriOpts.Converters.Add(new JsonIntConverter());
        //                seriOpts.Converters.Add(new JsonDoubleConverter());
        //                var deserializedForm = JsonSerializer.Deserialize<APluginFormInput>(pluginJstr, seriOpts);
        //            }
        //            continue;
        //        case "my-plugin-b":
        //            {
        //                var seriOpts = new JsonSerializerOptions();
        //                seriOpts.Converters.Add(new JsonBoolConverter());
        //                BPluginFormInput? deserializedForm = JsonSerializer.Deserialize<BPluginFormInput>(pluginJstr, seriOpts);
        //            }
        //            continue;
        //        case "my-plugin-c":
        //            {
        //                //var seriOpts = new JsonSerializerOptions();
        //                //seriOpts.Converters.Add(new JsonBoolConverter());
        //                CPluginFormInput? deserializedForm = JsonSerializer.Deserialize<CPluginFormInput>(pluginJstr);
        //            }
        //            continue;
        //    }
        //}

        var deserialized = JsonSerializer.Deserialize<List<KeyValuePair<string, JsonElement>>>(plugins)!;
        foreach (var i in deserialized)
        {
            if (i.Key == nameof(EnumMyPlugin.PluginA))
            {
                var seriOpts = new JsonSerializerOptions();
                seriOpts.Converters.Add(new JsonIntConverter());
                seriOpts.Converters.Add(new JsonDoubleConverter());
                seriOpts.Converters.Add(new JsonEnumFigureConverter());
                seriOpts.Converters.Add(new JsonEnumYesNoConverter());
                var a = JsonSerializer.Deserialize<APluginFormInput>(i.Value, seriOpts);
            }
            else if (i.Key == nameof(EnumMyPlugin.PluginB))
            {
                var seriOpts = new JsonSerializerOptions();
                seriOpts.Converters.Add(new JsonBoolConverter());
                var b = JsonSerializer.Deserialize<BPluginFormInput>(i.Value, seriOpts);
            }
            else if (i.Key == nameof(EnumMyPlugin.PluginC))
            {
                var seriOpts = new JsonSerializerOptions();
                seriOpts.Converters.Add(new JsonBoolConverter());
                seriOpts.Converters.Add(new JsonEnumFigureConverter());
                var c = JsonSerializer.Deserialize<CPluginFormInput>(i.Value, seriOpts);
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

public enum EnumMyPlugin
{
    PluginA,
    PluginB,
    PluginC
}

public class MyFigureOption
{
    public ENumFigure Value { get; set; } = default!;
    public string Text { get; set; } = default!;
}

public enum ENumFigure
{
    Rectangle,
    Circle,
    Triangle
}

public class MyYesNo
{
    public EnumYesNo Value { get; set; } = default!;
    public string LabelText { set; get; } = default!;
}

public enum EnumYesNo
{
    Yes,
    No
}

public class APluginForm
{
    //select要素では.NETのSelectListを利用できる。
    public SelectList FigureItems { get; set; } = new SelectList(new List<MyFigureOption>()
        {
            new MyFigureOption { Value = ENumFigure.Circle, Text = "まる" },
            new MyFigureOption { Value = ENumFigure.Rectangle, Text = "四角" }
        }, dataValueField: nameof(MyFigureOption.Value), dataTextField: nameof(MyFigureOption.Text), selectedValue: ENumFigure.Rectangle);

    [Display(Name = "はい・いいえ(LOC可)")]
    public List<MyYesNo> name_YesNoItems = new() {
        new MyYesNo(){Value = EnumYesNo.Yes, LabelText ="よいです"},
        new MyYesNo(){Value = EnumYesNo.No, LabelText ="いやです"}
    };

    [Display(Name = "1-5の入力")]
    public int name_MyInteger { set; get; } = 2; //label asp-for対応するにはpropが必須

    [Display(Name = "1.0-2.0で0.1刻み")]
    public double name_MyDouble { set; get; } = 1.2;

    [Display(Name = "選択肢(LOC可)")]
    public ENumFigure name_SelectedFigureItem { set; get; } = default!;
}

public class BPluginForm
{
    [Display(Name = "チェックしてください(LOC可)")]
    public bool name_MyCheckbox { set; get; }
}

public class CPluginForm
{
    //[Display(Name = "ラジオボタン(LOC可)")]
    //public SelectList name_FigureItems { get; set; } = new SelectList(new List<MyFigure>()
    //{
    //    new MyFigure { Value = "circle", Text = "まる（label forでクリックできる）" },
    //    new MyFigure { Value = "rectangle", Text = "四角（label forでクリックできる）" },
    //    new MyFigure { Value = "triangle", Text = "三角（label forでクリックできる）" }
    //}, dataValueField: nameof(MyFigure.Value), dataTextField: nameof(MyFigure.Text), selectedValue: "rectangle");

    [Display(Name = "ラジオボタン(LOC可)")]
    public List<MyFigureOption> name_FigureItems { get; set; } = new List<MyFigureOption>()
    {
        new MyFigureOption { Value = ENumFigure.Circle, Text = "まる（label forでクリックできる）" },
        new MyFigureOption { Value = ENumFigure.Rectangle, Text = "四角（label forでクリックできる）" },
        new MyFigureOption { Value = ENumFigure.Triangle, Text = "三角（label forでクリックできる）" }
    };
}

//JSONシリアライズ用
public class APluginFormInput
{
    //ラジオの未選択を許可したい場合、Nullableにしておく必要がある
    public EnumYesNo? name_YesNoItems { set; get; } = default!;
    public int name_MyInteger { set; get; }
    public double name_MyDouble { set; get; }
    public ENumFigure name_SelectedFigureItem { set; get; } = default!;
}

public class BPluginFormInput
{
    public bool name_MyCheckbox { set; get; }
}

public class CPluginFormInput
{
    public ENumFigure name_FigureItems { set; get; }
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

public class JsonEnumFigureConverter : JsonConverter<ENumFigure>
{
    public override ENumFigure Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();
            if (ENumFigure.TryParse(stringValue, out ENumFigure value))
            {
                return value;
            }
        }

        throw new System.Text.Json.JsonException();
    }

    public override void Write(Utf8JsonWriter writer, ENumFigure value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class JsonEnumYesNoConverter : JsonConverter<EnumYesNo>
{
    public override EnumYesNo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();
            if (EnumYesNo.TryParse(stringValue, out EnumYesNo value))
            {
                return value;
            }
        }

        throw new System.Text.Json.JsonException();
    }

    public override void Write(Utf8JsonWriter writer, EnumYesNo value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}