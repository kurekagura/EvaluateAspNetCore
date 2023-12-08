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
    public SelectList SelectSeasonItems { get; set; }

    [Display(Name = "�G�߂̑I���iDisplay�����j")]
    public EnumSeason name_SelectedSelectSeasonItem { set; get; }

    public List<EnumSeason> name_RadioSeasonItems { set; get; } = new();

    [Display(Name = nameof(All.PAGE_ENUM_ILIKESEASON))]
    public EnumSeason ILikeSeason { set; get; } = EnumSeason.Summer;

    public IndexModel()
    {
        //select�v�f�ł�.NET��SelectList�𗘗p�ł���BTuple���g����BValueTaple�̓t�B�[���h�Ȃ̂�NG?
        //Razor��@Html.DisplayTextFor�Ń��\�[�X�Ή��ł��Ȃ��͂Ȃ������iasp-items�͎g���Ȃ��Ȃ�H�j
        SelectSeasonItems = new SelectList(new List<Tuple<string, string>>()
        {
            new (nameof(EnumSeason.Spring), EnumSeason.Spring.DisplayName() ),
            new (nameof(EnumSeason.Summer), EnumSeason.Summer.DisplayName() ),
            new (nameof(EnumSeason.Autumn), EnumSeason.Autumn.DisplayName() ),
            new (nameof(EnumSeason.Winter), EnumSeason.Winter.DisplayName() )
        },
        dataValueField: "Item1",
        dataTextField: "Item2",
        selectedValue: nameof(EnumSeason.Summer) //�����@�\���ĂȂ��iTuple�̓_���H�j
        );

        name_RadioSeasonItems.AddRange(new[] {
            EnumSeason.Spring, EnumSeason.Summer, EnumSeason.Autumn, EnumSeason.Winter});
    }

    public void OnGet()
    {
    }
}
