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
using System.Collections.ObjectModel;

namespace WorldResourcesMap
{
    /// <summary>
    /// Interaction logic for ResourceSettings.xaml
    /// </summary>
    public partial class ResourceSettings : Window, INotifyPropertyChanged
    {
        private DataManager manager;
        private ResourceType new_resource_type;
        private ObservableCollection<Etiquette> new_tags;
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

        private void keyUpSearch(object sender, RoutedEventArgs e)
        {
            this.manager.resetResourceCounter();
            var filtered = this.manager.MapData.Resources.Where(res => res.Name.ToString().StartsWith(Search.Text));
            dgrMain.ItemsSource = filtered;
        }

        private void selectionChangedSearch(object sender, RoutedEventArgs e)
        {
            this.manager.resetResourceCounter();
            var filtered = this.manager.MapData.Resources.Where(res => res.Name.ToString().StartsWith(Search.Text));
            dgrMain.ItemsSource = filtered;
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
                resType.Text = "Tip: " + resource.Type.Name;

                new_resource_type = resource.Type;
                new_tags = resource.Tags;

            }
            catch(Exception ex)
            {

            }
        }

        private void EtiquettePicker(object sender, RoutedEventArgs e)
        {
            Resource resource = dgrMain.SelectedItem as Resource;
            Resource fake_resource = new Resource();
            foreach(Etiquette tag in resource.Tags)
            {
                fake_resource.Tags.Add(tag);
            }
            var form = new EtiquettePicker(manager, fake_resource);
            form.ShowDialog();

            new_tags = fake_resource.Tags;
        }

        private void ResourceTypePicker(object sender, RoutedEventArgs e)
        {
            Resource resource = dgrMain.SelectedItem as Resource;
            Resource fake_resource = new Resource();
            fake_resource.Type = resource.Type;
            var form = new ResourceTypeSelection(manager, fake_resource);
            form.ShowDialog();

            resType.Text = "Tip: " + fake_resource.Type.Name;
            new_resource_type = fake_resource.Type;
        }

        private void ResourceEdit(object sender, RoutedEventArgs e)
        {
            //provera
            Resource resource = dgrMain.SelectedItem as Resource;

            resource.Id = int.Parse(txtBoxId.Text);
            resource.Name = txtBoxName.Text;
            resource.Description = txtBoxName.Text;

            resource.DiscoveryDate = (DateTime)resDateFound.SelectedDate;
            resource.Frequency = resFrequency.SelectionBoxItem.ToString();
            resource.UnitOfMeasure = resUnit.SelectionBoxItem.ToString();
            resource.MapID = int.Parse(resMap.SelectionBoxItem.ToString());
            resource.Price = int.Parse(resPrice.Text);

            resource.Icon = resImage.Source.ToString();

            resource.Renewable = (bool)resRenewable.IsChecked;
            resource.StrategicImportance = (bool)resStrategicImportance.IsChecked;
            resource.Exploitation = (bool)resExploatation.IsChecked;

            resource.Type = new_resource_type;
            resource.Tags = new_tags;

            manager.SaveResources();

            MessageBox.Show("Upravo ste izmenili resurs sa id " + resource.Id,
                "Dodat resurs", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void DeleteResource(object sender, RoutedEventArgs e)
        {
            Resource resource = dgrMain.SelectedItem as Resource;

            this.manager.MapData.Resources.Remove(resource);
            this.manager.SaveResources();
        }

        private void DefaultRenewableMsg(object sender, RoutedEventArgs e)
        {
            resRenewable.ToolTip = "Označi resurs obnovljivim";
        }

        private void RenewableMsgUnmark(object sender, RoutedEventArgs e)
        {
            resRenewable.ToolTip = "Poništi oznaku";
        }

        private void DefaultStrategicImportanceMsg(object sender, RoutedEventArgs e)
        {
            resStrategicImportance.ToolTip = "Označi resurs strateški bitnim";
        }

        private void StrategicImportanceMsgUnmark(object sender, RoutedEventArgs e)
        {
            resStrategicImportance.ToolTip = "Poništi oznaku";
        }

        private void ExploatationMsgUnmark(object sender, RoutedEventArgs e)
        {
            resExploatation.ToolTip = "Poništi oznaku";
        }

        private void DefaultExploatationMsg(object sender, RoutedEventArgs e)
        {
            resExploatation.ToolTip = "Označi resurs eksploatisanim";
        }

        

    }
}
