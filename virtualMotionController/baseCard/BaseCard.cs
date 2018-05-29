using System.Reflection;

namespace zzz.driver.baseCard
{
    public class BaseCard : ICard
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }

        public int[] Di { get; protected set; }
        public int[] Do { get; protected set; }
        public MotionAxisStatus[] AxisStatus { get; protected set; }

        public BaseCard()
        {
            var t = GetType();
            var diAttr = t.GetCustomAttribute<DiAttribute>();
            if (diAttr != null)
            {
                Di = new int[diAttr.Count];
            }

            var doAttr = t.GetCustomAttribute<DoAttribute>();
            if (doAttr != null)
            {
                Do = new int[doAttr.Count];
            }

            var axisAttr = t.GetCustomAttribute<AxisAttribute>();
            if (axisAttr != null)
            {
                AxisStatus = new MotionAxisStatus[axisAttr.Count];
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} {Name} ID:{Id} DI:{Di?.Length} DO:{Do?.Length} AXIS:{AxisStatus?.Length}";
        }

        public virtual int Initialize(int id, params object[] configFileName)
        {
            return 0;
        }

        public virtual int Terminate(int id)
        {
            return 0;
        }

        public virtual int LoadParams(string path)
        {
            return 0;
        }

        public virtual int Update(int id)
        {
            return 0;
        }
    }
}
