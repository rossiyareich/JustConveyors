using JustConveyors.Source.Conveyor;
using JustConveyors.Source.Rendering;
using SDL2;

namespace JustConveyors.Source.Loop;

internal class ComponentManager
{
    public ComponentManager() => Components = new List<Component>();

    public List<Component> Components { get; }

    public T AddComponent<T>(T component) where T : Component
    {
        Components.Add(component);
        return component;
    }

    /// <remarks>
    ///     Put all component initializations here
    /// </remarks>
    public void InitializeComponents(Display display)
    {
        for (int i = 0; i < 1280; i += 128)
        {
            for (int j = 0; j < 800; j += 128)
            {
                SDL.SDL_Rect parent = new() { w = 128, h = 128, x = i, y = j };
                _ = new Aninmatable(display, ref parent, 100,
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
                    @"Resources\Assets\junction\output (Junction) 17.png"
                );
            }
        }
    }
}
