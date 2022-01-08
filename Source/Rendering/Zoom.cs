using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JustConveyors.Source.Rendering
{
    internal class Zoom
    {
        public static float M { get; private set; }
        public static Vector2 FocusPosition { get; private set; }

        public static event Action<float> OnZoomChanged;
        public static event Action<Vector2> OnFocusChanged;

        public Zoom(float m, Vector2 focus)
        {
            M = m;
            FocusPosition = focus;
        }

        public static void ChangeZoom(float newZoom, out float oldZoom)
        {
            oldZoom = M;
            M = newZoom;
            OnZoomChanged?.Invoke(M);
        }

        public static void ChangeFocus(Vector2 newFocus, out Vector2 oldFocus)
        {
            oldFocus = FocusPosition;
            FocusPosition = newFocus;
            OnFocusChanged?.Invoke(FocusPosition);
        }
    }
}
