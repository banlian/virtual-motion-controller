using System;

namespace zzz.driver.baseCard
{
    public class DiAttribute : Attribute
    {
        public int Count { get; }

        public DiAttribute(int count)
        {
            Count = count;
        }
    }
}
