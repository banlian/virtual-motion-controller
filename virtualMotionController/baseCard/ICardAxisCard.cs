namespace zzz.driver.baseCard
{
    /// <summary>
    ///     运动控制卡驱动接口
    /// </summary>
    public interface ICardAxisCard : ICardGpioCard
    {
        MotionAxisStatus[] AxisStatus { get; }

        #region motion

        /// <summary>
        ///     清除轴状态
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        int ClearAxisStatus(int id, int axis);


        /// <summary>
        ///     获取运动状态
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="sts">轴状态位</param>
        /// <returns></returns>
        int GetAxisStatus(int id, int axis, out int sts);


        /// <summary>
        ///     清除轴的伺报警
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
        int ClearAlarm(int id, int axis);

        /// <summary>
        ///     伺服使能
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="enable">使能状态：1使能</param>
        /// <returns></returns>
        int SetServo(int id, int axis, bool enable);

    

        /// <summary>
        ///     设置编码器的值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="axis"></param>
        /// <param name="encpos"></param>
        /// <returns></returns>
        int SetEncPos(int id, int axis, int encpos);

        /// <summary>
        ///     获取编码器的值
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="pos">编码器的值</param>
        /// <returns></returns>
        int GetEncPos(int id, int axis, out int pos);

        /// <summary>
        ///     设定规划位置
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="pos">设定规划位置值</param>
        /// <returns></returns>
        int SetCommandPos(int id, int axis, int pos);

        /// <summary>
        ///     获取规划位置
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="pos">获取规划位置值</param>
        /// <returns></returns>
        int GetCommandPos(int id, int axis, out int pos);


        /// <summary>
        ///     设定编码器零点
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
        int ZeroPos(int id, int axis);

        /// <summary>
        ///     设定轴的误差带
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="band">误差带</param>
        /// <param name="us">稳定时间</param>
        /// <returns></returns>
        int SetAxisBand(int id, int axis, int band, int us);

        /// <summary>
        ///     设定轴的加减速度
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="acc">加速度</param>
        /// <param name="dec">减速度</param>
        /// <returns></returns>
        int SetMoveParams(int id, int axis, double acc, double dec, double sacc, double sdec);

        /// <summary>
        ///     绝对运动
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="pos">绝对位置</param>
        /// <param name="vel">速度</param>
        /// <returns></returns>
        int AbsMove(int id, int axis, int pos, int vel);

        /// <summary>
        ///     相对运动
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="step">相对位置量</param>
        /// <param name="vel">速度</param>
        /// <returns></returns>
        int RelMove(int id, int axis, int step, int vel);

        /// <summary>
        ///     平滑停止
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
        int StopAxis(int id, int axis);

        /// <summary>
        ///     急停
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
        int EmgStop(int id, int axis);

        #endregion

        #region home

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="axis"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        int HomeMove(int id, int axis, int vel);

        /// <summary>
        ///     设定为原点补获模式
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
        int SetHomeCapture(int id, int axis);

        /// <summary>
        ///     获取编码器Index捕获值
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="captureSts">捕获状态</param>
        /// <param name="capturePos">捕获位置值</param>
        /// <returns></returns>
        int GetHomeCapture(int id, int axis, out int captureSts, out int capturePos);


        /// <summary>
        ///     设定为编码器捕获模式
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
        int SetIndexCapture(int id, int axis);

        /// <summary>
        ///     获取原点信号捕获值
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="captureSts">捕获状态</param>
        /// <param name="capturePos">捕获值</param>
        /// <returns></returns>
        int GetIndexCapture(int id, int axis, out int captureSts, out int capturePos);

        #endregion
    }
}