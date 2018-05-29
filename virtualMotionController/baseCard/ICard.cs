namespace zzz.driver.baseCard
{
    public interface ICard
    {
        /// <summary>
        ///     初始化运动控制卡
        /// </summary>
        /// <param name="id"></param>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        int Initialize(int id, params object[] configFileName);

        /// <summary>
        ///     关闭运动控制卡
        /// </summary>
        /// <param name="cardNum">卡号</param>
        /// <returns></returns>
        int Terminate(int id);

        int LoadParams(string path);

        int Update(int id);
    }
}