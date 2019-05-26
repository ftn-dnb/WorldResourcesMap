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
        public AddResourceForm()
        {
            InitializeComponent();
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

        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            // @TODO: find a better way to close a dialog
            this.Visibility = Visibility.Hidden;
        }
    }
}
