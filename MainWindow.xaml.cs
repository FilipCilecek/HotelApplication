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

namespace HotelApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //using (hotelDBEntities context = new hotelDBEntities())
            //{
            //    context.Storages.Add(new Storage {});
            //    context.SaveChanges();
            //}

        }

        private void ButtonPopUpAdmin_Click(object sender, RoutedEventArgs e)
        {
            this.MainLeftListBox.UnselectAll();
            this.mainContentControl.Content = new AdminControlWindow();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void Listview_home_Selected(object sender, RoutedEventArgs e)
        {
            
        }

        private void Listview_reservation_Selected(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new ReservationControlWindow();
        }

        private void Listview_pay_Selected(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new PayControlWindow();
        }

        private void Listview_storage_Selected(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new StorageControlWindow();
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            this.mainContentControl.Content = new HelpControlWindow();
        }
    }
}
