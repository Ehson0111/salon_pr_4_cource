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

namespace pr1_salon.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnguest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Service_page1());

        }

        private void btnadmin_Click(object sender, RoutedEventArgs e)
        {
            if (txtcode.Text.ToLower()=="0000")
            {
                NavigationService.Navigate(new Service_page1(1));

            }



        }
    }
}
