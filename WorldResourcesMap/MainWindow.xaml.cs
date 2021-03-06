﻿using System;
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

        private void keyUpSearch(object sender, RoutedEventArgs e)
        {
            DataManager.resetResourceCounter();
            int selectedMap = int.Parse(cbMap.SelectionBoxItem.ToString());
            var filtered = DataManager.MapData.Resources.Where(et => et.Name.StartsWith(Search.Text) && et.MapID == selectedMap);
            Lista.ItemsSource = filtered;
            
            FindElementsForCurrentMapSearch(filtered.ToList());
        }

        private void selectionChangedSearch(object sender, RoutedEventArgs e)
        {
            DataManager.resetResourceCounter();
            int selectedMap = int.Parse(cbMap.SelectionBoxItem.ToString());
            var filtered = DataManager.MapData.Resources.Where(et => et.Name.StartsWith(Search.Text) && et.MapID == selectedMap);
            Lista.ItemsSource = filtered;
            FindElementsForCurrentMapSearch(filtered.ToList());
        }

        private void FindElementsForCurrentMapSearch(IList<Resource> filtered)
        {
            
            //ResourcesList.Clear();
            ResourcesOnMap.Clear();
            CanvasMap.Children.Clear();
            
            foreach (Resource resource in filtered)
            {
                if (resource.OnMap) // Resource is on the map
                {
                    ResourcesOnMap.Add(resource);
                    AddIconToMap(resource);
                }
                try
                {
                    //ResourcesList.Add(resource);
                }
                catch (Exception e) { }
            }
        }

        private void FindElementsForCurrentMap(string selectedMap)
        {
            int map = int.Parse(selectedMap);

            List<Resource> query = (from r in DataManager.MapData.Resources
                                    where r.MapID == map
                                    select r).ToList();

            Lista.ItemsSource = ResourcesList;
            ResourcesList.Clear();
            ResourcesOnMap.Clear();
            CanvasMap.Children.Clear();

            foreach (Resource resource in query)
            {
                if (resource.OnMap) // Resource is on the map
                {
                    ResourcesOnMap.Add(resource);
                    AddIconToMap(resource);
                }

                ResourcesList.Add(resource);
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

       private void OpenResourceSettings(object sender, RoutedEventArgs e)
        {
            var form = new ResourceSettings(DataManager);
            form.ShowDialog();
            ChangedMapEvent(null, null); // @Hack: refresh current elements on map after adding new resource
        }

        private void OpenAddResource(object sender, RoutedEventArgs e)
        {
            var form = new AddResourceForm(DataManager);
            form.ShowDialog();
            ChangedMapEvent(null, null); // @Hack: refresh current elements on map after adding new resource
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

        private Resource FindResourceOnMap(int x, int y) 
        {
            foreach (Resource resource in ResourcesOnMap) 
            {
                if (Math.Sqrt(Math.Pow((x - resource.X - OFFSET), 2) + Math.Pow((y - resource.Y - OFFSET), 2)) < OFFSET)
                    return resource;
            }

            return null;
        }

        private void CanvasMap_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(CanvasMap);

            Resource res = FindResourceOnMap((int)dropPosition.X, (int)dropPosition.Y);
            if (res != null)
            {
                MessageBox.Show("Ne možete postaviti resurs na ovu lokaciju zato što je ona već zauzeta.",
                                "Zauzeta lokacija", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!e.Data.GetDataPresent("ListToCanvas") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;

                Resource resource = e.Data.GetData("ListToCanvas") as Resource;

                if (resource == null) // resource dragged from the map
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
                    if (resource.OnMap) // you can't move resource from list that is already on the map
                    {
                        MessageBox.Show("Ne možete postaviti resurs '" + resource.Name + "' sa oznakom " + resource.Id + " na mapu jer se on već nalazi na njoj.",
                                "Resurs je na mapi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    ResourcesOnMap.Add(resource);
                }

                resource.X = (int)dropPosition.X;
                resource.Y = (int)dropPosition.Y;
                resource.OnMap = true;

                DataManager.SaveResources();

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

            dataObject = FindResourceOnMap((int)mousePos.X, (int)mousePos.Y);

            if (dataObject != null)
            {
                DataObject data = new DataObject("CanvasToCanvas", dataObject);
                DragDrop.DoDragDrop(map, data, DragDropEffects.Move);
            }
        }
        private void CommandBinding_Executed_Main(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.MainWindow);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }
    }
}
