using Microsoft.Win32;
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

namespace WorldResourcesMap
{
    /// <summary>
    /// Interaction logic for AddResourceForm.xaml
    /// </summary>
    public partial class AddResourceForm : Window
    {

        private DataManager manager;
        private Resource resource;
        public AddResourceForm(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this.manager;
            resource = new Resource();
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
                MessageBox.Show("Morate uneti cenu resursa", "Nedovršen unos podataka",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            if(resPrice.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za oznaku resursa", "Nedovršen unos podataka",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            resource.Id = int.Parse(resId.Text);
            resource.Name = resName.Text;
            resource.Description = resDescription.Text;
            
            resource.DiscoveryDate = (DateTime)resDateFound.SelectedDate;
            resource.Frequency = resFrequency.SelectionBoxItem.ToString();
            resource.UnitOfMeasure = resUnit.SelectionBoxItem.ToString();
            resource.MapID = int.Parse(resMap.SelectionBoxItem.ToString()); 
            resource.Price = int.Parse(resPrice.Text);
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
