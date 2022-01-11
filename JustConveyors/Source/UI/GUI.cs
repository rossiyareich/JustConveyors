using System.Numerics;
using ImGuiNET;
using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.UI;

internal static class GUI
{
    private static float s_displayZoom = 1f;

    public static void Draw()
    {
        ImGui.Begin("Controls", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                ImGuiWindowFlags.NoMove);
        ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.09f, 0.13f, 0.18f, 1f));
        ImGui.SetWindowSize(new Vector2(Configuration.ControlsWidth, Configuration.WindowSizeY));
        ImGui.SetWindowPos(new Vector2(0, 0));

        ImGui.Text($"FPS: {1d / Time.DeltaTime:0}");
        ImGui.Text($"Frametime: {Time.DeltaTime * 1000d:0} ms");

        int zoomLevel = FloatToZoomLevel(s_displayZoom);
        ImGui.SliderInt("Zoom level", ref zoomLevel, 2, 10, $"{s_displayZoom}x");
        if (Math.Abs(ZoomLevelToFloat(zoomLevel) - s_displayZoom) > 0f)
        {
            s_displayZoom = ZoomLevelToFloat(zoomLevel);
            Camera2D.ChangeZoom(s_displayZoom);
        }

        if (ImGui.Button("Center screen", new Vector2(Configuration.ControlsWidth - 20, 20)))
        {
            Camera2D.ChangeFocusScrRaw(Coordinates.CenterScr);
        }

        ImGui.End();
    }

    private static float ZoomLevelToFloat(int zoomLevel) => zoomLevel * 0.5f;
    private static int FloatToZoomLevel(float zoomFloat) => (int)(zoomFloat * 2f);
}
