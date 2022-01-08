namespace JustConveyors.Source.Rendering;

internal class PoolResources : IDisposable
{
    private static PoolResources s_instance;

    public static readonly TexturePool BridgeConveyorPool = new(
        @"Resources\Assets\bridge_conveyor\output (BridgeCI) 0.png",
        @"Resources\Assets\bridge_conveyor\output (BridgeCS) 0.png");

    public static readonly TexturePool BridgeConveyorLinePool = new(
        @"Resources\Assets\bridge_conveyor\output (BridgeCon) 0.png");

    public static readonly TexturePool ConveyorsPool = new(
        @"Resources\Assets\conveyors\output (BluC) 0.png",
        @"Resources\Assets\conveyors\output (BluCC) 0.png",
        @"Resources\Assets\conveyors\output (PurC) 0.png",
        @"Resources\Assets\conveyors\output (PurCC) 0.png",
        @"Resources\Assets\conveyors\output (RedC) 0.png",
        @"Resources\Assets\conveyors\output (RedCC) 0.png");

    public static readonly TexturePool ConveyorArrowsPool = new(
        @"Resources\Assets\conveyors\output (BluArw) 0.png",
        @"Resources\Assets\conveyors\output (PurArw) 0.png",
        @"Resources\Assets\conveyors\output (RedArw) 0.png");

    public static readonly TexturePool JunctionPool = new(
        @"Resources\Assets\junction\output (Junction) 0.png",
        @"Resources\Assets\junction\output (Junction) 1.png",
        @"Resources\Assets\junction\output (Junction) 2.png",
        @"Resources\Assets\junction\output (Junction) 3.png",
        @"Resources\Assets\junction\output (Junction) 4.png",
        @"Resources\Assets\junction\output (Junction) 5.png",
        @"Resources\Assets\junction\output (Junction) 6.png",
        @"Resources\Assets\junction\output (Junction) 7.png",
        @"Resources\Assets\junction\output (Junction) 8.png",
        @"Resources\Assets\junction\output (Junction) 9.png",
        @"Resources\Assets\junction\output (Junction) 10.png",
        @"Resources\Assets\junction\output (Junction) 11.png",
        @"Resources\Assets\junction\output (Junction) 12.png",
        @"Resources\Assets\junction\output (Junction) 13.png",
        @"Resources\Assets\junction\output (Junction) 14.png",
        @"Resources\Assets\junction\output (Junction) 15.png",
        @"Resources\Assets\junction\output (Junction) 16.png",
        @"Resources\Assets\junction\output (Junction) 17.png",
        @"Resources\Assets\junction\output (Junction) 18.png",
        @"Resources\Assets\junction\output (Junction) 19.png");

    public static readonly TexturePool GuidePool = new(
        @"Resources\Assets\misc\output (Guide) 0.png");

    public static readonly TexturePool RubyPool = new(
        @"Resources\Assets\misc\output (Ruby) 0.png");

    public static readonly TexturePool RouterPool = new(
        @"Resources\Assets\router\output (Router) 0.png",
        @"Resources\Assets\router\output (Router) 1.png",
        @"Resources\Assets\router\output (Router) 2.png",
        @"Resources\Assets\router\output (Router) 3.png",
        @"Resources\Assets\router\output (Router) 4.png",
        @"Resources\Assets\router\output (Router) 5.png",
        @"Resources\Assets\router\output (Router) 6.png",
        @"Resources\Assets\router\output (Router) 7.png",
        @"Resources\Assets\router\output (Router) 8.png",
        @"Resources\Assets\router\output (Router) 9.png",
        @"Resources\Assets\router\output (Router) 10.png",
        @"Resources\Assets\router\output (Router) 11.png",
        @"Resources\Assets\router\output (Router) 12.png",
        @"Resources\Assets\router\output (Router) 13.png",
        @"Resources\Assets\router\output (Router) 14.png",
        @"Resources\Assets\router\output (Router) 15.png",
        @"Resources\Assets\router\output (Router) 16.png",
        @"Resources\Assets\router\output (Router) 17.png",
        @"Resources\Assets\router\output (Router) 18.png",
        @"Resources\Assets\router\output (Router) 19.png");

    public static readonly TexturePool SpawnersPool = new(
        @"Resources\Assets\spawner\output (Sink) 0.png",
        @"Resources\Assets\spawner\output (Source) 0.png");

    private PoolResources()
    {
        BridgeConveyorPool.Generate();
        BridgeConveyorLinePool.Generate();
        ConveyorsPool.Generate();
        ConveyorArrowsPool.Generate();
        JunctionPool.Generate();
        GuidePool.Generate();
        RubyPool.Generate();
        RouterPool.Generate();
        SpawnersPool.Generate();
    }

    public void Dispose()
    {
        BridgeConveyorPool.Dispose();
        BridgeConveyorLinePool.Dispose();
        ConveyorsPool.Dispose();
        ConveyorArrowsPool.Dispose();
        JunctionPool.Dispose();
        GuidePool.Dispose();
        RubyPool.Dispose();
        RouterPool.Dispose();
        SpawnersPool.Dispose();
    }

    public static PoolResources GetInstance()
    {
        if (s_instance == null)
        {
            s_instance = new PoolResources();
        }

        return s_instance;
    }
}
