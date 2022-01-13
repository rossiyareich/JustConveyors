﻿using System.Diagnostics;
using JustConveyors.Source.Drawing;

namespace JustConveyors.Source.Scripting;

internal class testConveyorScript : AnimatableScript
{
    private readonly Stopwatch _movementWatch = Stopwatch.StartNew();

    public testConveyorScript(Animatable animatable) : base(animatable)
    {
    }

    public override void Start()
    {
    }

    public override void Update()
    {
        if (_movementWatch.ElapsedMilliseconds > 500)
        {
            Animatable.Transform = Animatable.Transform with { x = Animatable.Transform.x + 16 };
            _movementWatch.Restart();
            Debug.WriteLine(Animatable.Manager.GetDrawable<testConveyorScript>(
                (Animatable.WorldSpaceTileTransform.X, Animatable.WorldSpaceTileTransform.Y), true
            ) is null
                ? "no"
                : "yes");
        }
    }

    public override void LateUpdate()
    {
    }

    public override void Close()
    {
    }
}
