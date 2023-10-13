using System.Diagnostics.CodeAnalysis;

namespace KrileDotNet;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Helpers
{
    public static string FormatStringWithSpaces(object s)
    {
        return string.Concat((s.ToString() ?? string.Empty).Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
    }

    public static Dictionary<string, List<string>> FFXIV_Zones => new Dictionary<string, List<string>>()
    {
        {"Endwalker", Zones_EW},
        {"Shadowbringers", Zones_ShB},
        {"Stormblood", Zones_SB},
        {"Heavensward", Zones_HW},
        {"A Realm Reborn", Zones_ARR},
    };

    private static List<string> Zones_EW => new List<string>()
    {
        "Radz-at-Han",
        "Thavnair",
        "Garlemald",
        "Old Sharlayan", 
        "Labyrinthos", 
        "Mare Lamentorum", 
        "Ultima Thule", 
        "Elpis"
    };

    private static List<string> Zones_ShB => new List<string>()
    {
        "Lakeland",
        "Kholusia",
        "Amh Araeng",
        "Il Mheg",
        "The Rak'tika Greatwood",
        "The Tempest"
    };
    
    private static List<string> Zones_SB => new List<string>()
    {
        "The Ruby Sea",
        "Yanxia",
        "The Azim Step",
        "The Fringes",
        "The Peaks",
        "The Lochs"
    };
    
    private static List<string> Zones_HW => new List<string>()
    {
        "Coerthas Western Highlands",
        "The Sea of Clouds",
        "Azys Lla",
        "The Dravanian Forelands",
        "The Churning Mists"
    };
    
    private static List<string> Zones_ARR => new List<string>()
    {
        "Western Thanalan",
        "Central Thanalan",
        "Eastern Thanalan",
        "Southern Thanalan",
        "Northern Thanalan",
        "The Gold Saucer",
        "Coerthas Central Highlands",
        "Mor Dhona",
        "Central Shroud",
        "East Shroud",
        "South Shroud",
        "North Shroud",
        "Middle La Noscea",
        "Lower La Noscea",
        "Eastern La Noscea",
        "Western La Noscea",
        "Upper La Noscea",
        "Outer La Noscea"
    };
}