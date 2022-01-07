using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace JustConveyors.Source.Rendering
{
    public struct SDL_Texture
    {
        public byte a;
        public int access;
        public byte b;
        public SDL.SDL_BlendMode blendMode;
        public IntPtr driverdata;
        public uint format;
        public byte g;
        public int h;
        public SDL.SDL_Rect locked_rect;
        public IntPtr magic;
        public int modMode;
        public IntPtr native;
        public IntPtr next;
        public int pitch;
        public IntPtr pixels;
        public byte r;
        public IntPtr renderer;
        public int w;
        public IntPtr yuv;
    }
}
