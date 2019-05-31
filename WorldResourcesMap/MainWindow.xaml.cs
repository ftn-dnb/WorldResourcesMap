using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorldResourcesMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point startPoint = new Point();

        private const int ICON_SIZE = 70;
        private const int OFFSET = ICON_SIZE / 2;

        public DataManager DataManager { get; set; }

        public ObservableCollection<Resource> ResourcesList { get; set; }
        public ObservableCollection<Resource> ResourcesOnMap { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataManager = new DataManager();
            this.DataContext = this;

            ResourcesList = new ObservableCollection<Resource>();

            FindElementsForCurrentMap("1");
        }

        private void FindElementsForCurrentMap(string selectedMap)
        {
            int map = int.Parse(selectedMap);

            List<Resource> query = (from r in DataManager.MapData.Resources
                                    where r.MapID == map
                                    select r).ToList();

            ResourcesList.Clear();

            foreach (Resource resource in query)
                ResourcesList.Add(resource);
        }

        private void OpenEtiqetteSettings(object sender, RoutedEventArgs e)
        {
            var form = new EtiquetteSettings(DataManager);
            form.ShowDialog();
        }


        private void OpenAddEtiquette(object sender, RoutedEventArgs e)
        {
            var form = new AddEtiquetteForm(DataManager);
            form.ShowDialog();
        }

        private void OpenAddResourceType(object sender, RoutedEventArgs e)
        {
            var form = new AddResourceTypeForm(DataManager);
            form.ShowDialog();
        }

        private void OpenResourceTypesSettings(object sender, RoutedEventArgs e)
        {
            var form = new ResourceTypeSettings(DataManager);
            form.ShowDialog();
        }

        private void OpenAddResource(object sender, RoutedEventArgs e)
        {
            var form = new AddResourceForm(DataManager);
            form.ShowDialog();
        }

        private void ChangedMapEvent(object sender, SelectionChangedEventArgs e)
        {
            if (cbMap.Text == "")
                return;

            ComboBoxItem typeItem = (ComboBoxItem)cbMap.SelectedItem;
            string value = typeItem.Content.ToString();

            FindElementsForCurrentMap(value);
        }


        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            startPoint = e.GetPosition(null);

            StackPanel panel = sender as StackPanel;
            Resource dataObject = null;

            foreach (Resource resource in ResourcesList)
            {
                if ((int)panel.Tag == resource.Id)
                {
                    dataObject = resource;
                    break;
                }
            }

            DataObject data = new DataObject("dd", dataObject);
            DragDrop.DoDragDrop(panel, data, DragDropEffects.Move);
        }

        private void CanvasMap_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CanvasMap_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void CanvasMap_DragEnter(object sender, DragEventArgs e)
        {
        }

        private Resource ClickedResource(int x, int y)   //vraca kliknutu vrstu na kanvasu
        {
            foreach (Resource resource in ResourcesList)   //prolazim kroz vrste spustene na kanvas
            {
                if (Math.Sqrt(Math.Pow((x - resource.X - OFFSET), 2) + Math.Pow((y - resource.Y - OFFSET), 2)) < 1 * OFFSET)
                    return resource;
            }

            return null;
        }

        private void CanvasMap_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(CanvasMap);

            if (ClickedResource((int)dropPosition.X, (int)dropPosition.Y) != null)
                return;

            if (e.Data.GetDataPresent("dd"))    //sa panela na kanvas
            {
                Resource resource = e.Data.GetData("dd") as Resource;

                ResourcesList.Remove(resource);

                resource.X = (int)dropPosition.X - OFFSET;
                resource.Y = (int)dropPosition.Y - OFFSET;

                // @TODO dodati cuvanje u fajl

                ResourcesOnMap.Add(resource);

                Canvas canvas = this.CanvasMap;

                Image icon = new Image
                {
                    Width = ICON_SIZE,
                    Height = ICON_SIZE,
                    Uid = resource.Id.ToString(),
                    Source = new BitmapImage(new Uri(resource.Icon, UriKind.Absolute))
                };

                icon.ToolTip = resource.Id; // @TODO: Promeniti tooltip


                canvas.Children.Add(icon);

                Canvas.SetLeft(icon, resource.X);
                Canvas.SetTop(icon, resource.Y);

                return;
            }

        }

        private void CanvasMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);

            Canvas map = sender as Canvas;

            Resource dataObject = null;
            Point mousePosition = e.GetPosition(CanvasMap);

            dataObject = ClickedResource((int)mousePosition.X, (int)mousePosition.Y);

            if (dataObject != null)
            {
                DataObject data = new DataObject("dd", dataObject);
                DragDrop.DoDragDrop(map, data, DragDropEffects.Move);
            }
        }
    }
}
