namespace JustConveyors.Source.Drawing;

internal static class MapHelper
{
    public static int Map(int x, int in_min, int in_max, int out_min, int out_max) =>
        (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

    public static float Map(float x, float in_min, float in_max, float out_min, float out_max) =>
        (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;

    public static long Map(long x, long in_min, long in_max, long out_min, long out_max) =>
        (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
}
