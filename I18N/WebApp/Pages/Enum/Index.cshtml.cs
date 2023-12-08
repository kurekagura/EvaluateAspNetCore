using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;
using WebApp.Resx;

namespace WebApp.Pages.Enum;

[Display(Name = "PAGE_TITLE_ENUM")]
public class IndexModel : PageModel
{
    #region 属性付与の位置に注意
    [Display(Name = "季節の選択（Display属性）")]
    public SelectList SelectSeasonItems2 { get; set; }
    #endregion

    #region 属性付与の位置に注意
    public SelectList SelectSeasonItems { get; set; }
    [Display(Name = "季節の選択（Display属性）")]
    public EnumSeason name_SelectedSelectSeasonItem { set; get; }
    #endregion

    public List<EnumSeason> name_RadioSeasonItems { set; get; } = new();

    [Display(Name = nameof(All.PAGE_ENUM_ILIKESEASON))]
    public EnumSeason ILikeSeason { set; get; } = EnumSeason.Summer;

    public IndexModel()
    {
        SelectSeasonItems2 = new SelectList(new List<SeasonValueText>
        {
            new SeasonValueText{Value=EnumSeason.Spring, Text=EnumSeason.Spring.DisplayName() },
            new SeasonValueText{Value=EnumSeason.Summer, Text=EnumSeason.Summer.DisplayName() },
            new SeasonValueText{Value=EnumSeason.Autumn, Text=EnumSeason.Autumn.DisplayName() },
            new SeasonValueText{Value= EnumSeason.Winter, Text=EnumSeason.Winter.DisplayName() }
        },
        dataValueField: nameof(SeasonValueText.Value),
        dataTextField: nameof(SeasonValueText.Text),
        selectedValue: nameof(EnumSeason.Summer)    //バインドしない場合初期選択はここ
        );

        //select要素では.NETのSelectListを利用できる。Tupleが使える。ValueTapleはフィールドなのでNG?
        //Razorの@Html.DisplayTextForでリソース対応できなくはなさそう（asp-itemsは使えなくなる？）
        SelectSeasonItems = new SelectList(new List<Tuple<EnumSeason, string>>()
        {
            new (EnumSeason.Spring, EnumSeason.Spring.DisplayName() ),
            new (EnumSeason.Summer, EnumSeason.Summer.DisplayName() ),
            new (EnumSeason.Autumn, EnumSeason.Autumn.DisplayName() ),
            new (EnumSeason.Winter, EnumSeason.Winter.DisplayName() )
        },
        dataValueField: "Item1",
        dataTextField: "Item2",
        selectedValue: nameof(EnumSeason.Summer)
        );
        name_SelectedSelectSeasonItem = EnumSeason.Summer; //バインドする場合の初期選択はここ

        name_RadioSeasonItems.AddRange(new[] {
            EnumSeason.Spring, EnumSeason.Summer, EnumSeason.Autumn, EnumSeason.Winter});
    }

    public void OnGet()
    {
    }
}

public class SeasonValueText
{
    public EnumSeason Value { get; set; }
    public string Text { get; set; } = default!;
}