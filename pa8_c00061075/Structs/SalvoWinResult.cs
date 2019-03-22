
using ZeroFormatter;

namespace pa8_c00061075.Structs
{
    [ZeroFormattable]
    public class SalvoWinResult
    {
        [Index(0)]
        public virtual bool DidWin { get; set; }
    }

}
