using System.Collections.Generic;

namespace pa8_c00061075.Models
{
    public class Location
    {
        public List<Coordinates> Coordinates;

        public Location(List<Coordinates> coords)
        {
            Coordinates = coords;
        }
    }
}
