using static SDL2.SDL;

namespace JustConveyors.Source.Loop;

internal static class Time
{
    public static double DeltaTime { get; private set; }
    public static double TotalElapsedSeconds { get; private set; }
    public static float DeltaTimeF => (float)DeltaTime;
    public static float TotalElapsedSecondsF => (float)TotalElapsedSeconds;

    public static void NextFrame()
    {
        double seconds = SDL_GetTicks() / 1000d;
        DeltaTime = seconds - TotalElapsedSeconds;
        TotalElapsedSeconds = seconds;
    }
}
