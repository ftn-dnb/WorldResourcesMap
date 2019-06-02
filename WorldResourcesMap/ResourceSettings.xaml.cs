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
    /// Interaction logic for ResourceSettings.xaml
    /// </summary>
    public partial class ResourceSettings : Window, INotifyPropertyChanged
    {
        private DataManager manager;

        private string selected_id;


        public ResourceSettings(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this.manager;

            View = CollectionViewSource.GetDefaultView(this.manager.MapData.Resources);
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
                Resource resource = dgrMain.SelectedItem as Resource;
                this.selected_id = resource.Id.ToString();
                txtBoxId.Text = resource.Id.ToString();
                txtBoxName.Text = resource.Name;
                txtBoxDescription.Text = resource.Description;
                resFrequency.Text = resource.Frequency;
                resUnit.Text = resource.UnitOfMeasure;
                resMap.Text = resource.MapID.ToString();
                resPrice.Text = resource.Price.ToString();
                resDateFound.SelectedDate = resource.DiscoveryDate;
                resRenewable.IsChecked = resource.Renewable;
                resStrategicImportance.IsChecked = resource.StrategicImportance;
                resExploatation.IsChecked = resource.Exploitation;
                resImage.Source = new BitmapImage(new Uri(resource.Icon));
            }
            catch(Exception ex)
            {

            }
        }

        private void EtiquettePicker(object sender, RoutedEventArgs e)
        {
            Resource resource = dgrMain.SelectedItem as Resource;
            var form = new EtiquettePicker(manager, resource);
            form.ShowDialog();
        }
    }
}
