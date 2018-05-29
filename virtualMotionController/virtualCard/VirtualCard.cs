using System;
using System.Threading;
using zzz.driver.baseCard;

namespace zzz.driver.virtualCard
{
    /// <summary>
    ///     virtual card driver for motion card simulate:
    ///     - implemented by internal timer for states update
    /// </summary>
    [Di(1024), Do(1024), Axis(64)]
    public class VirtualCard : BaseCard, ICardAxisCard
    {
        private Timer _timer;

        public override int Initialize(int id, params object[] configFileName)
        {
            Id = id;
            Name = GetType().Name;
            _timer?.Dispose();
            _timer = new Timer(OnTimer, null, 0, 100);
            return 0;
        }

        public override int Terminate(int id)
        {
            _timer.Dispose();
            return 0;
        }

        public override int LoadParams(string path)
        {
            Thread.Sleep(1);
            return 0;
        }

        public override int Update(int id)
        {
            Thread.Sleep(1);
            return 0;
        }

        public int ClearAxisStatus(int id, int axis)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].Alarm = false;
                return 0;
            }
        }
  

        public int GetAxisStatus(int id, int axis, out int sts)
        {
            lock (this)
            {
                Thread.Sleep(1);
                sts = 0;


                BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_ALM, AxisStatus[axis].Alarm);
                BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_PEL, AxisStatus[axis].Pel);
                BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_MEL, AxisStatus[axis].Mel);
                BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_ORG, AxisStatus[axis].Org);
                BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_EMG, AxisStatus[axis].Astp);
                BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_IO_SVON, AxisStatus[axis].Servo);

                BitOperateHelper.SetBit(ref sts, MotionAxisDefine.MOTION_STS_MDN, AxisStatus[axis].Mdn);
            
                return 0;
            }
        }

        private void OnTimer(object state)
        {
            lock (this)
            {
                //motion simulate update
                for (int i = 0; i < AxisStatus.Length; i++)
                {
                    AxisStatus[i].Update();
                }
            }
        }

        #region motion

        public int ClearAlarm(int id, int axis)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].Alarm = false;
                return 0;
            }
        }

        public int SetServo(int id, int axis, bool sts)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].Servo = sts;
                return 0;
            }
        }

        public int SetEncPos(int id, int axis, int pos)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].EncoderPos = pos;
                return 0;
            }
        }

        public int GetEncPos(int id, int axis, out int pos)
        {
            lock (this)
            {
                Thread.Sleep(1);
                pos = AxisStatus[axis].EncoderPos;
                return 0;
            }
        }

        public int SetCommandPos(int id, int axis, int pos)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].CommandPos = pos;
                return 0;
            }
        }

        public int GetCommandPos(int id, int axis, out int pos)
        {
            lock (this)
            {
                Thread.Sleep(1);
                pos = AxisStatus[axis].CommandPos;
                return 0;
            }
        }

        public int ZeroPos(int id, int axis)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].EncoderPos = 0;
                return 0;
            }
        }

        public int SetAxisBand(int id, int axis, int band, int us)
        {
            Thread.Sleep(1);
            return 0;
        }

        public int SetMoveParams(int id, int axis, double acc, double dec, double sacc, double sdec)
        {
            Thread.Sleep(1);
            return 0;
        }

        public int AbsMove(int id, int axis, int pos, int vel)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].Mdn = false;
                AxisStatus[axis].CommandPos = pos;
                AxisStatus[axis].Vel = vel * Math.Sign(AxisStatus[axis].CommandPos - AxisStatus[axis].EncoderPos);
                AxisStatus[axis].IsMove = true;
                return 0;
            }
        }

        public int RelMove(int id, int axis, int step, int vel)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].Mdn = false;
                AxisStatus[axis].CommandPos = AxisStatus[axis].EncoderPos + step;
                AxisStatus[axis].Vel = vel * Math.Sign(step);
                AxisStatus[axis].IsMove = true;
                return 0;
            }
        }

        public int StopAxis(int id, int axis)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].IsMove = false;
                AxisStatus[axis].Mdn = true;
                return 0;
            }
        }

        public int EmgStop(int id, int axis)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].IsMove = false;
                AxisStatus[axis].Mdn = true;
                return 0;
            }
        }

        #endregion

        #region home

        public int HomeMove(int id, int axis, int vel)
        {
            return -1;
        }

        public int SetHomeCapture(int id, int axis)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].IsCaptureHome = true;
                return 0;
            }
        }

        public int GetHomeCapture(int id, int axis, out int captureSts, out int capturePos)
        {
            lock (this)
            {
                Thread.Sleep(1);
                captureSts = 0;
                capturePos = 0;
                if (AxisStatus[axis].Org)
                {
                    AxisStatus[axis].IsCaptureHome = false;
                    captureSts = 1;
                    capturePos = 0;
                }
                return 0;
            }
        }

        public int SetIndexCapture(int id, int axis)
        {
            lock (this)
            {
                Thread.Sleep(1);
                AxisStatus[axis].IsCaptureIndex = true;
                return 0;
            }
        }

        public int GetIndexCapture(int id, int axis, out int captureSts, out int capturePos)
        {
            lock (this)
            {
                Thread.Sleep(1);
                captureSts = 0;
                capturePos = 0;
                if (AxisStatus[axis].Org)
                {
                    AxisStatus[axis].IsCaptureIndex = false;
                    captureSts = 1;
                    capturePos = 0;
                }
                return 0;
            }
        }

        #endregion

        #region di do

        public int SetDi(int id, int index, int status)
        {
            Thread.Sleep(1);
            Di[index] = status;
            return 0;
        }

        public int GetDi(int id, int index, out int status)
        {
            Thread.Sleep(1);
            status = Di[index];
            return 0;
        }

        public int SetDo(int id, int index, int status)
        {
            Thread.Sleep(1);
            Do[index] = status;
            return 0;
        }

        public int GetDo(int id, int index, out int status)
        {
            Thread.Sleep(1);
            status = Do[index];
            return 0;
        }

      
        #endregion


    }
}