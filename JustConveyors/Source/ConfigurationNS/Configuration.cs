using System.Numerics;
using System.Text.Json;

namespace JustConveyors.Source.ConfigurationNS;

public class Configuration
{
    private static Configuration s_currentConfiguration;
    public int windowSizeX { get; set; }
    public int windowSizeY { get; set; }
    public int controlsWidth { get; set; }
    public static int WindowSizeX => s_currentConfiguration.windowSizeX;
    public static int WindowSizeY => s_currentConfiguration.windowSizeY;
    public static int ControlsWidth => s_currentConfiguration.controlsWidth;
    public static Vector2 CenterScr => new Vector2(WindowSizeX, WindowSizeY) / 2f;

    public static void Load(string path) =>
        s_currentConfiguration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(path));
}
