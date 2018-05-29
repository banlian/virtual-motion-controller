using System;
using zzz.driver.baseCard;
using zzz.driver.gtsCard.Googol;

namespace zzz.driver.gtsCard
{
    [Di(32), Do(32), Axis(4)]
    public class GTS400 : BaseCard, ICardAxisCard
    {
        public int AxisCount { get; protected set; }
        public double[] Acc { get; set; }
        public double[] Dec { get; set; }


        public GTS400()
        {
            AxisCount = 4;
            Acc = new double[4] { 1000000, 1000000, 1000000, 1000000 };
            Dec = new double[4] { 1000000, 1000000, 1000000, 1000000 };
        }

        public override int Initialize(int id, params object[] configFileName)
        {
            if (configFileName == null || configFileName.Length < 1)
            {
                return -1;
            }

            Id = id;
            Name = GetType().Name;

            var ret = mc.GT_Open((short)id, 0, 1);
            if (ret != 0)
            {
                return -1;
            }
            ret = mc.GT_Reset((short)id);
            if (ret != 0)
            {
                return -1;
            }
            ret = mc.GT_ClrSts((short)id, 1, (short)AxisCount);
            if (ret != 0)
            {
                return -1;
            }
            ret = mc.GT_LoadConfig((short)id, configFileName[0].ToString());
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }

        public override int Terminate(int cardNum)
        {
            var ret = mc.GT_Close((short)cardNum);
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }

        public override int LoadParams(string path)
        {
            var ret = mc.GT_LoadConfig(0, "GTS400.cfg");
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }

        public int ClearAxisStatus(int id, int axis)
        {
            return mc.GT_ClrSts((short)id, (short)axis, 1);
        }


        public int GetAxisStatus(int id, int axis, out int sts)
        {
            sts = 0;

            int pSts, homeSts;
            uint clock;
            var ret = mc.GT_GetSts((short)id, (short)axis, out pSts, 1, out clock);
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_GetDi((short)id, mc.MC_HOME, out homeSts);
            if (ret != 0)
            {
                return -1;
            }

            BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_ALM, BitOperateHelper.GetBit(pSts, (int)GtsAxisDefine.ALARM));
            BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_PEL, BitOperateHelper.GetBit(pSts, (int)GtsAxisDefine.LMTP));
            BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_MEL, BitOperateHelper.GetBit(pSts, (int)GtsAxisDefine.LMTN));
            BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_ORG, BitOperateHelper.GetBit(homeSts, 0x01 << axis - 1));
            BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_EMG, BitOperateHelper.GetBit(pSts, (int)GtsAxisDefine.EmgStop));

            BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_SVON, BitOperateHelper.GetBit(pSts, (int)GtsAxisDefine.ENABLE));
            BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_STS_MDN, BitOperateHelper.GetBit(pSts, (int)GtsAxisDefine.PRFMOVEING));
            BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_STS_ASTP, BitOperateHelper.GetBit(pSts, (int)GtsAxisDefine.EmgStop));

            return ret;
        }

        public override int Update(int id)
        {
            //read di sts
            {
                int value = 0;
                var ret = mc.GT_GetDi((short)id, mc.MC_GPI, out value);
                if (ret != 0)
                {
                    return -1;
                }

                for (int i = 0; i < Di.Length; i++)
                {
                    Di[i] = (value & (0x1 << (short)i)) == 0 ? 1 : 0;
                }
            }

            //read do sts
            {
                int value = 0;
                var ret = mc.GT_GetDo((short)id, mc.MC_GPO, out value);
                if (ret != 0)
                {
                    return -1;
                }

                for (int i = 0; i < Do.Length; i++)
                {
                    Do[i] = (value & (0x1 << (short)i)) == 0 ? 1 : 0;
                }
            }

            //read axis sts
            {
                for (int i = 0; i < AxisStatus.Length; i++)
                {
                    int sts;
                    GetAxisStatus(id, i, out sts);
                    AxisStatus[i].SetAxisStatus(sts);
                }
            }

            return 0;
        }

        #region motion

        public int ClearAlarm(int id, int axis)
        {
            var ret = mc.GT_SetDoBit((short)id, mc.MC_CLEAR, (short)axis, 1);
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_SetDoBit((short)id, mc.MC_CLEAR, (short)axis, 0);
            if (ret != 0)
            {
                return -1;
            }

            int value;
            uint clock;
            ret = mc.GT_GetSts((short)id, (short)axis, out value, 1, out clock);
            if (ret != 0)
            {
                return -1;
            }
            if ((value & (int)GtsAxisDefine.ALARM) == (int)GtsAxisDefine.ALARM)
            {
                return -1;
            }
            return ret;
        }

        public int SetServo(int id, int axis, bool sts)
        {
            return sts ? mc.GT_AxisOn((short)id, (short)axis) : mc.GT_AxisOff((short)id, (short)axis);
        }

        public int SetEncPos(int id, int axis, int encpos)
        {
            var ret = mc.GT_SetEncPos((short)id, (short)axis, encpos);
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }

        public int GetEncPos(int id, int axis, out int pos)
        {
            pos = 0;

            double value;
            uint clock;

            var ret = mc.GT_GetEncPos((short)id, (short)axis, out value, 1, out clock);
            if (ret != 0)
            {
                return -1;
            }

            pos = (int)value;
            return ret;
        }

        public int SetCommandPos(int id, int axis, int pos)
        {
            var ret = mc.GT_SetPrfPos((short)id, (short)axis, pos);
            if (ret != 0)
            {
                return -1;
            }
            return ret;
        }

        public int GetCommandPos(int id, int axis, out int pos)
        {
            double value;
            uint clock;

            var ret = mc.GT_GetPrfPos((short)id, (short)axis, out value, 1, out clock);
            if (ret != 0)
            {
                pos = 0;
                return -1;
            }
            pos = (int)value;
            return ret;
        }

        public int ZeroPos(int id, int axis)
        {
            int ret = mc.GT_ZeroPos((short)id, (short)axis, 1);
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }

        public int SetAxisBand(int id, int axis, int band, int us)
        {
            int ret = mc.GT_SetAxisBand((short)id, (short)axis, band, us);
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }


        public int SetMoveParams(int id, int axis, double acc, double dec, double sacc = 0, double sdec = 0)
        {
            this.Acc[axis - 1] = acc;
            this.Dec[axis - 1] = dec;

            return 0;
        }

        public int AbsMove(int id, int axis, int pos, int vel)
        {
            ClearAxisStatus(id, axis);

            var ret = mc.GT_PrfTrap((short)id, (short)axis);
            if (ret != 0)
            {
                return -1;
            }

            var trapPrm = new mc.TTrapPrm
            {
                acc = Acc[axis - 1] / 1000000, //mm/s^2
                dec = Dec[axis - 1] / 1000000, //mm/s^2
                smoothTime = 10
            };

            ret = mc.GT_SetTrapPrm((short)id, (short)axis, ref trapPrm);
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_SetVel((short)id, (short)axis, (double)vel / 1000);
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_SetPos((short)id, (short)axis, pos);
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_Update((short)id, 1 << (axis - 1));
            if (ret != 0)
            {
                return -1;
            }

            return 0;
        }

        public int RelMove(int id, int axis, int step, int vel)
        {
            ClearAxisStatus(id, axis);

            int encPos;
            var ret = GetCommandPos((short)id, (short)axis, out encPos);
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_PrfTrap((short)id, (short)axis);
            if (ret != 0)
            {
                return -1;
            }

            var trapPrm = new mc.TTrapPrm
            {
                acc = Acc[axis - 1] / 1000000, //mm/s^2
                dec = Dec[axis - 1] / 1000000, //mm/s^2
                smoothTime = 10
            };

            ret = mc.GT_SetTrapPrm((short)id, (short)axis, ref trapPrm);
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_SetVel((short)id, (short)axis, (double)vel / 1000);
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_SetPos((short)id, (short)axis, Convert.ToInt32(step + encPos));
            if (ret != 0)
            {
                return -1;
            }

            ret = mc.GT_Update((short)id, 1 << (axis - 1));
            if (ret != 0)
            {
                return -1;
            }

            return 0;
        }

        public int StopAxis(int id, int axis)
        {
            return mc.GT_Stop((short)id, 1 << (axis - 1), 0);
        }

        public int EmgStop(int id, int axis)
        {
            return mc.GT_Stop((short)id, 1 << (axis - 1), 0xff);
        }

        #endregion

        #region home

        public int HomeMove(int id, int axis, int vel)
        {
            return -1;
        }

        public int SetHomeCapture(int id, int axis)
        {
            var ret = mc.GT_SetCaptureMode((short)id, (short)axis, mc.CAPTURE_HOME);
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }

        public int GetHomeCapture(int id, int axis, out int captureSts, out int capturePos)
        {
            short pSts;
            int pValue;
            uint pclk;

            var ret = mc.GT_GetCaptureStatus((short)id, (short)axis, out pSts, out pValue, 1, out pclk);
            if (ret != 0)
            {
                captureSts = 0;
                capturePos = 0;
                return -1;
            }
            captureSts = pSts;
            capturePos = pValue;
            return 0;
        }

        public int SetIndexCapture(int id, int axis)
        {
            var ret = mc.GT_SetCaptureMode((short)id, (short)axis, mc.CAPTURE_INDEX);
            if (ret != 0)
            {
                return -1;
            }
            return 0;
        }

        public int GetIndexCapture(int id, int axis, out int captureSts, out int capturePos)
        {
            short pSts;
            int pValue;
            uint pclk;

            var ret = mc.GT_GetCaptureStatus((short)id, (short)axis, out pSts, out pValue, 1, out pclk);
            if (ret != 0)
            {
                captureSts = 0;
                capturePos = 0;
                return -1;
            }
            captureSts = pSts;
            capturePos = pValue;
            return 0;
        }

        #endregion

        #region di/do

        public int SetDi(int id, int index, int status)
        {
            return 0;
        }

        public int GetDi(int id, int index, out int status)
        {
            status = 0;
            int value;

            var ret = mc.GT_GetDi((short)id, mc.MC_GPI, out value);
            if (ret != 0)
            {
                return -1;
            }

            status = (value & (0x1 << (short)index)) == 0 ? 1 : 0;
            return ret;
        }

        public int SetDo(int id, int index, int status)
        {
            status = 1 - status;

            var ret = mc.GT_SetDoBit((short)id, mc.MC_GPO, (short)(index + 1), (short)status);
            if (ret != 0)
            {
                return -1;
            }
            return ret;
        }

        public int GetDo(int id, int index, out int status)
        {
            status = 0;
            int pValue;

            var ret = mc.GT_GetDo((short)id, mc.MC_GPO, out pValue);
            if (ret != 0)
            {
                return -1;
            }

            status = (pValue & (0x01 << (short)index)) == 0 ? 1 : 0;
            return pValue;
        }


        #endregion
    }
}