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
using muzeum_v3.Views.Author;
using muzeum_v3.Views.Owner;
using muzeum_v3.Views.Presentation;

namespace muzeum_v3.Views.Exhibit
{
    /// <summary>
    /// Interaction logic for ExhibitDisplayView.xaml
    /// </summary>
    public partial class ExhibitDisplayView : UserControl
    {
        public ExhibitDisplayView()
        {
            InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window window = new AddAuthorWindow();
            window.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window window = new AddOwnerWindow();
            window.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Window window = new PresentExhibit();
            window.Show();
        }

       
    }
}
