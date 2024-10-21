public static class StringUtils
{
    private static string[] suffixes = { "", "K", "M", "B", "T", "Q" };

    public static string FormatNumber(long value)
    {
        double tmp = value;

        // Define the size suffixes for thousand, million, billion, trillion, etc.
        int suffixIndex = 0;

        // Keep dividing money by 1000 until it is less than 1000, tracking the suffix
        while (tmp >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            tmp /= 1000;
            suffixIndex++;
        }

        // Format the number with the appropriate suffix and return
        return tmp.ToString("0.##") + suffixes[suffixIndex];
    }
}
