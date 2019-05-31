using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WorldResourcesMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point startPoint = new Point();

        private const int ICON_SIZE = 30;
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
            ResourcesOnMap = new ObservableCollection<Resource>();

            FindElementsForCurrentMap("1");
        }

        private void FindElementsForCurrentMap(string selectedMap)
        {
            int map = int.Parse(selectedMap);

            List<Resource> query = (from r in DataManager.MapData.Resources
                                    where r.MapID == map
                                    select r).ToList();

            ResourcesList.Clear();
            ResourcesOnMap.Clear();
            CanvasMap.Children.Clear();

            foreach (Resource resource in query)
            {
                if (resource.X != -1 && resource.Y != -1)
                {
                    ResourcesOnMap.Add(resource);
                    AddIconToMap(resource);
                }
                else
                {
                    ResourcesList.Add(resource);
                }
            }
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
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null)
                    return;

                Resource resource = (Resource)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

                DataObject dragData = new DataObject("ListToCanvas", resource);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void CanvasMap_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("ListToCanvas") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private Resource ClickedResource(int x, int y) 
        {
            foreach (Resource resource in ResourcesOnMap) 
            {
                if (Math.Sqrt(Math.Pow((x - resource.X - OFFSET), 2) + Math.Pow((y - resource.Y - OFFSET), 2)) < 1 * OFFSET)
                    return resource;
            }

            return null;
        }

        private void CanvasMap_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(CanvasMap);

            if (!e.Data.GetDataPresent("ListToCanvas") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;

                Resource resource = e.Data.GetData("ListToCanvas") as Resource;

                if (resource == null)
                {
                    resource = e.Data.GetData("CanvasToCanvas") as Resource;

                    UIElement iconToRemove = null;

                    foreach (UIElement element in CanvasMap.Children)
                    {
                        if (element.Uid == resource.Id.ToString())
                        {
                            iconToRemove = element;
                            break;
                        }
                    }

                    CanvasMap.Children.Remove(iconToRemove);
                }
                else
                {
                    ResourcesList.Remove(resource);
                    ResourcesOnMap.Add(resource);
                }

                resource.X = (int)dropPosition.X;
                resource.Y = (int)dropPosition.Y;

                // @TODO dodati cuvanje u fajl jer smo promenili X i Y koordinate resursa

                AddIconToMap(resource);
            }
        }

        private void AddIconToMap(Resource resource)
        {
            Image icon = new Image
            {
                Width = ICON_SIZE,
                Height = ICON_SIZE,
                Uid = resource.Id.ToString(),
                Source = new BitmapImage(new Uri(resource.Icon))
            };

            icon.ToolTip = "Oznaka: " + resource.Id.ToString() + "\nNaziv: " + resource.Name;

            CanvasMap.Children.Add(icon);
            Canvas.SetLeft(icon, resource.X);
            Canvas.SetTop(icon, resource.Y);
        }

        private void CanvasMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);

            Canvas map = sender as Canvas;

            Resource dataObject = null;
            Point mousePos = e.GetPosition(map);

            dataObject = ClickedResource((int)mousePos.X, (int)mousePos.Y);

            if (dataObject != null)
            {
                DataObject data = new DataObject("CanvasToCanvas", dataObject);
                DragDrop.DoDragDrop(map, data, DragDropEffects.Move);
            }
        }

    }
}
