using zzz.driver.baseCard;

namespace zzz.driver.gtsCard
{
    [Di(32), Do(32), Axis(8)]
    public class GTS800 : GTS400
    {
        public GTS800()
        {
            AxisCount = 8;
            Acc = new double[8] { 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000 };
            Dec = new double[8] { 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000, 1000000 };
        }
    }
}