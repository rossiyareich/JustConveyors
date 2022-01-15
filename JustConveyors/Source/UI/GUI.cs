using System.Numerics;
using ImGuiNET;
using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.UI;

internal static class GUI
{
    private static float s_displayZoom = 1f;
    public static Action<TexturePool> OnActiveSurfaceChanged;
    public static Action<TransformFlags> OnActiveSurfaceTransformChanged;
    public static Action OnClearPalette;
    private static int oldSelection;
    public static bool IsPause;

    public static void Draw()
    {
        ImGui.Begin("Controls", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                ImGuiWindowFlags.NoMove);
        ImGui.PushStyleColor(ImGuiCol.WindowBg, new Vector4(0.09f, 0.13f, 0.18f, 1f));
        ImGui.SetWindowSize(new Vector2(Configuration.ControlsWidth, Configuration.WindowSizeY));
        ImGui.SetWindowPos(new Vector2(0, 0));

        #region Performance

        ImGui.BeginChild("Performance", new Vector2(Configuration.ControlsWidth - 17, 50), true);

        ImGui.Text($"FPS: {1d / Time.DeltaTime:0}");
        ImGui.Text($"Frametime: {Time.DeltaTime * 1000d:0} ms");

        ImGui.EndChild();

        #endregion

        ImGui.Dummy(new Vector2(0, 2));

        #region Time

        ImGui.BeginChild("Misc", new Vector2(Configuration.ControlsWidth - 17, 65), true);

        if (ImGui.Button("Clear palette", new Vector2(Configuration.ControlsWidth - 40, 20)))
        {
            OnClearPalette?.Invoke();
        }

        ImGui.Dummy(new Vector2(0, 2));

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Paused");
        ImGui.SameLine(200);
        ImGui.Checkbox(new string(' ', 0), ref IsPause);

        ImGui.EndChild();

        #endregion

        ImGui.Dummy(new Vector2(0, 2));

        #region Pan&Zoom

        ImGui.BeginChild("Pan&Zoom", new Vector2(Configuration.ControlsWidth - 17, 59), true);

        int zoomLevel = FloatToZoomLevel(s_displayZoom);
        ImGui.SliderInt("Zoom level", ref zoomLevel, 2, 10, $"{s_displayZoom:0.0}x");
        if (Math.Abs(ZoomLevelToFloat(zoomLevel) - s_displayZoom) > 0f)
        {
            s_displayZoom = ZoomLevelToFloat(zoomLevel);
            Camera2D.ChangeZoom(s_displayZoom);
        }

        if (ImGui.Button("Center screen", new Vector2(Configuration.ControlsWidth - 33, 20)))
        {
            Camera2D.ChangeFocusScrRaw(Coordinates.CenterScr);
        }

        ImGui.EndChild();

        #endregion

        ImGui.Dummy(new Vector2(0, 2));

        #region Tile Selection

        int selection = oldSelection;
        ImGui.BeginChild("TileSelection", new Vector2(Configuration.ControlsWidth - 17, 200), true);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Bridge Conveyor");
        ImGui.SameLine(200);
        ImGui.RadioButton(new string(' ', 1), ref selection, 0);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Titanium Conveyor");
        ImGui.SameLine(200);
        ImGui.RadioButton(new string(' ', 2), ref selection, 1);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Thorium Conveyor");
        ImGui.SameLine(200);
        ImGui.RadioButton(new string(' ', 3), ref selection, 2);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Armored Conveyor");
        ImGui.SameLine(200);
        ImGui.RadioButton(new string(' ', 4), ref selection, 3);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Junction");
        ImGui.SameLine(200);
        ImGui.RadioButton(new string(' ', 5), ref selection, 4);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Router");
        ImGui.SameLine(200);
        ImGui.RadioButton(new string(' ', 6), ref selection, 5);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Ruby Source");
        ImGui.SameLine(200);
        ImGui.RadioButton(new string(' ', 7), ref selection, 6);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("    Ruby Sink");
        ImGui.SameLine(200);
        ImGui.RadioButton(new string(' ', 8), ref selection, 7);

        if (oldSelection != selection)
        {
            OnActiveSurfaceChanged?.Invoke(selection switch
            {
                0 => PoolResources.BridgeConveyorPool,
                1 => PoolResources.BlueConveyorPool,
                2 => PoolResources.PurpleConveyorPool,
                3 => PoolResources.RedConveyorPool,
                4 => PoolResources.JunctionPool,
                5 => PoolResources.RouterPool,
                6 => PoolResources.SourcePool,
                7 => PoolResources.SinkPool,
                _ => throw new ArgumentException($"No selection numbered selection {selection}")
            });
            oldSelection = selection;
        }

        ImGui.EndChild();

        #endregion


        ImGui.End();
    }

    private static float ZoomLevelToFloat(int zoomLevel) => zoomLevel * 0.5f;
    private static int FloatToZoomLevel(float zoomFloat) => (int)(zoomFloat * 2f);
}
