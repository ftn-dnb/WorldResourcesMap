using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResourcesMap
{
    public class Resource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string name;
        private int id;
        private string description;
        private ResourceType type;
        private string frequency;
        private string icon;
        private bool renewable;
        private bool strategicImportance;
        private bool exploitation;
        private string unitOfMeasure;
        private float price;
        private DateTime discoveryDate;
        private List<Etiquette> tags;
        private int mapId;
        private int x;
        private int y;
        private bool onMap;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("id");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("name");
                }
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if(value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public ResourceType Type
        {
            get
            {
                return type;
            }
            set
            {
                if(value != type)
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }
        public string Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                if(value != frequency)
                {
                    frequency = value;
                    OnPropertyChanged("Frequency");
                }
            }
        } // @TODO mozda staviti kao enum ?
        public string Icon
        {
            get
            {
                return icon;
            }
            set
            {
                if(value != icon)
                {
                    icon = value;
                    OnPropertyChanged("Icon");
                }
            }
        } // ovo ostaviti kao putanju do ikonice ?
        public bool Renewable
        {
            get
            {
                return renewable;
            }
            set
            {
                if(value != renewable)
                {
                    renewable = value;
                    OnPropertyChanged("Renewable");
                }
            }
        }
        public bool StrategicImportance
        {
            get
            {
                return strategicImportance;
            }
            set
            {
                if (value != strategicImportance)
                {
                    strategicImportance = value;
                    OnPropertyChanged("StrategicImportance");
                }
            }
        }
        public bool Exploitation
        {
            get
            {
                return exploitation;
            }
            set
            {
                if (value != exploitation)
                {
                    exploitation = value;
                    OnPropertyChanged("Exploration");
                }
            }
        }
        public string UnitOfMeasure
        {
            get
            {
                return unitOfMeasure;
            }
            set
            {
                if (value != unitOfMeasure)
                {
                    unitOfMeasure = value;
                    OnPropertyChanged("UnitOfMeasure");
                }
            }
        }
        public float Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value != price)
                {
                    price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        public DateTime DiscoveryDate
        {
            get
            {
                return discoveryDate;
            }
            set
            {
                if (value != discoveryDate)
                {
                    discoveryDate = value;
                    OnPropertyChanged("DiscoveryDate");
                }
            }
        }
        public List<Etiquette> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                if (value != tags)
                {
                    tags = value;
                    OnPropertyChanged("Tags");
                }
            }
        }
        public int MapID
        {
            get
            {
                return mapId;
            }
            set
            {
                if (value != mapId)
                {
                    mapId = value;
                    OnPropertyChanged("MapId");
                }
            }
        }// treba staviti da bude lista jer ima 4 mape

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                if (value != x)
                {
                    x = value;
                    OnPropertyChanged("x");
                }
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value != y)
                {
                    y = value;
                    OnPropertyChanged("y");
                }
            }
        }

        public bool OnMap
        {
            get
            {
                return onMap;
            }
            set
            {
                if (value != onMap)
                {
                    onMap = value;
                    OnPropertyChanged("onMap");
                }
            }
        }

        public Resource()
        {
            onMap = false;
        }

    }
}
