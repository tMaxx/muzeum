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
using muzeum_v3.Views.Org;
using muzeum_v3.Views.Location;
using muzeum_v3.Views.Sale;

namespace muzeum_v3.Views.Exposition
{
    /// <summary>
    /// Interaction logic for ExpositionDisplayView.xaml
    /// </summary>
    public partial class ExpositionDisplayView : UserControl
    {
        public ExpositionDisplayView()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window window = new AddLocationWindow();
            window.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window window = new AddOrgWindow();
            window.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Window window = new BuySomeTicketsWindow();
            window.Show();
        }
    }
}
