using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pa8_c00061075.Models;
using ZeroFormatter;

namespace pa8_c00061075.Structs
{
    [ZeroFormattable]
    public class SalvoAttack
    {
        [Index(0)]
        public virtual int X { get; set; }

        [Index(1)]
        public virtual int Y { get; set; }
    }
}
