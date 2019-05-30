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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorldResourcesMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataManager DataManager { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataManager = new DataManager();
            DataManager.SaveDataToFile();
        }

        private void OpenAddEtiquette(object sender, RoutedEventArgs e)
        {
            var form = new AddEtiquetteForm();
            form.ShowDialog();
        }

        private void OpenAddResourceType(object sender, RoutedEventArgs e)
        {
            var form = new AddResourceTypeForm();
            form.ShowDialog();
        }

        private void OpenAddResource(object sender, RoutedEventArgs e)
        {
            var form = new AddResourceForm();
            form.ShowDialog();
        }
        
    }
}
