﻿using Microsoft.Win32;
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
    /// Interaction logic for ResourceTypeSettings.xaml
    /// </summary>
    public partial class ResourceTypeSettings : Window, INotifyPropertyChanged
    {
        private DataManager manager;
        private bool valid;
        private string selected_id;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void keyUpSearch(object sender, RoutedEventArgs e)
        {
            this.manager.resetTypeCounter();
            var filtered = this.manager.MapData.Types.Where(ty => ty.Name.StartsWith(Search.Text));
            dgrMain.ItemsSource = filtered;
        }

        private void selectionChangedSearch(object sender, RoutedEventArgs e)
        {
            this.manager.resetTypeCounter();
            var filtered = this.manager.MapData.Types.Where(ty => ty.Name.StartsWith(Search.Text));
            dgrMain.ItemsSource = filtered;
        }

        private void keyUpChangeId(object sender, RoutedEventArgs e)
        {
            var filtered = this.manager.MapData.Types.Where(et => string.Compare(et.Id.ToString(), txtBoxId.Text) == 0);
            if (filtered.ToList().Count != 0)
            {
                if (string.Compare(filtered.ToList().First().Id.ToString(), this.selected_id) != 0)
                {
                    txtBoxId.Background = Brushes.Salmon;
                    idTextBoxError.Text = "Oznaka mora biti jedinstvena.";
                    this.valid = false;
                }
                else
                {
                    txtBoxId.Background = Brushes.White;
                    idTextBoxError.Text = "";
                    this.valid = true;
                }
            }
            else
            {
                txtBoxId.Background = Brushes.White;
                idTextBoxError.Text = "";
                this.valid = true;
            }
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

        public ResourceTypeSettings(DataManager manager)
        {
            this.manager = manager;
            this.DataContext = this.manager;
            this.valid = true;

            InitializeComponent();


            //this.manager.MapData.Types.Add(new ResourceType(1, "aa", @"E:\dev\WorldResourcesMap\WorldResourcesMap\resources\images\no-image.png", "opis1"));
            //this.manager.MapData.Types.Add(new ResourceType(2, "bb", @"E:\dev\WorldResourcesMap\WorldResourcesMap\resources\images\no-image.png", "opis2"));
            //this.manager.MapData.Types.Add(new ResourceType(3, "cc", @"E:\dev\WorldResourcesMap\WorldResourcesMap\resources\images\no-image.png", "opis3"));

            EnableEditForm(false);
            View = CollectionViewSource.GetDefaultView(this.manager.MapData.Types);
        }

        private void EnableEditForm(bool state)
        {
            if (txtBoxId == null) // Komponenta jos nije inicijalizovana
                return;

            txtBoxId.IsEnabled = state;
            txtBoxName.IsEnabled = state;
            txtBoxDescription.IsEnabled = state;
            btnChangeImage.IsEnabled = state;
            btnRemoveImage.IsEnabled = state;
            btnEditType.IsEnabled = state;
            btnRemoveType.IsEnabled = state;
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            ResourceType item = dgrMain.SelectedItem as ResourceType;
            if (!this.valid)
            {
                MessageBox.Show("Nije moguće izmeniti tip resursa " + item.Id,
                "Upozorenje o izmeni podataka", MessageBoxButton.OK,
                MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("Da li ste sigurni da želite da izmenite podatake za tip resursa sa oznakom " + item.Id + " ?",
                "Upozorenje o izmeni podataka", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            item.Id = int.Parse(txtBoxId.Text);
            item.Name = txtBoxName.Text;
            item.Description = txtBoxDescription.Text;
            item.Icon = resTypeImage.Source.ToString();

            this.manager.SaveResourceTypes();
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            ResourceType item = dgrMain.SelectedItem as ResourceType;
            
            if (MessageBox.Show("Da li ste sigurni da želite da obrišete tip resursa sa oznakom " + item.Id + " ?", 
                "Upozorenje o brisanju", MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            this.manager.MapData.Types.Remove(item);
            this.manager.SaveResourceTypes();
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

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            ResourceType item = dgrMain.SelectedItem as ResourceType;
            item.Icon = "./resources/images/no-image.png";
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableEditForm(true);

            ResourceType item = dgrMain.SelectedItem as ResourceType;

            if (txtBoxId == null) // Komponenta jos nije inicijalizovana
                return;

            try
            {
                this.selected_id = item.Id.ToString();
                txtBoxId.Text = item.Id.ToString();
                txtBoxName.Text = item.Name;
                txtBoxDescription.Text = item.Description;
                resTypeImage.Source = new BitmapImage(new Uri(item.Icon));
            }
            catch (Exception ex) { }
            
        }
    }
}
