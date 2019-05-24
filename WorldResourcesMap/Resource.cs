using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResourcesMap
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ResourceType Type { get; set; }
        public string Frequency { get; set; } // @TODO mozda staviti kao enum ?
        public string Icon { get; set; } // ovo ostaviti kao putanju do ikonice ?
        public bool Renewable { get; set; }
        public bool StrategicImportance { get; set; }
        public bool Exploitation { get; set; }
        public string UnitOfMeasure { get; set; }
        public float Price { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public List<Tag> tags { get; set; }

        public Resource()
        {
        }


    }
}
