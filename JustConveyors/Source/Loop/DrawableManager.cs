using JustConveyors.Source.Drawing;
using JustConveyors.Source.Rendering;
using JustConveyors.Source.UI;

namespace JustConveyors.Source.Loop;

internal partial class DrawableManager : IDisposable
{
    public readonly Display Display;
    public readonly IEventHolder EventHolder;

    public readonly Texture Texture;

    public DrawableManager(Display display, Texture texture, IEventHolder eventHolder)
    {
        Drawables = new List<Drawable>();
        Display = display;
        Texture = texture;
        EventHolder = eventHolder;
        PoolResources = PoolResources.GetInstance();

        GUI.OnClearPalette += () =>
        {
            foreach (Drawable drawable in Drawables)
            {
                if (drawable.ParentPool == SDLEventHandler.ActiveBlock.ParentPool)
                {
                    continue;
                }

                drawable.CloseStateless();
            }

            Drawable x = Drawables.FirstOrDefault(x => x.ParentPool == SDLEventHandler.ActiveBlock.ParentPool);
            Drawables.Clear();
            Drawables.Add(x);
        };
    }

    public PoolResources PoolResources { get; }
    public List<Drawable> Drawables { get; set; }

    public void Dispose() => PoolResources.Dispose();
}
