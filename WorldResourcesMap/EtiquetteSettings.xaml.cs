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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
           
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
            
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            Etiquette etiquette = dgrMain.SelectedItem as Etiquette;
            this.manager.MapData.Etiquettes.Remove(etiquette);
            //TODO: potrebno je sacuvati izmjenu u JSON fajl ( nova metoda )
      
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
            this.manager = manager;
            this.DataContext = this.manager;
            InitializeComponent();
            this.manager.MapData.Etiquettes.Add(new Etiquette(1, Brushes.Black, "etiketa1"));
            this.manager.MapData.Etiquettes.Add(new Etiquette(2, Brushes.Blue, "etiketa2"));
            this.manager.MapData.Etiquettes.Add(new Etiquette(3, Brushes.Black, "etiketa3"));
            this.manager.MapData.Etiquettes.Add(new Etiquette(4, Brushes.Brown, "etiketa4"));
            View = CollectionViewSource.GetDefaultView(this.manager.MapData.Etiquettes);
        }
    }
}
