namespace zzz.driver.baseCard
{
    public class BitOperateHelper
    {
        //public static bool IsBitSet(int word, int bit)
        //{
        //    return (word & bit) != 0;
        //}

        //public static bool IsBitReset(int word, int bit)
        //{
        //    return (word & bit) == 0;
        //}

        public static bool GetBit(int sts, int bit)
        {
            return (sts & bit) != 0;
        }

        public static void SetBit(ref int sts, int bit, bool high = true)
        {
            if (high)
            {
                sts |= bit;
            }
            else
            {
                sts &= ~bit;
            }
        }

        public static void ResetBit(ref int sts, int bits)
        {
            sts &= ~bits;
        }
    }
}