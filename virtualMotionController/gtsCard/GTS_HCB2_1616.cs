using zzz.driver.gtsCard.Googol;
using zzz.framework.baseCard;

namespace zzz.driver.gtsCard
{
    [Di(16), Do(16)]
    public class GTS_HCB2_1616 : BaseCard, ICardGpioCard
    {
        private static bool _isInit;

        private int _id;
        private int _extid;

        /// <summary>
        ///     初始化卡：
        ///     所有的扩展模块都只需要初始化一次。
        ///     卡号默认为0，将传递的卡号作为Mdl用，Mdl在配置文件中。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public override int Initialize(int id, params object[] configFileName)
        {
            if (configFileName == null || configFileName.Length < 2)
            {
                return -1;
            }

            _id = id;
            _extid = int.Parse(configFileName[1].ToString());

            if (!_isInit)
            {
                int ret = mc.GT_OpenExtMdl((short)_id, "gts.dll");
                if (ret != 0)
                {
                    return -1;
                }

                ret = mc.GT_LoadExtConfig((short)_id, configFileName[0].ToString());
                if (ret != 0)
                {
                    return -1;
                }

                _isInit = true;
            }

            return 0;
        }

        public override int Terminate(int cardNum)
        {
            if (_isInit)
            {
                int ret = mc.GT_CloseExtMdl((short)_id);
                if (ret != 0)
                {
                    return -1;
                }
                _isInit = false;
            }
            return 0;
        }

        public override int Update(int id)
        {
            return 0;
        }


        #region di do
        public int SetDi(int id, int index, int status)
        {
            return -1;
        }

        public int GetDi(int id, int index, out int status)
        {
            status = 0;
            ushort sts = 0;
            int ret = mc.GT_GetExtIoValue((short)_id, (short)_extid, ref sts);
            if (ret != 0)
            {
                return -1;
            }

            status = (sts & (1 << (short)index)) == 0 ? 1 : 0;
            return 0;
        }

        public int SetDo(int id, int index, int status)
        {
            status = 1 - status;
            int ret = mc.GT_SetExtIoBit((short)_id, (short)_extid, (short)index, (ushort)status);
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }

        public int GetDo(int id, int index, out int status)
        {
            status = 0;
            ushort sts = 0;
            int ret = mc.GT_GetExtDoValue((short)_id, (short)_extid, ref sts);
            if (ret != 0)
            {
                return -1;
            }

            status = sts & (1 << index);
            status = 1 - status;
            return 0;
        }

        #endregion
    }
}