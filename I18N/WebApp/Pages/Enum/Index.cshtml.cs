using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebApp.Resx;

namespace WebApp.Pages.Enum;

[Display(Name="PAGE_TITLE_ENUM")]
public class IndexModel : PageModel
{
    public List<EnumSeason> name_SeasonItems { set; get; } = new();

    [Display(Name = nameof(All.PAGE_ENUM_ILIKESEASON))]
    public EnumSeason ILikeSeason { set; get; } = EnumSeason.Summer;

    public IndexModel()
    {
        name_SeasonItems.AddRange(new[] { 
            EnumSeason.Spring, EnumSeason.Summer, EnumSeason.Autumn, EnumSeason.Winter});
    }

    public void OnGet()
    {
    }
}
