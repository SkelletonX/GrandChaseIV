using System;

namespace GrandChase.Utilities
{
    public static class TimeUtil
    {
        public static ushort[] GetDateTime(long ticks)
        {
            ushort[] arr = new ushort[2];

            if (ticks == -1)
            {
                arr[0] = 0xFFFF;
                arr[1] = 0;
            }
            else
            {
                DateTime dt = new DateTime(new DateTime(ticks).Ticks - new DateTime(1899, 12, 6).Ticks);

                arr[0] = (ushort)(dt.DayOfYear + (dt.Year - 1) * 365);
                arr[1] = (ushort)(dt.Minute + (dt.Hour * 60));
            }

            return arr;
        }
    }
}
