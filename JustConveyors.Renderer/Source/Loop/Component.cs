using JustConveyors.Renderer.Source.Rendering;

namespace JustConveyors.Renderer.Source.Loop;

internal abstract class Component
{
    public Component(Display display, Texture texture)
    {
        Display = display;
        Texture = texture;
        Program.OnStart += Start;
        Program.OnUpdate += Update;
        Program.OnLateUpdate += LateUpdate;
        Program.OnClose += Close;
    }

    protected Display Display { get; }
    protected Texture Texture { get; }
    protected abstract void Start();
    protected abstract void Update();
    protected abstract void LateUpdate();

    protected virtual void Close()
    {
        Program.OnStart -= Start;
        Program.OnUpdate -= Update;
        Program.OnLateUpdate -= LateUpdate;
        Program.OnClose -= Close;
    }
}
