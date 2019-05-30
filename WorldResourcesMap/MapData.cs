using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WorldResourcesMap
{
    public class MapData
    {
        public ObservableCollection<ResourceType> Types { get; set; }
        public ObservableCollection<Resource> Resources { get; set; }
        public ObservableCollection<Etiquette> Etiquettes { get; set; }

        public MapData()
        {
            Types = new ObservableCollection<ResourceType>();
            Resources = new ObservableCollection<Resource>();
            Etiquettes = new ObservableCollection<Etiquette>();
        }
    }
}
