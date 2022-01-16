using JustConveyors.Source.Drawing;
using JustConveyors.Source.Loop;
using JustConveyors.Source.Rendering;
using JustConveyors.Source.Scripting;

namespace JustConveyors.Source.UI;

internal partial class SDLEventHandler
{
    private readonly Func<Drawable, bool> _checkInteractability =
        x => ScriptHelper.NonInteractableScriptTypes.All(y => y != x.ParentPool.ScriptType);

    private readonly DrawableManager _drawableManager;
    private (int, int) _endLine;
    private (int, int) _endLine2;
    private (int, int) _endLineDraw;
    private TransformFlags _hv;
    private TransformFlags _hv2;

    private (int, int) _startLine;
    private (int, int) _startLine2;
    private (int, int) _startLineDraw;

    public SDLEventHandler(Display display, DrawableManager drawableManager)
    {
        _display = display;
        _drawableManager = drawableManager;
        GUI.OnActiveSurfaceChanged += OnActiveSurfaceChanged;
        GUI.OnActiveSurfaceTransformChanged += OnActiveSurfaceTransformChanged;
        OnActiveSurfaceChanged(PoolResources.BridgeConveyorPool);
    }

    public static Drawable ActiveBlock { get; private set; }

    private void OnActiveSurfaceTransformChanged(TransformFlags newFlag) => ActiveBlock.SetSurface(newFlag);

    private void OnActiveSurfaceChanged(TexturePool newSurface)
    {
        ActiveBlock?.ParentPool?.Dispose();
        ActiveBlock?.Close();
        ActiveBlock = Drawable.Instantiate(_drawableManager, Configuration.ControlsWidth, Configuration.WindowSizeY,
            new TexturePool(newSurface), 0, 5,
            TransformFlags.IndexZero);
        ActiveBlock.SetAlpha(100, 0, ActiveBlock.Surfaces.Count);
    }

    public void PollDrawing()
    {
        Coordinates.UpdatePointer();
        ActiveBlock.Transform = ActiveBlock.Transform with
        {
            x = Coordinates.PointingToTileScreenSpace.X, y = Coordinates.PointingToTileScreenSpace.Y
        };

        if (_isWaitingMouseMiddleUp)
        {
            Camera2D.ChangeFocusScrRaw(_clickDownInitFocus + (Coordinates.PointingToRaw - _clickDownFocus));
        }

        if (_oldTileScreenSpaceMouseAlways != Coordinates.PointingToTileScreenSpace)
        {
            TryDestroyArrowSwallow();

            if (_isLimitDrawingToVertical && _isLimitDrawingToHorizontal)
            {
                if (_oldTileScreenSpaceMouseAlways.X != Coordinates.PointingToTileScreenSpace.X)
                {
                    _isLimitDrawingToVertical = false;
                    _permY = Coordinates.PointingToTileScreenSpace.Y;
                    SetHorizontal(_oldTileScreenSpaceMouseAlways);
                }
                else if (_oldTileScreenSpaceMouseAlways.Y != Coordinates.PointingToTileScreenSpace.Y)
                {
                    _isLimitDrawingToHorizontal = false;
                    _permX = Coordinates.PointingToTileScreenSpace.X;
                    SetVertical(_oldTileScreenSpaceMouseAlways);
                }

                ScriptHelper.DestroyGuideLines(_drawableManager, _startLine, _endLine, _hv);
                ScriptHelper.DestroyGuideLines(_drawableManager, _startLine2, _endLine2, _hv2);
                ScriptHelper.DrawGuideLines(_drawableManager, _startLineDraw, _endLineDraw, _hv2);
                _isStopDrawingGuides = true;
            }

            if (!_isStopDrawingGuides)
            {
                if (_isLimitDrawingToVertical)
                {
                    SetHorizontal(_oldTileScreenSpaceMouseAlways);
                    RedrawGuide();
                }

                if (_isLimitDrawingToHorizontal)
                {
                    SetVertical(_oldTileScreenSpaceMouseAlways);
                    RedrawGuide();
                }

                if (ActiveBlock.ParentPool.ScrollType == ScrollTransformFlags.FourDirections &&
                    Coordinates.IsInCanvasBounds)
                {
                    DrawGuideFromActive();
                }

                _oldTileScreenSpaceMouseAlways = Coordinates.PointingToTileScreenSpace;
            }
        }

        if (_isLimitDrawingToHorizontal)
        {
            Coordinates.PointingToTileScreenSpace.Y = _permY;
        }
        else if (_isLimitDrawingToVertical)
        {
            Coordinates.PointingToTileScreenSpace.X = _permX;
        }

        IEnumerable<Drawable> occupiedBlock =
            _drawableManager.GetDrawables(Coordinates.GetWorldSpaceTile(Coordinates.PointingToTileScreenSpace),
                ActiveBlock).ToList();

        if (_isMouseLeftDragging && Coordinates.IsInCanvasBounds)
        {
            if (!occupiedBlock.Any(_checkInteractability))
            {
                if (ActiveBlock.ParentPool.IsAninmatable)
                {
                    Animatable animatable = Animatable.Instantiate(_drawableManager,
                        Coordinates.PointingToTileScreenSpace.X,
                        Coordinates.PointingToTileScreenSpace.Y
                        , ActiveBlock.ParentPool.OriginalPool, 0,
                        100, true,
                        2);
                    animatable.Script =
                        Activator.CreateInstance(animatable.ParentPool.ScriptType, animatable) as
                            AnimatableScript;
                }
                else
                {
                    Drawable drawable = Drawable.Instantiate(_drawableManager,
                        Coordinates.PointingToTileScreenSpace.X,
                        Coordinates.PointingToTileScreenSpace.Y, ActiveBlock.ParentPool.OriginalPool, 0,
                        2,
                        ActiveBlock.Rotation);
                    drawable.Script =
                        Activator.CreateInstance(drawable.ParentPool.ScriptType, drawable) as
                            DrawableScript;
                }
            }
        }

        if (_isMouseRightDragging)
        {
            occupiedBlock.FirstOrDefault(_checkInteractability)?.Close();
        }
    }

    private void SetHorizontal((int X, int Y) coord)
    {
        _startLine = (coord.X, 0);
        _endLine = (coord.X, Configuration.WindowSizeY);
        _hv = TransformFlags.Vertical;
        _startLine2 = (Configuration.ControlsWidth, coord.Y);
        _endLine2 = (Configuration.WindowSizeX, coord.Y);
        _hv2 = TransformFlags.Horizontal;
        _startLineDraw = (Configuration.ControlsWidth, Coordinates.PointingToTileScreenSpace.Y);
        _endLineDraw = (Configuration.WindowSizeX, Coordinates.PointingToTileScreenSpace.Y);
    }

    private void SetVertical((int X, int Y) coord)
    {
        _startLine = (Configuration.ControlsWidth, coord.Y);
        _endLine = (Configuration.WindowSizeX, coord.Y);
        _hv = TransformFlags.Horizontal;
        _startLine2 = (coord.X, 0);
        _endLine2 = (coord.X, Configuration.WindowSizeY);
        _hv2 = TransformFlags.Vertical;
        _startLineDraw = (Coordinates.PointingToTileScreenSpace.X, 0);
        _endLineDraw = (Coordinates.PointingToTileScreenSpace.X, Configuration.WindowSizeY);
    }

    private void RedrawGuide()
    {
        ScriptHelper.DestroyGuideLines(_drawableManager, _startLine, _endLine, _hv);
        ScriptHelper.DrawGuideLines(_drawableManager, _startLineDraw, _endLineDraw, _hv);
    }

    private void TryDestroyArrowSwallow()
    {
        try
        {
            ScriptHelper.TryDestroyArrow(_drawableManager,
                _oldTileScreenSpaceMouseAlways.TryGetScrSpaceArwXY(ActiveBlock.Rotation));
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void DrawGuideFromActive() =>
        ScriptHelper.DrawArrow(_drawableManager,
            Coordinates.PointingToTileScreenSpace.TryGetScrSpaceArwXY(ActiveBlock.Rotation),
            ActiveBlock.TryGetArwFromBlock(), ActiveBlock.Rotation);
}
