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
        private DataManager manager;

        public AddEtiquetteForm(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        private void idTextChanged(object sender, RoutedEventArgs e)
        {
            this.manager.resetEtiquetteCounter();
            var filtered = this.manager.MapData.Etiquettes.Where(et => et.Id.ToString().StartsWith(txtBoxId.Text));
            if (txtBoxId.Text == "") { filtered.ToList().Clear(); }
            if (filtered.ToList().Count != 0)
            {
                txtBoxId.Background = Brushes.Salmon;
                txtBoxIdError.Text = "Oznaka mora biti jedinstvena.";
            } else
            {
                txtBoxId.Background = Brushes.White;
                txtBoxIdError.Text = "";
            }
            dgrMain.ItemsSource = filtered;
        }

        private void AddEtiquette(object sender, RoutedEventArgs e)
        {
            var query = this.manager.MapData.Etiquettes.Where(i => i.Id == int.Parse(txtBoxId.Text));
            if (query.ToList().Count != 0)
            {
                MessageBox.Show("Etiketa sa oznakom " + query.ToList().First().Id + " već postoji.", "Duplikat", MessageBoxButton.OK,
                MessageBoxImage.Warning);
                return;
            }

            Etiquette etiquette = new Etiquette();
            etiquette.Color = new SolidColorBrush(colorPicker.SelectedColor.Value);
            etiquette.Id = int.Parse(txtBoxId.Text);
            etiquette.Description = txtBoxDescription.Text;

            manager.SaveEtiquette(etiquette);
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            // @TODO (maybe): find better way to close dialog
            this.Visibility = Visibility.Hidden;
        }
    }
}
