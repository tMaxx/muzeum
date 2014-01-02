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
using System.Windows.Navigation;
using System.Windows.Shapes;
using muzeum_v3.Views.Presentation;
namespace muzeum_v3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ExhibitDisplaySelectorView_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("GetExhibits");
            App.Messenger.NotifyColleagues("Clear");
        }


        private void AuthorDisplaySelectorView_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("GetAuthors");
            App.Messenger.NotifyColleagues("Clear");
        }

        private void OwnerDisplaySelectorView_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("GetOwners");
            App.Messenger.NotifyColleagues("Clear");
        }

        private void ExpositionDisplaySelectorView_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("GetExpositions");
            App.Messenger.NotifyColleagues("Clear");
        }

        private void OrgDisplaySelectorView_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("GetOrgs");
            App.Messenger.NotifyColleagues("Clear");
        }

        private void LocationDisplaySelectorView_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("Clear");
            App.Messenger.NotifyColleagues("GetLocations");
        }

        private void HallDisplaySelectorView_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("GetHalls");
            App.Messenger.NotifyColleagues("Clear");
        }

        private void ExhibitList_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("ClearList");
        }

        private void ExpositionList_Loaded_1(object sender, RoutedEventArgs e)
        {
            App.Messenger.NotifyColleagues("ClearList");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window window = new UpdatePres();
            window.Show();
        }

    }
}
