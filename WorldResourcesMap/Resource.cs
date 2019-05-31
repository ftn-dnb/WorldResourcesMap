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
        public List<Etiquette> tags { get; set; }
        public int MapID { get; set; }// treba staviti da bude lista jer ima 4 mape

        public Resource()
        {
        }


    }
}
