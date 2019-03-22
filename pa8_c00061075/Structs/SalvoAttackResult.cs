using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace pa8_c00061075.Structs
{
    [ZeroFormattable]
    public class SalvoAttackResult
    {
        [Index(0)]
        public virtual bool WasHit { get; set; }


    }
}
