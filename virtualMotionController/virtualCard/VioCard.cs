using zzz.driver.baseCard;

namespace zzz.driver.virtualCard
{
    /// <summary>
    /// virtual io signal driver support 128bit signals
    /// </summary>
    [Di(128), Do(128)]
    public class VioCard : BaseCard, ICardGpioCard
    {
        public override int Initialize(int id, params object[] configFileName)
        {
            lock (this)
            {
                Id = id;
                Name = GetType().Name;

                for (var i = 0; i < Di.Length; i++)
                {
                    Di[i] = 0;
                }

                for (var i = 0; i < Do.Length; i++)
                {
                    Do[i] = 0;
                }

                return 0;
            }
        }

        public override int Terminate(int id)
        {
            lock (this)
            {
                for (var i = 0; i < Di.Length; i++)
                {
                    Di[i] = 0;
                }

                for (var i = 0; i < Do.Length; i++)
                {
                    Do[i] = 0;
                }

                return 0;
            }
        }
        

        public int SetDi(int id, int index, int status)
        {
            lock (this)
            {
                Di[index] = status;
                return 0;
            }
        }

        public int GetDi(int id, int index, out int status)
        {
            lock (this)
            {
                status = Di[index];
                return 0;
            }
        }

        public int SetDo(int id, int index, int status)
        {
            lock (this)
            {
                Do[index] = status;
                return 0;
            }
        }

        public int GetDo(int id, int index, out int status)
        {
            lock (this)
            {
                status = Do[index];
                return 0;
            }
        }
        
    }
    
}