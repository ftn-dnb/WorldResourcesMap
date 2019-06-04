using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WorldResourcesMap
{
    /// <summary>
    /// Interaction logic for AddResourceForm.xaml
    /// </summary>
    public partial class AddResourceForm : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Resource resource;

        private DataManager manager;
        public DataManager Manager
        {
            get
            {
                return manager;
            }
            set
            {
                if (value != manager)
                {
                    manager = value;
                    OnPropertyChanged("Manager");
                }
            }
        }

        private string _test0;
        public string Test0
        {
            get
            {
                return _test0;
            }
            set
            {
                if (value != _test0)
                {
                    _test0 = value;
                    OnPropertyChanged("Test0");
                }
            }
        }

        private string _test1;
        public string Test1
        {
            get
            {
                return _test1;
            }
            set
            {
                if (value != _test1)
                {
                    _test1 = value;
                    OnPropertyChanged("Test1");
                }
            }
        }

        private string _test2;
        public string Test2
        {
            get
            {
                return _test2;
            }
            set
            {
                if (value != _test2)
                {
                    _test2 = value;
                    OnPropertyChanged("Test2");
                }
            }
        }

        private float _test3;
        public float Test3
        {
            get
            {
                return _test3;
            }
            set
            {
                if (value != _test3)
                {
                    _test3 = value;
                    OnPropertyChanged("Test3");
                }
            }
        }

        public AddResourceForm(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this;
            resource = new Resource();
        }

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void AddImage(object sender, RoutedEventArgs e)
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

        private void AddResource(object sender, RoutedEventArgs e)
        {
           

            if(resId.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za oznaku resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(resName.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za naziv resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(resDescription.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za opis resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(resource.Type == null)
            {
                MessageBox.Show("Morate izabrati tip resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(resDateFound.SelectedDate == null)
            {
                MessageBox.Show("Morate izabrati datum otkrivanja resursa", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(resFrequency.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati frekvenciju ponavljanja resursa", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(resUnit.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati jedinicu mere resursa", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            if (resMap.SelectedItem == null)
            {
                MessageBox.Show("Morate izabrati mapu na kojoj će se prikazati resurs", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            if(resPrice.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za cenu resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            int id_test;
            float price_test;
            if (!int.TryParse(resId.Text, out id_test))
            {
                MessageBox.Show("Oznaka mora biti celobrojna vrednost", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!float.TryParse(resPrice.Text, out price_test))
            {
                MessageBox.Show("Cena mora biti nenegativna realna vrednost", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (float.Parse(resPrice.Text) < 0)
            {
                MessageBox.Show("Cena mora biti nenegativna realna vrednost", "Greška",
                 MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var query = this.manager.MapData.Resources.Where(i => i.Id == int.Parse(resId.Text));
            if (query.ToList().Count != 0)
            {
                MessageBox.Show("Resurs sa oznakom " + query.ToList().First().Id + " već postoji.", "Duplikat", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }


            resource.Id = int.Parse(resId.Text);
            resource.Name = resName.Text;
            resource.Description = resDescription.Text;
            
            resource.DiscoveryDate = (DateTime)resDateFound.SelectedDate;
            resource.Frequency = resFrequency.SelectionBoxItem.ToString();
            resource.UnitOfMeasure = resUnit.SelectionBoxItem.ToString();
            resource.MapID = int.Parse(resMap.SelectionBoxItem.ToString()); 
            resource.Price = float.Parse(resPrice.Text);
            if(resImage.Source.ToString().Contains("no-image.png"))
            {
                resource.Icon = resource.Type.Icon;
            }
            else
            {
                resource.Icon = resImage.Source.ToString();
            }
            
            
            resource.Renewable = (bool)resRenewable.IsChecked;
            resource.StrategicImportance = (bool)resStrategicImportance.IsChecked;
            resource.Exploitation = (bool)resExploatation.IsChecked;
            

            manager.SaveResource(resource);

            MessageBox.Show("Upravo ste dodali resurs sa id " + resource.Id,
                "Dodat resurs", MessageBoxButton.OK,
                MessageBoxImage.Information);

            //resetovanje forme
            resId.Text = "";
            resName.Text = "";
            resDescription.Text = "";

            resource = new Resource();
            resTypeName.Text = "Tip:";

            resDateFound.SelectedDate = null;
            resFrequency.SelectedItem = null;
            resUnit.SelectedItem = null;
            resMap.SelectedItem = null;
            resPrice.Text = "";

            resRenewable.IsChecked = false;
            resStrategicImportance.IsChecked = false;
            resExploatation.IsChecked = false;


            resImage.Source = new BitmapImage(new Uri("./resources/images/no-image.png",UriKind.Relative));


        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            // @TODO: find a better way to close a dialog
            this.Visibility = Visibility.Hidden;
        }

        private void TypeSelection(object sender, RoutedEventArgs e)
        {
            var form = new ResourceTypeSelection(manager, resource);
            form.ShowDialog();
            if(resource.Type == null)
            {
                resTypeName.Text = "Tip: Niste izabrali tip!";
                resTypeName.Foreground = Brushes.Red;
                return;
            }
            resTypeName.Text = "Tip: " + resource.Type.Name;
            resTypeName.Foreground = Brushes.Black; // bug ako je null tip
        }

        private void EtiqettePick(object sender, RoutedEventArgs e)
        {
            var form = new EtiquettePicker(manager, resource);
            form.ShowDialog();
        }

        private void ExploatationMsgUnmark(object sender, RoutedEventArgs e)
        {
            resExploatation.ToolTip = "Poništi oznaku";
        }

        private void DefaultExploatationMsg(object sender, RoutedEventArgs e)
        {
            resExploatation.ToolTip = "Označi resurs eksploatisanim";
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

        private void Help_AddResourceForm(object sender, ExecutedRoutedEventArgs e)
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
