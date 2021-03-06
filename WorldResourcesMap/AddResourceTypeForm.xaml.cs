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
    /// Interaction logic for AddResourceTypeForm.xaml
    /// </summary>
    public partial class AddResourceTypeForm : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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

        private string _test2;
        public string Test2
        {
            get
            {
                return _test2;
            }
            set
            {
                if (value != _test2)
                {
                    _test2 = value;
                    OnPropertyChanged("Test2");
                }
            }
        }


        private DataManager manager;
 
        public AddResourceTypeForm(DataManager manager)
        {
            InitializeComponent();
            this.manager = manager;
            this.DataContext = this;
        }

        private void idTextChanged(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (!int.TryParse(resTypeId.Text, out id))
            {
                txtBoxIdError.Text = "Oznaka mora biti ceo broj.";
                return;
            }

            this.manager.resetTypeCounter();
            var filtered = this.manager.MapData.Types.Where(et => et.Id.ToString().StartsWith(resTypeId.Text));
            if (resTypeId.Text == "") { filtered.ToList().Clear(); }
            if (filtered.ToList().Count != 0)
            {
                //resTypeId.Background = Brushes.Salmon;
                txtBoxIdError.Text = "Oznaka mora biti jedinstvena.";
            }
            else
            {
                resTypeId.Background = Brushes.White;
                txtBoxIdError.Text = "";
            }
            dgrMain.ItemsSource = filtered;
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

        private void AddResourceType(object sender, RoutedEventArgs e)
        {
            if (resTypeId.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za oznaku tipa resursa.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (resTypeName.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za naziv tipa resursa.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (resTypeDescription.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti polje za opis tipa resursa.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            int id = 0;
            if (!int.TryParse(resTypeId.Text, out id))
            {
                MessageBox.Show("Oznaka tipa resursa mora biti ceo broj.", "Nedovršen unos podataka", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var query = this.manager.MapData.Types.Where(i => i.Id == int.Parse(resTypeId.Text));

            if (query.ToList().Count != 0)
            {
                MessageBox.Show("Tip sa oznakom " + query.ToList().First().Id + " već postoji.", "Duplikat", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            ResourceType type = new ResourceType();
            type.Id = int.Parse(resTypeId.Text);
            type.Name = resTypeName.Text;
            type.Description = resTypeDescription.Text;
            type.Icon = resTypeImage.Source.ToString();

            manager.SaveResourceType(type);

            MessageBox.Show("Upravo ste dodali tip resursa sa id " + type.Id,
                "Dodat tip resursa", MessageBoxButton.OK,
                MessageBoxImage.Information);

            resTypeId.Text = "";
            resTypeDescription.Text = "";
            resTypeName.Text = "";

            resTypeImage.Source = new BitmapImage(new Uri("./resources/images/no-image.png", UriKind.Relative));
        }

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da želite da odustanete ? Napravljene promene neće biti sačuvane",
                    "Odustajanje od dodavanja", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            // @TODO: find a better way to close dialog
            this.Visibility = Visibility.Hidden;
        }

        private void AddResourceTypeHelp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[2]);

            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);

                HelpProvider.ShowHelp(str, this);
            }
        }
    }
}
