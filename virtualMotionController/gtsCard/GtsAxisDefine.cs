namespace zzz.driver.gtsCard
{
    internal enum GtsAxisDefine
    {
        ALARM = 0x0002,
        ERRORBEYOND = 0x0010,

        LMTP = 0x0020,
        LMTN = 0x0040,

        SMOOTHSTOP = 0x0080,
        EmgStop = 0x0100,

        ENABLE = 0x0200,

        PRFMOVEING = 0x0400,
        PRFDONE = 0x0800
    }
}