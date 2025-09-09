using pr1_salon.Pages;
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

namespace pr1_salon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //frame.Content = new Pages.Service_page1(frame);
            FrmMain.Navigate(new Auth ());
        
            //NavigationService.GetNavigationService();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrmMain.GoBack();
        }

        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrmMain.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;

            }
            else
            {
                btnBack.Visibility = Visibility.Hidden;
            }
        }
        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }
    }
}
