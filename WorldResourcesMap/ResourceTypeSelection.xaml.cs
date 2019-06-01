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
    /// Interaction logic for ResourceTypeSelection.xaml
    /// </summary>
    public partial class ResourceTypeSelection : Window, INotifyPropertyChanged
    {

        private DataManager manager;
        private Resource resource;
        public ResourceTypeSelection(DataManager manager, Resource resource)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this.manager;
            this.resource = resource;

            View = CollectionViewSource.GetDefaultView(this.manager.MapData.Types);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private ICollectionView _View;

        public ICollectionView View
        {
            get
            {
                return _View;

            }
            set
            {
                _View = value;
                OnPropertyChanged("View");
            }
        }

        private void ResourceSelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                ResourceType type = dgrMain.SelectedItem as ResourceType;
                resTypeImage.Source = new BitmapImage(new Uri(type.Icon));

                this.resource.Type = type;
            }
            catch(Exception ex)
            {

            }
        }

        private void SendType(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
