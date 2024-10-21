

public static class StringUtils
{
    private static string[] moneySuffixes = { "", "K", "M", "B", "T", "Q" };

    public static string MoneyFormatting(long value)
    {
        double tempMoney = value;

        // Define the size suffixes for thousand, million, billion, trillion, etc.
        int suffixIndex = 0;

        // Keep dividing money by 1000 until it is less than 1000, tracking the suffix
        while (tempMoney >= 1000 && suffixIndex < moneySuffixes.Length - 1)
        {
            tempMoney /= 1000;
            suffixIndex++;
        }

        // Format the number with the appropriate suffix and return
        return tempMoney.ToString("0.##") + moneySuffixes[suffixIndex];
    }
}
