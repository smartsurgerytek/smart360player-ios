public static class Mathd
{
    public static double Clamp(double value, double min, double max)
    {
        if (min > max)
        {
            var temp = min;
            min = max;
            max = temp;
        }
        if (value < min)
        {
            value = min;
        }
        if (value > max)
        {
            value = max;
        }
        return value;
    }
}
