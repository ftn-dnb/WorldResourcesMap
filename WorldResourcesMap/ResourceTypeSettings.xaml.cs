using Microsoft.Win32;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
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

            InitializeComponent();

            this.manager.MapData.Types.Add(new ResourceType(1, "aa", "", "opis1"));
            this.manager.MapData.Types.Add(new ResourceType(2, "bb", "", "opis2"));
            this.manager.MapData.Types.Add(new ResourceType(3, "cc", "", "opis3"));

            View = CollectionViewSource.GetDefaultView(this.manager.MapData.Types);
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {

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
            //TODO: potrebno je sacuvati izmjenu u JSON fajl ( nova metoda )
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
                ResourceType item = dgrMain.SelectedItem as ResourceType;
                item.Icon = fileDialog.FileName;
            }
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            ResourceType item = dgrMain.SelectedItem as ResourceType;
            item.Icon = "./resources/images/no-image.png";
        }
    }
}
