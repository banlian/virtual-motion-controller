namespace zzz.driver.baseCard
{
    public interface ICardGpioCard : ICard
    {
        int[] Di { get; }
        int[] Do { get; }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int SetDi(int id, int index, int status);

        /// <summary>
        ///     读DI
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="index">DI 索引</param>
        /// <param name="status">DI 状态</param>
        /// <returns></returns>
        int GetDi(int id, int index, out int status);

        /// <summary>
        ///     设置Do
        /// </summary>
        /// <param name="id">卡号</param>
        /// <param name="index">Do 索引</param>
        /// <param name="status">Do 状态</param>
        /// <returns></returns>
        int SetDo(int id, int index, int status);

        /// <summary>
        ///     读Do状态
        /// </summary>
        /// <param name="id">卡 号</param>
        /// <param name="index">Do 索引</param>
        /// <param name="status">Do 状态</param>
        /// <returns></returns>
        int GetDo(int id, int index, out int status);
        
    }
}