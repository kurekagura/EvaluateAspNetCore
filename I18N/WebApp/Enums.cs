using System.ComponentModel.DataAnnotations;
using WebApp.Resx;

namespace WebApp;

public enum EnumSeason
{
    [Display(Name = nameof(All.ESEASON_SPRING))]
    Spring,
    [Display(Name = nameof(All.ESEASON_SUMMER), ResourceType = typeof(All))]
    Summer,
    [Display(Name = nameof(All.ESEASON_AUTUMN), ResourceType = typeof(All))]
    Autumn,
    [Display(Name = nameof(All.ESEASON_WINTER), ResourceType = typeof(All))]
    Winter
}
