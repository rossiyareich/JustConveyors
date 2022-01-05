using System.Runtime.InteropServices;
using System.Text;
using static SDL2.SDL;

namespace SDLImGuiGL;

public static partial class GL
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void AttachShader(uint program, uint shader);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BindBuffer(BufferTarget target, uint buffer);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BindSampler(uint unit, uint sampler);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BindTexture(TextureTarget target, uint texture);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BindVertexArray(uint array);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BlendEquation(BlendEquationMode mode);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BlendFunc(BlendingFactorSrc sfactor, BlendingFactorDest dfactor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BufferData(BufferTarget target, IntPtr size, IntPtr data, BufferUsageHint usage);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Clear(ClearBufferMask mask);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ClearColor(float r, float g, float b, float a);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void CompileShader(uint shader);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate uint CreateProgram();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate uint CreateShader(ShaderType shaderType);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DeleteBuffers(int n, uint[] buffers);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DeleteProgram(uint program);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DeleteShader(uint shader);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DeleteTextures(int n, uint[] textures);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DeleteVertexArrays(int n, uint[] arrays);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DetachShader(uint program, uint shader);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Disable(EnableCap cap);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DisableVertexAttribArray(uint index);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DrawElementsBaseVertex(BeginMode mode, int count, DrawElementsType type, IntPtr indices,
        int basevertex);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Enable(EnableCap cap);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void EnableVertexAttribArrayDel(uint index);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GenBuffers(int n, [Out] uint[] buffers);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GenTextures(int n, [OutAttribute] uint[] textures);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GenVertexArrays(int n, [Out] uint[] arrays);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetActiveAttrib(uint program, uint index, int bufSize, [Out] int[] length, [Out] int[] size,
        [Out] ActiveAttribType[] type, [Out] StringBuilder name);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetActiveUniform(uint program, uint index, int bufSize, [OutAttribute] int[] length,
        [OutAttribute] int[] size, [OutAttribute] ActiveUniformType[] type, [OutAttribute] StringBuilder name);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetAttribLocation(uint program, string name);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetProgramInfoLogDel(uint program, int maxLength, [OutAttribute] int[] length,
        [OutAttribute] StringBuilder infoLog);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetProgramiv(uint program, ProgramParameter pname, [Out] int[] @params);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetShaderInfoLogDel(uint shader, int maxLength, [Out] int[] length,
        [Out] StringBuilder infoLog);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GetShaderiv(uint shader, ShaderParameter pname, [Out] int[] @params);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetUniformLocation(uint program, string name);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void LinkProgram(uint program);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void PixelStorei(PixelStoreParameter pname, int param);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Scissor(int x, int y, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ShaderSourceDel(uint shader, int count, string[] @string, int[] length);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width,
        int height, int border, PixelFormat format, PixelType type, IntPtr data);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void TexParameteri(TextureTarget target, TextureParameterName pname, TextureParameter param);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Uniform1f(int location, float v0);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Uniform1i(int location, int v0);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Uniform2f(int location, float v0, float v1);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Uniform3f(int location, float v0, float v1, float v2);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Uniform3fv(int location, int count, float[] value);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Uniform4f(int location, float v0, float v1, float v2, float v3);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Uniform4fv(int location, int count, float[] value);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void UniformMatrix3fvDel(int location, int count, bool transpose, float[] value);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void UniformMatrix4fvDel(int location, int count, bool transpose, float[] value);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void UseProgram(uint program);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void VertexAttribPointerDel(uint index, int size, VertexAttribPointerType type, bool normalized,
        int stride, IntPtr pointer);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Viewport(int x, int y, int width, int height);

    private static readonly GetString _GetString = _<GetString>();
    public static GenBuffers glGenBuffers = _<GenBuffers>();
    public static DeleteBuffers glDeleteBuffers = _<DeleteBuffers>();
    public static Viewport glViewport = _<Viewport>();
    public static ClearColor glClearColor = _<ClearColor>();
    public static Clear glClear = _<Clear>();
    public static Enable glEnable = _<Enable>();
    public static Disable glDisable = _<Disable>();
    public static BlendEquation glBlendEquation = _<BlendEquation>();
    public static BlendFunc glBlendFunc = _<BlendFunc>();
    public static UseProgram glUseProgram = _<UseProgram>();
    public static GetShaderiv glGetShaderiv = _<GetShaderiv>();
    public static GetShaderInfoLogDel glGetShaderInfoLog = _Del<GetShaderInfoLogDel>();
    public static CreateShader glCreateShader = _<CreateShader>();
    public static ShaderSourceDel glShaderSource = _Del<ShaderSourceDel>();
    public static CompileShader glCompileShader = _<CompileShader>();
    public static DeleteShader glDeleteShader = _<DeleteShader>();
    public static GetProgramiv glGetProgramiv = _<GetProgramiv>();
    public static GetProgramInfoLogDel glGetProgramInfoLog = _Del<GetProgramInfoLogDel>();
    public static CreateProgram glCreateProgram = _<CreateProgram>();
    public static AttachShader glAttachShader = _<AttachShader>();
    public static LinkProgram glLinkProgram = _<LinkProgram>();
    public static GetUniformLocation glGetUniformLocation = _<GetUniformLocation>();
    public static GetAttribLocation glGetAttribLocation = _<GetAttribLocation>();
    public static DetachShader glDetachShader = _<DetachShader>();
    public static DeleteProgram glDeleteProgram = _<DeleteProgram>();
    public static GetActiveAttrib glGetActiveAttrib = _<GetActiveAttrib>();
    public static GetActiveUniform glGetActiveUniform = _<GetActiveUniform>();
    public static Uniform1f glUniform1f = _<Uniform1f>();
    public static Uniform2f glUniform2f = _<Uniform2f>();
    public static Uniform3f glUniform3f = _<Uniform3f>();
    public static Uniform4f glUniform4f = _<Uniform4f>();
    public static Uniform1i glUniform1i = _<Uniform1i>();
    public static Uniform3fv glUniform3fv = _<Uniform3fv>();
    public static Uniform4fv glUniform4fv = _<Uniform4fv>();
    public static UniformMatrix3fvDel glUniformMatrix3fv = _Del<UniformMatrix3fvDel>();
    public static UniformMatrix4fvDel glUniformMatrix4fv = _Del<UniformMatrix4fvDel>();
    public static BindSampler glBindSampler = _<BindSampler>();
    public static BindVertexArray glBindVertexArray = _<BindVertexArray>();
    public static BindBuffer glBindBuffer = _<BindBuffer>();
    public static EnableVertexAttribArrayDel glEnableVertexAttribArray = _Del<EnableVertexAttribArrayDel>();
    public static DisableVertexAttribArray glDisableVertexAttribArray = _<DisableVertexAttribArray>();
    public static VertexAttribPointerDel glVertexAttribPointer = _Del<VertexAttribPointerDel>();
    public static BindTexture glBindTexture = _<BindTexture>();
    public static BufferData glBufferData = _<BufferData>();
    public static Scissor glScissor = _<Scissor>();
    public static DrawElementsBaseVertex glDrawElementsBaseVertex = _<DrawElementsBaseVertex>();
    public static DeleteVertexArrays glDeleteVertexArrays = _<DeleteVertexArrays>();
    public static GenVertexArrays glGenVertexArrays = _<GenVertexArrays>();
    public static GenTextures glGenTextures = _<GenTextures>();
    public static PixelStorei glPixelStorei = _<PixelStorei>();
    public static TexImage2D glTexImage2D = _<TexImage2D>();
    public static TexParameteri glTexParameteri = _<TexParameteri>();
    public static DeleteTextures glDeleteTextures = _<DeleteTextures>();

    private static T _<T>() where T : class
    {
        string method = "gl" + typeof(T).Name;
        IntPtr ptr = SDL_GL_GetProcAddress(method);
        if (ptr == IntPtr.Zero)
        {
            throw new Exception($"nogo: {method} from {typeof(T).Name}");
        }

        return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;
    }

    /// <summary>
    ///     Alternate delegate fetcher for when our delegate Type ends in "Del". These happen when the method needs a wrapper
    ///     in GL.Utils.
    /// </summary>
    private static T _Del<T>() where T : class
    {
        string method = "gl" + typeof(T).Name.Substring(0, typeof(T).Name.Length - 3);
        IntPtr ptr = SDL_GL_GetProcAddress(method);
        if (ptr == IntPtr.Zero)
        {
            throw new Exception($"nogo: {method} from {typeof(T).Name}");
        }

        return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;
    }

    public static unsafe string glGetString(StringName pname) => new((sbyte*)_GetString(pname));


    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate IntPtr GetString(StringName pname);
}
