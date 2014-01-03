using System.Collections.Generic;

namespace OHHelper
{
    public class Anime
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public List<Ep> Eps { get; set; }
    }
}