﻿using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Rendering;

internal class PoolResources : IDisposable
{
    private static PoolResources s_instance;

    public static readonly TexturePool BridgeConveyorPool = new(false,
        (@"Resources\Assets\bridge_conveyor\output (BridgeCI) 0.png", TransformFlags.None),
        (@"Resources\Assets\bridge_conveyor\output (BridgeCS) 0.png", TransformFlags.None));

    public static readonly TexturePool BridgeConveyorLinePool = new(false,
        (@"Resources\Assets\bridge_conveyor\output (BridgeCon) 0.png", TransformFlags.Horizontal),
        (@"Resources\Assets\bridge_conveyor\output (BridgeCon) 1.png", TransformFlags.Vertical));

    public static readonly TexturePool BlueConveyorPool = new(false,
        (@"Resources\Assets\conveyors\output (BluC) 0.png", TransformFlags.Horizontal),
        (@"Resources\Assets\conveyors\output (BluC) 1.png", TransformFlags.Vertical),
        (@"Resources\Assets\conveyors\output (BluCC) 0.png", TransformFlags.DirNE),
        (@"Resources\Assets\conveyors\output (BluCC) 1.png", TransformFlags.DirNW),
        (@"Resources\Assets\conveyors\output (BluCC) 2.png", TransformFlags.DirSW),
        (@"Resources\Assets\conveyors\output (BluCC) 3.png", TransformFlags.DirSE));

    public static readonly TexturePool PurpleConveyorPool = new(false,
        (@"Resources\Assets\conveyors\output (PurC) 0.png", TransformFlags.Horizontal),
        (@"Resources\Assets\conveyors\output (PurC) 1.png", TransformFlags.Vertical),
        (@"Resources\Assets\conveyors\output (PurCC) 0.png", TransformFlags.DirNE),
        (@"Resources\Assets\conveyors\output (PurCC) 1.png", TransformFlags.DirNW),
        (@"Resources\Assets\conveyors\output (PurCC) 2.png", TransformFlags.DirSW),
        (@"Resources\Assets\conveyors\output (PurCC) 3.png", TransformFlags.DirSE));

    public static readonly TexturePool RedConveyorPool = new(false,
        (@"Resources\Assets\conveyors\output (RedC) 0.png", TransformFlags.Horizontal),
        (@"Resources\Assets\conveyors\output (RedC) 1.png", TransformFlags.Vertical),
        (@"Resources\Assets\conveyors\output (RedCC) 0.png", TransformFlags.DirNE),
        (@"Resources\Assets\conveyors\output (RedCC) 1.png", TransformFlags.DirNW),
        (@"Resources\Assets\conveyors\output (RedCC) 2.png", TransformFlags.DirSW),
        (@"Resources\Assets\conveyors\output (RedCC) 3.png", TransformFlags.DirSE));

    public static readonly TexturePool BlueConveyorArrowPool = new(false,
        (@"Resources\Assets\conveyors\output (BluArw) 0.png", TransformFlags.DirE),
        (@"Resources\Assets\conveyors\output (BluArw) 1.png", TransformFlags.DirS),
        (@"Resources\Assets\conveyors\output (BluArw) 2.png", TransformFlags.DirW),
        (@"Resources\Assets\conveyors\output (BluArw) 3.png", TransformFlags.DirN));

    public static readonly TexturePool PurpleConveyorArrowPool = new(false,
        (@"Resources\Assets\conveyors\output (PurArw) 0.png", TransformFlags.DirE),
        (@"Resources\Assets\conveyors\output (PurArw) 1.png", TransformFlags.DirS),
        (@"Resources\Assets\conveyors\output (PurArw) 2.png", TransformFlags.DirW),
        (@"Resources\Assets\conveyors\output (PurArw) 3.png", TransformFlags.DirN));

    public static readonly TexturePool RedConveyorArrowPool = new(false,
        (@"Resources\Assets\conveyors\output (RedArw) 0.png", TransformFlags.DirE),
        (@"Resources\Assets\conveyors\output (RedArw) 1.png", TransformFlags.DirS),
        (@"Resources\Assets\conveyors\output (RedArw) 2.png", TransformFlags.DirW),
        (@"Resources\Assets\conveyors\output (RedArw) 3.png", TransformFlags.DirN));

    public static readonly TexturePool JunctionPool = new(true,
        (@"Resources\Assets\junction\output (Junction) 0.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 1.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 2.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 3.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 4.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 5.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 6.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 7.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 8.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 9.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 10.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 11.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 12.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 13.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 14.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 15.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 16.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 17.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 18.png", TransformFlags.None),
        (@"Resources\Assets\junction\output (Junction) 19.png", TransformFlags.None));

    public static readonly TexturePool GuidePool = new(false,
        (@"Resources\Assets\misc\output (Guide) 0.png", TransformFlags.Horizontal),
        (@"Resources\Assets\misc\output (Guide) 1.png", TransformFlags.Vertical));

    public static readonly TexturePool RubyPool = new(false,
        (@"Resources\Assets\misc\output (Ruby) 0.png", TransformFlags.None));

    public static readonly TexturePool RouterPool = new(true,
        (@"Resources\Assets\router\output (Router) 0.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 1.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 2.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 3.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 4.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 5.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 6.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 7.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 8.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 9.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 10.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 11.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 12.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 13.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 14.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 15.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 16.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 17.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 18.png", TransformFlags.None),
        (@"Resources\Assets\router\output (Router) 19.png", TransformFlags.None));

    public static readonly TexturePool SourcePool = new(false,
        (@"Resources\Assets\spawner\output (Source) 0.png", TransformFlags.None));

    public static readonly TexturePool SinkPool = new(false,
        (@"Resources\Assets\spawner\output (Sink) 0.png", TransformFlags.None));

    private PoolResources()
    {
        BridgeConveyorPool.Generate();
        BridgeConveyorLinePool.Generate();
        BlueConveyorPool.Generate();
        PurpleConveyorPool.Generate();
        RedConveyorPool.Generate();
        BlueConveyorArrowPool.Generate();
        PurpleConveyorArrowPool.Generate();
        RedConveyorArrowPool.Generate();
        JunctionPool.Generate();
        GuidePool.Generate();
        RubyPool.Generate();
        RouterPool.Generate();
        SourcePool.Generate();
        SinkPool.Generate();
    }

    public void Dispose()
    {
        BridgeConveyorPool.Dispose();
        BridgeConveyorLinePool.Dispose();
        BlueConveyorPool.Dispose();
        PurpleConveyorPool.Dispose();
        RedConveyorPool.Dispose();
        BlueConveyorArrowPool.Dispose();
        PurpleConveyorArrowPool.Dispose();
        RedConveyorArrowPool.Dispose();
        JunctionPool.Dispose();
        GuidePool.Dispose();
        RubyPool.Dispose();
        RouterPool.Dispose();
        SourcePool.Dispose();
        SinkPool.Dispose();
    }

    public static PoolResources GetInstance()
    {
        if (s_instance is null)
        {
            s_instance = new PoolResources();
        }

        return s_instance;
    }
}
