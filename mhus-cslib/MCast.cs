using System;
namespace mhuscslib
{
    public class MCast
    {
        internal static string ToHex2LowerString(int v)
        {
            return v.ToString("X2").ToLower();
        }

        internal static int ToIntFromHex(string v)
        {
            return int.Parse(v, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
