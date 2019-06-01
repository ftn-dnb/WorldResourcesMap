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

namespace WorldResourcesMap
{
    /// <summary>
    /// Interaction logic for EtiquetteSettings.xaml
    /// </summary>
    public partial class EtiquetteSettings : Window, INotifyPropertyChanged
    {
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

        private string selected_id;
        private bool valid;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public EtiquetteSettings(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this;
            View = CollectionViewSource.GetDefaultView(this.manager.MapData.Etiquettes);
            this.valid = true;
        }

        private void keyUpSearch(object sender, RoutedEventArgs e)
        {
            this.manager.resetEtiquetteCounter();
            var filtered = this.manager.MapData.Etiquettes.Where(et => et.Id.ToString().StartsWith(Search.Text));
            dgrMain.ItemsSource = filtered;
        }

        private void keyUpChangeId(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (!int.TryParse(idTextBox.Text, out id))
            {
                idTextBoxError.Text = "Oznaka mora biti ceo broj.";
                return;
            }

            var filtered = this.manager.MapData.Etiquettes.Where(et => string.Compare(et.Id.ToString(), idTextBox.Text) == 0);
            if (filtered.ToList().Count != 0)
            {
                if (string.Compare(filtered.ToList().First().Id.ToString(), this.selected_id) != 0)
                {
                    //idTextBox.Background = Brushes.Salmon;
                    idTextBoxError.Text = "Oznaka mora biti jedinstvena.";
                    this.valid = false;
                }
                else
                {
                    idTextBox.Background = Brushes.White;
                    idTextBoxError.Text = "";
                    this.valid = true;
                }
            }
            else
            {
                idTextBox.Background = Brushes.White;
                idTextBoxError.Text = "";
                this.valid = true;
            }
        }

        private void selectionChangedSearch(object sender, RoutedEventArgs e)
        {
            this.manager.resetEtiquetteCounter();
            var filtered = this.manager.MapData.Etiquettes.Where(et => et.Id.ToString().StartsWith(Search.Text));
            dgrMain.ItemsSource = filtered;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            Etiquette etiquette = dgrMain.SelectedItem as Etiquette;

            if (MessageBox.Show("Da li ste sigurni da želite da obrišete etiketu sa oznakom " + etiquette.Id + " ?",
                "Upozorenje o brisanju", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            this.manager.MapData.Etiquettes.Remove(etiquette);
            this.manager.SaveEtiquettes();
        }

        private void ChangeItem(object sender, RoutedEventArgs e)
        {
            if (idTextBox.Text.Length == 0)
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

            if (descTextBox.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za opis etikete.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            int id = 0;
            if (!int.TryParse(idTextBox.Text, out id))
            {
                MessageBox.Show("Oznaka etikete mora biti ceo broj.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            Etiquette etiquette = dgrMain.SelectedItem as Etiquette;
            if (!this.valid)
            {
                MessageBox.Show("Nije moguće izmeniti etiketu " + etiquette.Id,
                    "Upozorenje o izmeni podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Da li ste sigurni da želite da izmenite podatake za etiketu sa oznakom " + etiquette.Id + " ?",
                    "Upozorenje o izmeni podataka", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                etiquette.Id = int.Parse(idTextBox.Text);
                etiquette.Color = new SolidColorBrush(colorPicker.SelectedColor.Value);
                etiquette.Description = descTextBox.Text;
                this.manager.SaveEtiquettes();
            }
            catch (Exception ex) {
                etiquette.Id = int.Parse(idTextBox.Text);
                etiquette.Description = descTextBox.Text;
                this.manager.SaveEtiquettes();
            }
        }

        private void EtiquetteSelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                Etiquette etiquette = dgrMain.SelectedItem as Etiquette;
                this.selected_id = etiquette.Id.ToString();
                idTextBox.Text = etiquette.Id.ToString();
                colorPicker.Background = etiquette.Color;
                descTextBox.Text = etiquette.Description;
            }
            catch (Exception ex) { }
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

        
    }
}
