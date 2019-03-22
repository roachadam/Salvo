
using ZeroFormatter;

namespace pa8_c00061075.Models
{
    [ZeroFormattable]
    public class Coordinates
    {
        [Index(0)]
        public virtual int X { get; set; }

        [Index(1)]
        public virtual int Y { get; set; }

        [Index(2)]
        public virtual bool IsHit { get; set; }

        public Coordinates(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override bool Equals(object obj)
        {
            Coordinates comp = obj as Coordinates;
            return this.X == comp.X && this.Y == comp.Y;
        }
    }
}
