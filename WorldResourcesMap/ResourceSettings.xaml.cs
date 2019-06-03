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
using Microsoft.Win32;

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

        private void AddImage(object sender , RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Odaberi sliku";
            fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png";

            if (fileDialog.ShowDialog() == true)
            {
                resImage.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            resImage.Source = new BitmapImage(new Uri("./resources/images/no-image.png",UriKind.Relative));
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
            if (txtBoxId.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za oznaku resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtBoxName.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za naziv resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtBoxDescription.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za opis resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (new_resource_type == null)
            {
                MessageBox.Show("Morate izabrati tip resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (resDateFound.SelectedDate == null)
            {
                MessageBox.Show("Morate izabrati datum otkrivanja resursa", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (resFrequency.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati frekvenciju ponavljanja resursa", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (resUnit.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati jedinicu mere resursa", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            if (resMap.SelectedItem == null)
            {
                MessageBox.Show("Morate uneti cenu resursa", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            if (resPrice.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za oznaku resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Resource resource = dgrMain.SelectedItem as Resource;

            int id_test;
            float price_test;
            if (!int.TryParse(txtBoxId.Text, out id_test))
            {
                //dodati upozorenje
                return;
            }

            if (!float.TryParse(resPrice.Text, out price_test))
            {
                //dodati upozorenje
                return;
            }
            resource.Id = int.Parse(txtBoxId.Text);

            resource.Name = txtBoxName.Text;
            resource.Description = txtBoxName.Text;

            resource.DiscoveryDate = (DateTime)resDateFound.SelectedDate;
            resource.Frequency = resFrequency.SelectionBoxItem.ToString();
            resource.UnitOfMeasure = resUnit.SelectionBoxItem.ToString();

            resource.MapID = int.Parse(resMap.SelectionBoxItem.ToString());
            resource.Price = float.Parse(resPrice.Text);

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Help_ResourceSettingsForm(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[1]);

            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }
    }
}
