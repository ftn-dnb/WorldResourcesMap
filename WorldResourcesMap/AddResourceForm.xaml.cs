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
           

            // dodati proveru
            //Resource resource = new Resource();
            resource.Id = int.Parse(resId.Text);
            resource.Name = resName.Text;
            resource.Description = resDescription.Text;
            
            resource.DiscoveryDate = (DateTime)resDateFound.SelectedDate;
            resource.Frequency = resFrequency.SelectionBoxItem.ToString();
            resource.UnitOfMeasure = resUnit.SelectionBoxItem.ToString();
            resource.MapID = int.Parse(resMap.SelectionBoxItem.ToString()); 
            resource.Price = int.Parse(resPrice.Text);
            resource.Icon = resImage.Source.ToString();

            resource.Renewable = (bool)resRenewable.IsChecked;
            resource.StrategicImportance = (bool)resStrategicImportance.IsChecked;
            resource.Exploitation = (bool)resExploatation.IsChecked;
            

            manager.SaveResource(resource);

            resource = new Resource();
            resTypeName.Text = "Tip:";


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
            resTypeName.Text = "Tip: " + resource.Type.Name;
        }
    }
}
