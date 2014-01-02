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
using muzeum_v3.Views.Location;

namespace muzeum_v3.Views.Hall
{
    /// <summary>
    /// Interaction logic for HallDisplayView.xaml
    /// </summary>
    public partial class HallDisplayView : UserControl
    {
        public HallDisplayView()
        {
            InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window window = new AddLocationWindow();
            window.Show();
        }
    }
}
