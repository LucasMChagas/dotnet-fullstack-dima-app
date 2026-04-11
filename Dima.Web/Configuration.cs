using MudBlazor;
using MudBlazor.Utilities;

namespace Dima.Web;

public static class Configuration
{
    public const string HttpClientName = "Dima";
    public static string BackendUrl { get; set; } = "http://localhost:5115";
    public static MudTheme  Theme = new MudTheme
    {
        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = ["Raleway", "sans-serif"]
            }
        },
        Palette = new PaletteLight
        {
            Primary = new MudColor("#1EFA2D"),
            Secondary = Colors.LightGreen.Darken3,
            Background = Colors.Grey.Lighten4,
            AppbarBackground = new MudColor("#1EFA2D"),
            AppbarText = Colors.Shades.Black,
            TextPrimary = Colors.Shades.Black,
            PrimaryContrastText = new MudColor("#000000"),
            DrawerText = Colors.Shades.White,
            DrawerBackground = Colors.LightGreen.Darken3
        },
        PaletteDark = new PaletteDark
        {
            Primary = Colors.LightGreen.Accent3,
            Secondary = Colors.LightGreen.Darken3,
            AppbarBackground = Colors.LightGreen.Accent3,
            AppbarText = Colors.Shades.Black,
            PrimaryContrastText =  new MudColor("#000000")
        }
    };
}