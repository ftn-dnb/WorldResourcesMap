using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResourcesMap
{
    public class ResourceType : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int id;
        private string name;
        private string icon;
        private string description;

        public int Id
        {
            get { return id; }
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

        public string Icon
        {
            get { return icon; }
            set
            {
                if (value != icon)
                {
                    icon = value;
                    OnPropertyChanged("icon");
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("description");
                }
            }
        }

        public ResourceType()
        {
        }

        public ResourceType(int id, string name, string icon, string description)
        {
            this.id = id;
            this.name = name;
            this.icon = icon;
            this.description = description;
        }
    }
}
