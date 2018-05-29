using System;

namespace zzz.driver.baseCard
{
    public class DoAttribute : Attribute
    {
        public int Count { get; }

        public DoAttribute(int count)
        {
            Count = count;
        }
    }
}