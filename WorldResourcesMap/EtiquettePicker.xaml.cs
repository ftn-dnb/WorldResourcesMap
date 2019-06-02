using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace WorldResourcesMap
{
    /// <summary>
    /// Interaction logic for EtiquettePicker.xaml
    /// </summary>
    public partial class EtiquettePicker : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DataManager manager;
        private Resource resource;
        public DataManager DataManager {
            get
            {
                return this.manager;
            } 
            set
            {
                this.manager = value;
            }
        }
        public Resource Resource {
            get
            {
                return this.resource;
            }
            set
            {
                this.resource = value;
                if (value != resource)
                {
                    resource = value;
                    OnPropertyChanged("Resource");
                }
            }
        }
        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        public EtiquettePicker(DataManager manager, Resource resource)
        {
            InitializeComponent();
            this.DataContext = this;
            this.manager = manager;
            this.resource = resource;
            this.dgrMain1.ItemsSource = resource.Tags;
        }
        
        private void AddEtiquette(object sender, RoutedEventArgs e)
        {
            Etiquette etiquette = dgrMain.SelectedItem as Etiquette;
            var filtered = Resource.Tags.Where(et => et.Id == etiquette.Id);
            if (filtered.ToList().Count != 0)
            {
                MessageBox.Show("Odabrana etiketa sa id " + etiquette.Id + " već postoji u listi.",
                "Upozorenje o dodavanju", MessageBoxButton.OK,
                MessageBoxImage.Warning);
                return;
            }
            Resource.Tags.Add(etiquette);
            dgrMain1.ItemsSource = Resource.Tags;
            DataManager.SaveResources();
        }

        private void RemoveEtiquette(object sender, RoutedEventArgs e)
        {
            Etiquette etiquette = dgrMain1.SelectedItem as Etiquette;
            Resource.Tags.Remove(etiquette);
        }
    }
}
