using System.Numerics;
using ImGuiNET;
using JustConveyors.Source.ConfigurationNS;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.Controls;

internal static class GUI
{
    public static float UserZoom = 1f;
    public static event Action<float> OnZoomChanged;

    public static void Draw()
    {
        ImGui.Begin("Controls", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                ImGuiWindowFlags.NoMove);
        ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.09f, 0.13f, 0.18f, 1f));
        ImGui.SetWindowSize(new Vector2(Configuration.ControlsWidth, Configuration.WindowSizeY));
        ImGui.SetWindowPos(new Vector2(0, 0));

        ImGui.Text($"FPS: {1d / Time.DeltaTime:0}");
        ImGui.Text($"Frametime: {Time.DeltaTime * 1000d:0} ms");

        int zoomLevel = FloatToZoomLevel(UserZoom);
        ImGui.SliderInt("Zoom level", ref zoomLevel, 2, 10, $"{UserZoom}x");

        if (Math.Abs(ZoomLevelToFloat(zoomLevel) - UserZoom) > 0f)
        {
            UserZoom = ZoomLevelToFloat(zoomLevel);
            OnZoomChanged?.Invoke(UserZoom);
        }

        if (ImGui.Button("Center screen", new Vector2(Configuration.ControlsWidth - 20, 20)))
        {
            Zoom.ChangeFocusPxs(Configuration.CenterScr);
        }

        ImGui.End();
    }

    private static float ZoomLevelToFloat(int zoomLevel) => zoomLevel * 0.5f;

    private static int FloatToZoomLevel(float zoomFloat) => (int)(zoomFloat * 2f);
}
