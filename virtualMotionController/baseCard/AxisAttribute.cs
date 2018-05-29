using System;

namespace zzz.driver.baseCard
{
    public class AxisAttribute : Attribute
    {
        public int Count { get; }

        public AxisAttribute(int count)
        {
            Count = count;
        }
    }
}