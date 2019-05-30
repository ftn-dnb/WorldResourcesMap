using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;

namespace WorldResourcesMap
{
    public class Etiquette : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int id;
        private Brush color;
        private string description;

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
                    OnPropertyChanged("Id");
                }
            }
        }
        public Brush Color
        {
            get
            {
                return color;
            }
            set
            {
                if (value != color)
                {
                    color = value;
                    OnPropertyChanged("Color");
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
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public Etiquette()
        {
        }

        public Etiquette(int id, Brush color, string description)
        {
            this.id = id;
            this.color = color;
            this.description = description;
        }
    }
}
