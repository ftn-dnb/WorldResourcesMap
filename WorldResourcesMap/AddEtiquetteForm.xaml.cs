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
    /// Interaction logic for AddEtiquetteForm.xaml
    /// </summary>
    public partial class AddEtiquetteForm : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public AddEtiquetteForm(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void idTextChanged(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (!int.TryParse(txtBoxId.Text, out id))
            {
                txtBoxIdError.Text = "Oznaka mora biti ceo broj.";
                return;
            }

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
            if (txtBoxId.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za oznaku etikete.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (colorPicker.SelectedColor == null)
            {
                MessageBox.Show("Morate odabrati boju etikete.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (txtBoxDescription.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za opis etikete.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            int id = 0;
            if (!int.TryParse(txtBoxId.Text, out id))
            {
                MessageBox.Show("Oznaka etikete mora biti ceo broj.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var query = this.manager.MapData.Etiquettes.Where(i => i.Id == int.Parse(txtBoxId.Text));
            if (query.ToList().Count != 0)
            {
                MessageBox.Show("Etiketa sa oznakom " + query.ToList().First().Id + " već postoji.", "Duplikat", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            Etiquette etiquette = new Etiquette();
            try
            {
                etiquette.Color = new SolidColorBrush(colorPicker.SelectedColor.Value);
                etiquette.Id = int.Parse(txtBoxId.Text);
                etiquette.Description = txtBoxDescription.Text;

                manager.SaveEtiquette(etiquette);
            } catch (Exception ex)
            {
                etiquette.Color = Brushes.White;
                etiquette.Id = int.Parse(txtBoxId.Text);
                etiquette.Description = txtBoxDescription.Text;

                manager.SaveEtiquette(etiquette);
            }
            MessageBox.Show("Upravo ste dodali etiketu sa id " + etiquette.Id,
                "Dodata etiketa", MessageBoxButton.OK,
                MessageBoxImage.Information);
            txtBoxId.Text = "";
            txtBoxDescription.Text = "";
            //colorPicker.Background = Brushes.White;
            colorPicker.SelectedColor = null;
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da želite da odustanete ? Napravljene promene neće biti sačuvane",
                    "Odustajanje od dodavanja", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            // @TODO (maybe): find better way to close dialog
            this.Visibility = Visibility.Hidden;
        }
    }
}
