using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResourcesMap
{
    public class ResourceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; } // ostaviti kao putanju ?
        public string Description { get; set; }
    }
}
