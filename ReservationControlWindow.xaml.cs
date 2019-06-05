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

namespace HotelApplication
{
    /// <summary>
    /// Interaction logic for ReservationControlWindow.xaml
    /// </summary>
    public partial class ReservationControlWindow : UserControl
    {
        int roomcount = 1; //max 8
        ValidateFunctions validateFunctions = new ValidateFunctions();


        public ReservationControlWindow()
        {
            InitializeComponent();
            FillCombobox_Roomnames();
        }

        private void FillCombobox_Roomnames()
        {
            combo_chooseroom.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Room> roomlist = context.Rooms.ToList();
                foreach (Room room in roomlist)
                {
                    combo_chooseroom.Items.Add("Pokoj č.: " + room.id_ro + ", počet postelí: " + room.beds);
                }
            }
        }

        private void Btn_addreservationroomcombo_Click(object sender, RoutedEventArgs e)
        {
            roomcount++;
            if (roomcount >= 9) { return; }
            groupbox_reservation.Height = groupbox_reservation.Height + 40;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<ComboBox xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns:materialDesign='http://materialdesigninxaml.net/winfx/xaml/themes' ");
            sb.Append(@"materialDesign:HintAssist.Hint='Pokoj " + roomcount + "' Style='{ StaticResource MaterialDesignFloatingHintComboBox}' />");
            ComboBox newcombo = (ComboBox)XamlReader.Parse(sb.ToString());
            newcombo.Name = "combo_rooms" + roomcount;
            newcombo.SelectedIndex = 0;

            using (hotelDBEntities context = new hotelDBEntities())
            {
                var roomlist = context.Rooms.ToList();
                newcombo.Items.Clear(); // Clearnu a vyplním combobox s výběrem, aby nešlo vybrat pokoj, který je v tento datum zabraný
                foreach (Room room in roomlist)
                {
                    if (!reserved_id_ro.Contains(room.id_ro)) // Pokuď list již rezervovaných pokojů neobsahuje pokoj -> přidej ho do datagridu
                    {
                        newcombo.Items.Add(room.id_ro);
                    }
                }
            }
            stackpanel_morerooms.Children.Add(newcombo);
        }

        private void Btn_addReservation_Click(object sender, RoutedEventArgs e)
        {
            if((validateFunctions.ValidateTexboxLettersOnly(txt_firstname) == true)  && (validateFunctions.ValidateTexboxLettersOnly(txt_lastname) == true) && (validateFunctions.ValidateTextboxDecimalOnly(txt_phone) == true) && (validateFunctions.ValidateEmail(txt_email) == true) && (validateFunctions.ValidateAddress(txt_adress) == true))
            {
                //MessageBox.Show("DOBRE zadané údaje !");
                AddReservationToDatabase();
            }
            else { MessageBox.Show("Špatně zadané údaje !"); }
        }

        private void AddReservationToDatabase()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                DateTime datefrom = date_reservationfrom.SelectedDate.Value.Date;
                DateTime dateto = date_reservationto.SelectedDate.Value.Date;
                Reservation newreservation = context.Reservations.Add(new Reservation { firstname = txt_firstname.Text, lastname = txt_lastname.Text, phone = txt_phone.Text, email = txt_email.Text, address = txt_adress.Text, date_from = datefrom, date_to = dateto });

                List<Room> roomlist = new List<Room>(); // Prázdný list roomů
                List<ComboBox> comboboxlist = new List<ComboBox>(); // Vyplním list comboboxů
                int roomid = Convert.ToInt32(combo_rooms1.Text);
                Room selectedroom = context.Rooms.FirstOrDefault(r => r.id_ro == roomid);
                roomlist.Add(selectedroom); // První room, který musí být ... nemůže být rezervace bez pokoje. Může být však jen jeden
                foreach (ComboBox combobox in stackpanel_morerooms.Children) // Pro každý další combobox v stackpanelu s dynamickým přidáváním projedu a přidám pokoj do roomlistu
                {
                    int anotherroomid = Convert.ToInt32(combobox.Text);
                    Room anotherroom = context.Rooms.FirstOrDefault(r => r.id_ro == anotherroomid);
                    roomlist.Add(anotherroom);
                }
                foreach (Room room in roomlist) // Přidám každý room v roomlistu do dané rezervace
                {
                    newreservation.Rooms.Add(room);
                }
                context.SaveChanges();
                MessageBox.Show("Rezervace přidána do databáze !");
                EmptyAllToolboxesInReservation();
            }
        }

        private void EmptyAllToolboxesInReservation()
        {
            txt_firstname.Text = String.Empty;
            txt_lastname.Text = String.Empty;
            txt_phone.Text = String.Empty;
            txt_email.Text = "@";
            txt_adress.Text = String.Empty;
            date_reservationfrom.SelectedDate = DateTime.Now;
            date_reservationto.SelectedDate = DateTime.Now;
        }

        private void Combo_chooseroom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                FillDatagrid_Selectedroominfo();
            }

        }

        private void FillDatagrid_Selectedroominfo()
        {
            datagrid_rooms.Items.Clear();
            string combostring = combo_chooseroom.SelectedItem.ToString();
            char combochar = combostring[10];
            int selected_id_ro = int.Parse(combochar.ToString());
            using (hotelDBEntities context = new hotelDBEntities())
            {
                var roomreservationslist = context.view_RoomReservations.ToList();
                foreach (view_RoomReservations roomreservations in roomreservationslist)
                {
                    if(roomreservations.id_ro == selected_id_ro)
                    {
                        DateTime datetime_from = roomreservations.date_from ?? DateTime.Now; // Datum rezervace z databáze (date_from)
                        DateTime datetime_to = roomreservations.date_to ?? DateTime.Now;   // Datum rezervace z databáze (date_to)
                        string formateddate_from = datetime_from.ToString("dd/MM/yyyy");
                        string formateddate_to = datetime_to.ToString("dd/MM/yyyy");
                        var formateddates = new TwoDates { ClassDateFrom = formateddate_from, ClassDateTo = formateddate_to }; 
                        datagrid_rooms.Items.Add(formateddates);
                    }
                }
            }
        }

        private void Date_reservationfrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                if (date_reservationfrom.SelectedDate.Value > date_reservationto.SelectedDate.Value)
                {
                    if (date_reservationfrom.SelectedDate != DateTime.Now)
                    {
                        MessageBox.Show("Datum začátku rezervace nemůže být po konci rezervace !");
                    }
                    date_reservationfrom.SelectedDate = DateTime.Now;
                    date_reservationto.SelectedDate = DateTime.Now;
                    return;
                }
                FillDatagrid_Roomswithoutreservation();
            }
        }

        List<int> reserved_id_ro = new List<int>();
        private void FillDatagrid_Roomswithoutreservation()
        {
            
            if (datagrid_reservationdays.HasItems)
            {
                datagrid_reservationdays.Items.Clear();
            }

            DateTime pickerdate1 = date_reservationfrom.SelectedDate.Value; // Datepicked "Rezervace od" - do DateTime
            DateTime pickerdate2 = date_reservationto.SelectedDate.Value;   // Datepicked "Rezervace do" - do DateTime

            using (hotelDBEntities context = new hotelDBEntities())
            {
                var roomreservationslist = context.view_RoomReservations.ToList();
                

                foreach (view_RoomReservations roomreservations in roomreservationslist)
                {
                    DateTime datetime1 = roomreservations.date_from ?? DateTime.Now; // Datum rezervace z databáze (date_from)
                    DateTime datetime2 = roomreservations.date_to ?? DateTime.Now;   // Datum rezervace z databáze (date_to)
                    var datesrooms = DaysBetweenTwoDatesList(datetime1, datetime2);

                    foreach (var date in datesrooms)
                    {
                        if (reserved_id_ro.Contains(roomreservations.id_ro)) // Pokuď pokoj již byl rezervován v jinčí rezervaci na toto datum -> pokračuj
                        {
                            continue;
                        }
                        if ((pickerdate1 <= date) && (date <= pickerdate2)) // Pokuď se datum nachází mezi vybranými datumy rezervace -> přidej do seznamu pokojů, které nechceme
                        {
                            reserved_id_ro.Add(roomreservations.id_ro);
                        }
                    }
                }
                var roomlist = context.Rooms.ToList();
                combo_rooms1.Items.Clear(); // Clearnu a vyplním combobox s výběrem, aby nešlo vybrat pokoj, který je v tento datum zabraný
                foreach (Room room in roomlist)
                {
                    if (!reserved_id_ro.Contains(room.id_ro)) // Pokuď list již rezervovaných pokojů neobsahuje pokoj -> přidej ho do datagridu
                    {
                        datagrid_reservationdays.Items.Add(room);
                        combo_rooms1.Items.Add(room.id_ro);
                    }
                }
            }
        }

        private List<DateTime> DaysBetweenTwoDatesList(DateTime date1, DateTime date2)
        {
            var datesbetween = new List<DateTime>();
            for (var dt = date1; dt <= date2; dt = dt.AddDays(1))
            {
                datesbetween.Add(dt);
            }
            return datesbetween;
        }

        private void Date_reservationto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                if (date_reservationfrom.SelectedDate.Value > date_reservationto.SelectedDate.Value)
                {
                    if (date_reservationto.SelectedDate != DateTime.Now)
                    {
                        MessageBox.Show("Datum začátku rezervace nemůže být po konci rezervace !");
                    }
                    date_reservationto.SelectedDate = DateTime.Now;
                    date_reservationfrom.SelectedDate = DateTime.Now;
                    return;
                }
                FillDatagrid_Roomswithoutreservation();
            }
        }

    }

    public class TwoDates
    {
        public string ClassDateFrom { get; set; }
        public string ClassDateTo { get; set; }
    }
}
