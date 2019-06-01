﻿using System;
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

        private string selected_id;
        private bool valid;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void keyUpSearch(object sender, RoutedEventArgs e)
        {
            this.manager.resetEtiquetteCounter();
            var filtered = this.manager.MapData.Etiquettes.Where(et => et.Id.ToString().StartsWith(Search.Text));
            dgrMain.ItemsSource = filtered;
        }

        private void keyUpChangeId(object sender, RoutedEventArgs e)
        {
            var filtered = this.manager.MapData.Etiquettes.Where(et => string.Compare(et.Id.ToString(), idTextBox.Text) == 0);
            if (filtered.ToList().Count != 0)
            {
                if (string.Compare(filtered.ToList().First().Id.ToString(), this.selected_id) != 0)
                {
                    idTextBox.Background = Brushes.Salmon;
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
            Etiquette etiquette = dgrMain.SelectedItem as Etiquette;
            if (!this.valid)
            {
                MessageBox.Show("Nije moguće izmeniti etiketu " + etiquette.Id,
                "Upozorenje o izmeni podataka", MessageBoxButton.OK,
                MessageBoxImage.Warning);
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

        public EtiquetteSettings(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this.manager;
            View = CollectionViewSource.GetDefaultView(this.manager.MapData.Etiquettes);
            this.valid = true;
        }
    }
}
