using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResourcesMap
{
    public class MapData
    {
        public List<ResourceType> Types { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Etiquette> Etiquettes { get; set; }

        public MapData()
        {
            Types = new List<ResourceType>();
            Resources = new List<Resource>();
            Etiquettes = new List<Etiquette>();
        }
    }
}
