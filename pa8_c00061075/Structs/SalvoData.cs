
using ZeroFormatter;

namespace pa8_c00061075.Structs
{
    [ZeroFormattable]
    public class SalvoData
    {

        [Index(0)]
        public virtual string Message { get; set; }

        [Index(1)]
        public virtual bool IsAttack { get; set; }

        [Index(2)]
        public virtual SalvoAttack Attack { get; set; }

        [Index(3)]
        public virtual SalvoAttackResult AttackResult { get; set; }

        [Index(4)]
        public virtual bool IsAttackResult { get; set; }

        [Index(5)]
        public virtual bool IsWinResult { get; set; }

        [Index(6)]
        public virtual SalvoWinResult SalvoWinResult { get; set; }
    }
}
