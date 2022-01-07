namespace JustConveyors.Source.Rendering;

internal class PoolResources : IDisposable
{
    private static PoolResources s_instance;

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
        @"Resources\Assets\junction\output (Junction) 17.png");

    private PoolResources() => JunctionPool.Generate();

    public void Dispose() => JunctionPool.Dispose();

    public static PoolResources GetInstance()
    {
        if (s_instance == null)
        {
            s_instance = new PoolResources();
        }

        return s_instance;
    }
}
