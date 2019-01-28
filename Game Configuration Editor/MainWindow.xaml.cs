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

namespace Game_Configuration_Editor
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


        private void format_Checked(object sender, RoutedEventArgs e)
        {
            if (chbINI.IsChecked == true)
            {

            }
        }

        private void btnScan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChangeDir_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
