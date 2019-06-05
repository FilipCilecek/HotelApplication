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
using System.Windows.Markup;
using Newtonsoft.Json;

namespace HotelApplication
{
    /// <summary>
    /// Interaction logic for PayControlWindow.xaml
    /// </summary>
    public partial class PayControlWindow : UserControl
    {
        StorageControlWindow storageFunction = new StorageControlWindow();

        public PayControlWindow()
        {
            InitializeComponent();
            FillCombobox_reservations();
            

            /*using (hotelDBEntities context = new hotelDBEntities())
            {
                Reservation reservation = context.Reservations.FirstOrDefault(r => r.id_re == 3);
                List<view_RoomReservations> listroomreservations = context.view_RoomReservations.ToList();

                string jsonoutput = JsonConvert.SerializeObject(listroomreservations, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore } );
                Console.WriteLine(jsonoutput);
                System.IO.File.WriteAllText(@"D:\jsontestLISTjson.json", jsonoutput);
                MessageBox.Show("DONE");
            }*/
            
        }

        int id_reservation = 0;
        int roomcount = 0;
        private void FillCombobox_reservations()
        {
            combo_choosereservation.Items.Clear();
            // Vyplnit combobox - jméno, příjmení
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Reservation> reservationlist = context.Reservations.ToList();
                foreach (Reservation reservation in reservationlist)
                {
                    if(reservation.ispayed == false)
                    {
                        combo_choosereservation.Items.Add(reservation.firstname + " " + reservation.lastname);
                    }
                }
            }
            //combo_choosereservation.SelectedIndex = 0;
        }

        private void FillCombobox_reservationproducts()
        {
            RestartGroupboxesAndClearCombos();
            // Vyplnit combobox - produkty podle lednice, která je v pokoji (poté se to z ní bude odebírat) - podle rezervace (firstname, lastname)

            using (hotelDBEntities context = new hotelDBEntities())
            {
                Reservation reservation = context.Reservations.FirstOrDefault(r => r.id_re == id_reservation); // Zjistím jaká je to rezervace
                List<Room> reservationrooms = reservation.Rooms.ToList(); // List pokojů v rezervaci
                int roomproductsadded = 0;
                foreach(Room room in reservationrooms)
                {
                    int id_fridge = room.Fridge.id_fr;
                    Fridge roomfridge = context.Fridges.FirstOrDefault(r => r.id_fr == id_fridge);

                    List<view_FridgeProductAmount> fridgeproductlist = context.view_FridgeProductAmount.ToList();

                    foreach (view_FridgeProductAmount fridgeproduct in fridgeproductlist)
                    {
                        
                        if((fridgeproduct.id_fr == id_fridge))
                        {
                            switch (roomproductsadded)
                            {
                                case 0:
                                    groupbox_room1.Visibility = Visibility.Visible;
                                    label_room1.Content = "Číslo pokoje : " + room.id_ro.ToString();
                                    combo_roomproducts1.Items.Add(fridgeproduct.name);
                                    combo_roomproducts1.SelectedIndex = 0;
                                    break;
                                case 1:
                                    groupbox_room2.Visibility = Visibility.Visible;
                                    label_room2.Content = "Číslo pokoje : " + room.id_ro.ToString();
                                    combo_roomproducts2.Items.Add(fridgeproduct.name);
                                    combo_roomproducts2.SelectedIndex = 0;
                                    break;
                                case 2:
                                    groupbox_room3.Visibility = Visibility.Visible;
                                    label_room3.Content = "Číslo pokoje : " + room.id_ro.ToString();
                                    combo_roomproducts3.Items.Add(fridgeproduct.name);
                                    combo_roomproducts3.SelectedIndex = 0;
                                    break;
                                case 3:
                                    groupbox_room4.Visibility = Visibility.Visible;
                                    label_room4.Content = "Číslo pokoje : " + room.id_ro.ToString();
                                    combo_roomproducts4.Items.Add(fridgeproduct.name);
                                    combo_roomproducts4.SelectedIndex = 0;
                                    break;
                                case 4:
                                    groupbox_room5.Visibility = Visibility.Visible;
                                    label_room5.Content = "Číslo pokoje : " + room.id_ro.ToString();
                                    combo_roomproducts5.Items.Add(fridgeproduct.name);
                                    combo_roomproducts5.SelectedIndex = 0;
                                    break;
                                case 5:
                                    groupbox_room6.Visibility = Visibility.Visible;
                                    label_room6.Content = "Číslo pokoje : " + room.id_ro.ToString();
                                    combo_roomproducts6.Items.Add(fridgeproduct.name);
                                    combo_roomproducts6.SelectedIndex = 0;
                                    break;
                                case 6:
                                    groupbox_room7.Visibility = Visibility.Visible;
                                    label_room7.Content = "Číslo pokoje : " + room.id_ro.ToString();
                                    combo_roomproducts7.Items.Add(fridgeproduct.name);
                                    combo_roomproducts7.SelectedIndex = 0;
                                    break;
                                case 7:
                                    groupbox_room8.Visibility = Visibility.Visible;
                                    label_room8.Content = "Číslo pokoje : " + room.id_ro.ToString();
                                    combo_roomproducts8.Items.Add(fridgeproduct.name);
                                    combo_roomproducts8.SelectedIndex = 0;
                                    break;
                            }
                        }
                    }
                    roomproductsadded++;
                }
                
                
            }
            
        }

        private void RestartGroupboxesAndClearCombos()
        {
            groupbox_room1.Visibility = Visibility.Hidden;
            groupbox_room2.Visibility = Visibility.Hidden;
            groupbox_room3.Visibility = Visibility.Hidden;
            groupbox_room4.Visibility = Visibility.Hidden;
            groupbox_room5.Visibility = Visibility.Hidden;
            groupbox_room6.Visibility = Visibility.Hidden;
            groupbox_room7.Visibility = Visibility.Hidden;
            groupbox_room8.Visibility = Visibility.Hidden;
            combo_roomproducts1.Items.Clear();
            combo_roomproducts2.Items.Clear();
            combo_roomproducts3.Items.Clear();
            combo_roomproducts4.Items.Clear();
            combo_roomproducts5.Items.Clear();
            combo_roomproducts6.Items.Clear();
            combo_roomproducts7.Items.Clear();
            combo_roomproducts8.Items.Clear();
        }

        private void Combo_choosereservation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                SetReservationID_and_RoomCount();
            }
        }

        private void SetReservationID_and_RoomCount()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<view_RoomReservations> roomreservationlist = context.view_RoomReservations.ToList();
                foreach (view_RoomReservations roomreservation in roomreservationlist)
                {
                    string name = roomreservation.firstname + " " + roomreservation.lastname;
                    if( name == combo_choosereservation.SelectedItem.ToString())
                    {
                        id_reservation = roomreservation.id_re;
                        Reservation res = context.Reservations.FirstOrDefault(r => r.id_re == id_reservation);
                        roomcount = res.Rooms.Count(); // Zpočítám kolik pokojů je v dané rezervaci

                        FillCombobox_reservationproducts();
                        return;
                    }
                }
            }
        }

        private Reservation GetReservationBy_FirstnameLastname(string firstname, string lastname)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Reservation reservation = context.Reservations.FirstOrDefault(r => (r.firstname == firstname) && (r.lastname == lastname));
                return reservation;
            }
        }

        private void Btn_pay_Click(object sender, RoutedEventArgs e)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Reservation reservation = context.Reservations.FirstOrDefault(r => r.id_re == id_reservation);

                List<view_RoomReservations> roomreservationlist = context.view_RoomReservations.ToList();
                foreach (view_RoomReservations roomreservation in roomreservationlist)
                {
                    if (roomreservation.id_re == id_reservation)
                    {
                        // Price_total + ( count_days(from-to) * price_per_day )
                        decimal priceperday = roomreservation.price_per_day.Value;
                        reservation.price_total = reservation.price_total + (CalculateDaysBetweenTwoDates(reservation.date_from, reservation.date_to) * priceperday);
                        reservation.ispayed = true;
                        context.SaveChanges();
                    }
                }
                // Uložím do JSON filu všechny zaplacené rezervace
                List<view_AllPayedReservations> payedreservationlist = context.view_AllPayedReservations.ToList();
                string jsonoutput = JsonConvert.SerializeObject(payedreservationlist, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
                Console.WriteLine(jsonoutput);
                System.IO.File.WriteAllText(@"D:\jsontestLISTjson.json", string.Empty);
                System.IO.File.WriteAllText(@"D:\jsontestLISTjson.json", jsonoutput);
                MessageBox.Show("Zaplaceno !");
            }
        }


        private void Btn_addProduct1_Click(object sender, RoutedEventArgs e)
        {
            int id_room = 0;
            string s = label_room1.Content.ToString();
            var separetedroomname = s.Split(' '); // Cislo0pokoje1:2CISLO3
            id_room = Convert.ToInt32(separetedroomname[3]);

            int id_fridge = 0;
            int id_product = 0;
            int amount = 0;

            using (hotelDBEntities context = new hotelDBEntities())
            {
                Room room = context.Rooms.FirstOrDefault(r => r.id_ro == id_room);
                id_fridge = room.Fridge.id_fr;

                Product product = context.Products.FirstOrDefault(r => r.name == combo_roomproducts1.SelectedItem.ToString());
                id_product = product.id_pr;
            }

            amount = Int32.Parse(txt_productamount1.Text);

            if(storageFunction.IsEnoughAmountInFridge(id_fridge,id_product,amount) == true)
            {
                string name = combo_choosereservation.SelectedItem.ToString();
                var nameseparated = name.Split(' ');
                string firstname = nameseparated[0];
                string lastname = nameseparated[1];

                using (hotelDBEntities context = new hotelDBEntities())
                {
                    Reservation reservation = context.Reservations.FirstOrDefault(r => r.id_re == id_reservation);
                    reservation.price_total = reservation.price_total + CalculateProductAmountPrices(id_product, amount);
                    context.SaveChanges();
                }

                storageFunction.DeleteFridgeProducts(id_fridge, id_product, amount);

                FillCombobox_reservations();

                MessageBox.Show("Úspěšně odepsáno z lednice a přidáno do zaplacení!");
            }
            else
            {
                MessageBox.Show("V lednici nebylo tolik produktů !");
                return;
            }
            
        }

        private decimal CalculateProductAmountPrices(int id_product, int amount)
        {
            decimal money = 0;

            using (hotelDBEntities context = new hotelDBEntities())
            {
                Product product = context.Products.FirstOrDefault(r => r.id_pr == id_product);
                money = product.price * amount;
            }

            return money;
        }

        private int CalculateDaysBetweenTwoDates(DateTime start, DateTime end)
        {
            var days = ((end - start).TotalDays);
            return (Convert.ToInt32(days) + 1) ;
        }
    }
}
