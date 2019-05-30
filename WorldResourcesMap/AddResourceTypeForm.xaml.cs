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
    /// Interaction logic for AddResourceTypeForm.xaml
    /// </summary>
    public partial class AddResourceTypeForm : Window
    {
        private DataManager manager;
 
        public AddResourceTypeForm(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
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
                resTypeImage.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void AddResourceType(object sender, RoutedEventArgs e)
        {
            ResourceType type = new ResourceType();
            type.Id = int.Parse(resTypeId.Text);
            type.Name = resTypeName.Text;
            type.Description = resTypeDescription.Text;
            type.Icon = resTypeImage.Source.ToString();
            manager.SaveResourceType(type);
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            // @TODO: find a better way to close dialog
            this.Visibility = Visibility.Hidden;
        }
    }
}
