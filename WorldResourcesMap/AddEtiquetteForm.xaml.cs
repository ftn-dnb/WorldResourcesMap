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
    /// Interaction logic for AddEtiquetteForm.xaml
    /// </summary>
    public partial class AddEtiquetteForm : Window
    {
        public AddEtiquetteForm()
        {
            InitializeComponent();
        }

        private void AddEtiquette(object sender, RoutedEventArgs e)
        {

        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            // @TODO (maybe): find better way to close dialog
            this.Visibility = Visibility.Hidden;
        }
    }
}
