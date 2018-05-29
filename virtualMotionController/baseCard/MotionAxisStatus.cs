using System;

namespace zzz.driver.baseCard
{
    /// <summary>
    /// axis motion params during move 
    /// </summary>
    public struct MotionAxisStatus
    {
        //motion sts
        public bool Hmv { get; set; }
        public bool Astp { get; set; }
        public bool Mdn { get; set; }
        public bool Alarm { get; set; }

        public bool Servo { get; set; }
        public bool Mel { get; set; }
        public bool Pel { get; set; }
        public bool Org { get; set; }

        //motion

        public bool IsMove { get; set; }
        public int EncoderPos { get; set; }
        public int CommandPos { get; set; }
        public int Vel { get; set; }
        public int Acc { get; set; }


        //home/index capture
        public bool IsCaptureHome { get; set; }
        public bool IsCaptureIndex { get; set; }
        public int CaptureStatus { get; set; }
        public int CapturePos { get; set; }


        //axis default define
        public const int PelLim = 20000;
        public const int MelLim = -320;
        public const int OrgLim = 0;
        public const int IndexLim = 100;


        /// <summary>
        ///     update at each 0.1s
        /// </summary>
        public void Update()
        {
            //is moving
            if (IsMove && !Mdn)
            {
                //update pos
                if (Math.Abs(Vel) < 10)
                {
                    Vel = 10;
                }

                EncoderPos = EncoderPos + (int)(Vel * 0.1);

                //mel 
                if (EncoderPos <= MelLim)
                {
                    EncoderPos = MelLim;
                    CommandPos = MelLim;
                    Mdn = true;
                    Mel = true;
                }
                else
                {
                    Mel = false;
                }

                //pel
                if (EncoderPos >= PelLim)
                {
                    EncoderPos = PelLim;
                    CommandPos = PelLim;
                    Mdn = true;
                    Pel = true;
                }
                else
                {
                    Pel = false;
                }


                //org
                if (Math.Abs(EncoderPos - OrgLim) <= (Math.Abs(Vel * 0.11) + 0.1))
                {
                    Org = true;
                    if (IsCaptureHome || IsCaptureIndex)
                    {
                        Mdn = true;
                    }
                }
                else
                {
                    Org = false;
                }

                //check move finish
                if (Math.Abs(EncoderPos - CommandPos) <= (Math.Abs(Vel * 0.11) + 0.1))
                {
                    EncoderPos = CommandPos;
                    Mdn = true;
                    IsMove = false;
                    return;
                }
            }
        }


        public void SetAxisStatus(int sts)
        {
            Alarm = BitOperateHelper.GetBit(sts, MotionAxisDefine.MOTION_IO_ALM);
            Pel = BitOperateHelper.GetBit(sts, MotionAxisDefine.MOTION_IO_PEL);
            Mel = BitOperateHelper.GetBit(sts, MotionAxisDefine.MOTION_IO_MEL);
            Org = BitOperateHelper.GetBit(sts, MotionAxisDefine.MOTION_IO_ORG);
            Servo = BitOperateHelper.GetBit(sts, MotionAxisDefine.MOTION_IO_SVON);

            Astp = BitOperateHelper.GetBit(sts, MotionAxisDefine.MOTION_STS_ASTP);
            Hmv = BitOperateHelper.GetBit(sts, MotionAxisDefine.MOTION_STS_HMV);
            Mdn = BitOperateHelper.GetBit(sts, MotionAxisDefine.MOTION_STS_MDN);
        }

    }
}