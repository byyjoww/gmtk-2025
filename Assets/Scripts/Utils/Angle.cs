namespace GMTK2025.Utils
{
    public static class Angle
    {
        public static float FromValue(float value)
        {
            if (value < 0) { value += 360; }
            return value % 360;
        }
    }
}
