using System.Numerics;
using ImGuiNET;
using JustConveyors.Source.ConfigurationNS;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.Controls;

internal static class GUI
{
    public static void Draw()
    {
        ImGui.Begin("Controls", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                ImGuiWindowFlags.NoMove);
        ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.09f, 0.13f, 0.18f, 1f));
        ImGui.SetWindowSize(new Vector2(Configuration.ControlsWidth, Configuration.WindowSizeY));
        ImGui.SetWindowPos(new Vector2(0, 0));

        ImGui.Text($"FPS: {1d / Time.DeltaTime:0}");
        ImGui.Text($"Frametime: {Time.DeltaTime * 1000d:0} ms");
        ImGui.Text($"Zoom Level: {Zoom.M}x");

        ImGui.End();
    }
}
