﻿using JustConveyors.Source.Rendering;

namespace JustConveyors.Source.Loop;

internal abstract class Component
{
    public Component(Display display)
    {
        Display = display;
        Program.OnStart += Start;
        Program.OnUpdate += Update;
        Program.OnLateUpdate += LateUpdate;
        Program.OnClose += Close;
    }

    protected Display Display { get; }
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
