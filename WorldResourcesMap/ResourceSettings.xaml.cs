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
    /// Interaction logic for ResourceSettings.xaml
    /// </summary>
    public partial class ResourceSettings : Window, INotifyPropertyChanged
    {
        private DataManager manager;
        public ResourceSettings(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this.manager;

            View = CollectionViewSource.GetDefaultView(this.manager.MapData.Resources);
        }

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
    }
}
