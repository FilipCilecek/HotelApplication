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
using System.Text.RegularExpressions;

namespace HotelApplication
{
    /// <summary>
    /// Interaction logic for AdminControlWindow.xaml
    /// </summary>
    public partial class AdminControlWindow : UserControl
    {
        ValidateFunctions validateFunctions = new ValidateFunctions();

        public AdminControlWindow()
        {
            InitializeComponent();

            FillCombobox_Productnames();
            FillCombobox_Storagenames();
            FillCombobox_Fridgenames();
            FillCombobox_Roomnames();
            FillCombobox_Attributenames();
            FillCombobox_Roomattributes();
        }

        private void Btn_addProduct_Click(object sender, RoutedEventArgs e)
        {
            if ((validateFunctions.ValidateTextboxLettersAndNumbersOnly(txt_productname) == true) && (validateFunctions.ValidateTextboxDecimalOnly(txt_productprice) == true) )
            {
                AddProductToDatabase(txt_productname.Text, TextboxText_To_Decimal(txt_productprice));
            }
            else { MessageBox.Show("Špatně zadané údaje !"); }
        }

        private void AddProductToDatabase(string name, decimal price)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                context.Products.Add(new Product { name = name, price = price });
                context.SaveChanges();
                MessageBox.Show("Produkt přidán do databáze !");
                txt_productname.Text = String.Empty;
                txt_productprice.Text = String.Empty;
                FillCombobox_Productnames();
            }
        }

        private decimal TextboxText_To_Decimal(TextBox txtbox)
        {
            string decimalString = txtbox.Text.Replace(".", ","); // Musím změnit tečku za čárku, jinak nejde zkonvertovat
            decimal price = Convert.ToDecimal(decimalString);
            return price;
        }

        private void DeleteProductFromDatabase()
        {
            using(hotelDBEntities context = new hotelDBEntities())
            {
                Product product = context.Products.FirstOrDefault(r => r.name == combo_productnames.Text);
                context.Products.Remove(product);
                context.SaveChanges();
                MessageBox.Show("Produkt odstraněn z databáze !");
            }
            FillCombobox_Productnames();
        }

        private void FillCombobox_Productnames()
        {
            combo_productnames.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Product> productlist = context.Products.ToList();
                foreach (Product product in productlist)
                {
                    combo_productnames.Items.Add(product.name);
                }
            }
            combo_productnames.SelectedIndex = 0;
        }

        private void RegexPatternCreate(string patternparameter)
        {
            string pattern = @""+patternparameter+"";
            Regex regex = new Regex(pattern);
        }

        private void Btn_deleteProduct_Click(object sender, RoutedEventArgs e)
        {
            DeleteProductFromDatabase();
        }

        private void Btn_addStorage_Click(object sender, RoutedEventArgs e)
        {
            if ((validateFunctions.ValidateTextboxLettersAndNumbersOnly(txt_storagename) == true))
            {
                AddStorageToDatabase(txt_storagename.Text);
            }
            else { MessageBox.Show("Špatně zadaný název skladu !"); }
        }

        private void AddStorageToDatabase(string name)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                context.Storages.Add(new Storage { name = txt_storagename.Text });
                context.SaveChanges();
                MessageBox.Show("Sklad přidán do databáze !");
                txt_storagename.Text = String.Empty;
                FillCombobox_Storagenames();
            }
        }

        private void FillCombobox_Storagenames()
        {
            combo_storagenames.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Storage> storagelist = context.Storages.ToList();
                foreach (Storage storage in storagelist)
                {
                    combo_storagenames.Items.Add(storage.name);
                }
            }
            combo_storagenames.SelectedIndex = 0;
        }

        private void Btn_deleteStorage_Click(object sender, RoutedEventArgs e)
        {
            DeleteStorageFromDatabase();
        }

        private void DeleteStorageFromDatabase()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Storage storage = context.Storages.FirstOrDefault(r => r.name == combo_storagenames.Text);
                context.Storages.Remove(storage);
                context.SaveChanges();
                MessageBox.Show("Sklad odstraněn z databáze !");
            }
            FillCombobox_Storagenames();
        }

        private void Btn_addFridge_Click(object sender, RoutedEventArgs e)
        {
            
            if ((validateFunctions.ValidateTextboxLettersAndNumbersOnly(txt_fridgename) == true))
            {
                AddFridgeToDatabase(txt_fridgename.Text);
            }
            else { MessageBox.Show("Špatně zadaný název lednice !"); }
        }

        private void AddFridgeToDatabase(string name)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                context.Fridges.Add(new Fridge { name = txt_fridgename.Text });
                context.SaveChanges();
                MessageBox.Show("Lednice přidána do databáze !");
                txt_fridgename.Text = String.Empty;
                FillCombobox_Fridgenames();
            }
        }

        private void FillCombobox_Fridgenames()
        {
            combo_fridgenames.Items.Clear();
            combo_roomfridgename.Items.Clear();
            combo_4changefridge_choosefridge.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Room> roomlist = context.Rooms.ToList();
                List<int> roomfridgeids = new List<int>();
                foreach(Room room in roomlist)
                {
                    roomfridgeids.Add(room.id_fr);
                }

                List<Fridge> fridgelist = context.Fridges.ToList();
                foreach (Fridge fridge in fridgelist)
                {
                    if (roomfridgeids.Contains(fridge.id_fr))
                    {
                        continue;
                    }
                    else
                    {
                        combo_fridgenames.Items.Add(fridge.name);
                        combo_roomfridgename.Items.Add(fridge.name);
                        combo_4changefridge_choosefridge.Items.Add(fridge.name);
                    }
                }
            }
            combo_fridgenames.SelectedIndex = 0;
            combo_roomfridgename.SelectedIndex = 0;
            combo_4changefridge_choosefridge.SelectedIndex = 0;
        }

        private void Btn_deleteFridge_Click(object sender, RoutedEventArgs e)
        {
            DeleteFridgeFromDatabase();
        }

        private void DeleteFridgeFromDatabase()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                int id_fridge = 0;
                Fridge selectedfridge = context.Fridges.FirstOrDefault(r => r.name == combo_fridgenames.Text);
                id_fridge = selectedfridge.id_fr;

                List<Room> roomlist = context.Rooms.ToList();
                foreach (Room room in roomlist)
                {
                    if (room.id_fr == id_fridge)
                    {
                        MessageBox.Show("Lednice je používána v pokoji : " + room.id_ro.ToString() + "\nPrvní lednici zaměň za jinčí !");
                        return;
                    }
                }

                Fridge fridge = context.Fridges.FirstOrDefault(r => r.name == combo_fridgenames.Text);
                context.Fridges.Remove(fridge);
                context.SaveChanges();
                MessageBox.Show("Lednice odstraněna z databáze !");

                FillCombobox_Fridgenames();
            }

        }

        private void Btn_addRoom_Click(object sender, RoutedEventArgs e)
        {
            if ((validateFunctions.ValidateTextboxNumbersOnly(txt_roombeds) == true) && (validateFunctions.ValidateTextboxDecimalOnly(txt_roompriceperday) == true))
            {
                AddRoomToDatabase(Int32.Parse(txt_roombeds.Text), TextboxText_To_Decimal(txt_roompriceperday), combo_roomfridgename.Text);
            }
            else { MessageBox.Show("Špatně zadané údaje pokoje !"); }   
        }

        private void AddRoomToDatabase(int beds, decimal price, string fridgename)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Fridge fridge = context.Fridges.FirstOrDefault(r => r.name == fridgename);

                context.Rooms.Add(new Room { beds = beds , price_per_day = price , id_fr = fridge.id_fr });
                context.SaveChanges();
                MessageBox.Show("Pokoj přidán do databáze !");
                txt_roombeds.Text = String.Empty;
                txt_roompriceperday.Text = String.Empty;
                FillCombobox_Roomnames();
            }
        }

        private void FillCombobox_Roomnames()
        {
            combo_roomnames.Items.Clear();
            combo_6chooseroomid.Items.Clear();
            combo_6deleteroomid.Items.Clear();
            combo_4changefridge_chooseroom.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Room> roomlist = context.Rooms.ToList();
                foreach (Room room in roomlist)
                {
                    combo_roomnames.Items.Add(room.id_ro);
                    combo_6chooseroomid.Items.Add(room.id_ro);
                    combo_6deleteroomid.Items.Add(room.id_ro);
                    combo_4changefridge_chooseroom.Items.Add(room.id_ro);
                }
            }
            combo_roomnames.SelectedIndex = 0;
            combo_6chooseroomid.SelectedIndex = 0;
            combo_6deleteroomid.SelectedIndex = 0;
        }

        private void Btn_deleteRoom_Click(object sender, RoutedEventArgs e)
        {
            DeleteRoomFromDatabase();
        }

        private void DeleteRoomFromDatabase()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                int id_room = Int32.Parse(combo_roomnames.Text);
                Room room = context.Rooms.FirstOrDefault(r => r.id_ro == id_room);
                context.Rooms.Remove(room);
                context.SaveChanges();
                MessageBox.Show("Pokoj odstraněn z databáze !");
            }
            FillCombobox_Roomnames();
        }

        private void Btn_addAttribute_Click(object sender, RoutedEventArgs e)
        {
            if ((validateFunctions.ValidateTextboxLettersAndNumbersOnly(txt_attributename) == true))
            {
                AddAttributeToDatabase(txt_attributename.Text);
            }
            else { MessageBox.Show("Špatně zadaný název skladu !"); }
        }

        private void AddAttributeToDatabase(string attribute)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                context.Attributes.Add(new Attribute { name = attribute });
                context.SaveChanges();
                MessageBox.Show("Vlastnost přidána do databáze !");
                txt_attributename.Text = String.Empty;
                FillCombobox_Attributenames();
            }
        }

        private void FillCombobox_Attributenames()
        {
            combo_attributenames.Items.Clear();
            combo_6roomattributes.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Attribute> attributelist = context.Attributes.ToList();
                foreach (Attribute attribute in attributelist)
                {
                    combo_attributenames.Items.Add(attribute.name);
                    combo_6roomattributes.Items.Add(attribute.name);
                }
            }
            combo_attributenames.SelectedIndex = 0;
            combo_6roomattributes.SelectedIndex = 0;
        }

        private void Btn_deleteAttribute_Click(object sender, RoutedEventArgs e)
        {
            DeleteAttributeFromDatabase();
        }

        private void DeleteAttributeFromDatabase()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Attribute attribute = context.Attributes.FirstOrDefault(r => r.name == combo_attributenames.Text);
                context.Attributes.Remove(attribute);
                context.SaveChanges();
                MessageBox.Show("Vlastnost odstraněna z databáze !");
            }
            FillCombobox_Attributenames();
        }

        private void Btn_addAttributeToRoom_Click(object sender, RoutedEventArgs e)
        {
            AddAttributeToRoom();
        }

        private void AddAttributeToRoom()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                int roomid = Convert.ToInt32(combo_6chooseroomid.Text);
                Room selectedroom = context.Rooms.FirstOrDefault(r => r.id_ro == roomid);
                Attribute selectedattribute = context.Attributes.FirstOrDefault(r => r.name == combo_6roomattributes.Text);
                selectedroom.Attributes.Add(selectedattribute);
                context.SaveChanges();
                MessageBox.Show("Vlastnost přidána pokoji !");
                FillCombobox_Roomattributes();
            }
        }

        private void Btn_deleteAttributeFromRoom_Click(object sender, RoutedEventArgs e)
        {
            DeleteAttributeFromRoom();
        }

        private void DeleteAttributeFromRoom()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                int roomid = Convert.ToInt32(combo_6deleteroomid.Text);
                Room selectedroom = context.Rooms.FirstOrDefault(r => r.id_ro == roomid);
                Attribute selectedattribute = context.Attributes.FirstOrDefault(r => r.name == combo_6deleteroomattributes.Text);
                selectedroom.Attributes.Remove(selectedattribute);
                context.SaveChanges();
                MessageBox.Show("Vlastnost odebrána pokoji !");
                FillCombobox_Roomattributes();
            }
        }

        private void FillCombobox_Roomattributes()
        {
            if(combo_6deleteroomid.SelectedItem == null) { return; }
            combo_6deleteroomattributes.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                int roomid = Convert.ToInt32(combo_6deleteroomid.SelectedItem.ToString());
                Room room = context.Rooms.FirstOrDefault(r => r.id_ro == roomid);

                List<Attribute> attributelist = room.Attributes.ToList();
                foreach (Attribute attribute in attributelist)
                {
                    combo_6deleteroomattributes.Items.Add(attribute.name);
                }
            }
            combo_6deleteroomattributes.SelectedIndex = 0;
        }

        private void Combo_6deleteroomid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                FillCombobox_Roomattributes();
            }

        }

        private void Btn_changeFridge_Click(object sender, RoutedEventArgs e)
        {
            if ((combo_4changefridge_chooseroom.SelectedItem != null) && (combo_4changefridge_chooseroom.SelectedItem != null))
            {
                int id_room = 0;
                using (hotelDBEntities context = new hotelDBEntities())
                {
                    id_room = Convert.ToInt32(combo_4changefridge_chooseroom.SelectedItem);
                    Room room = context.Rooms.FirstOrDefault(r => r.id_ro == id_room);
                    Fridge fridge = context.Fridges.FirstOrDefault(r => r.name == combo_4changefridge_choosefridge.SelectedItem.ToString());

                    room.Fridge = fridge;
                    MessageBox.Show("Lednice změněna !");
                    context.SaveChanges();

                    FillCombobox_Roomnames();
                    FillCombobox_Fridgenames();
                }
            }
            else
            {
                MessageBox.Show("Vyber pokoj a lednici pro změnu !");
            }
        }

        private void Combo_4changefridge_chooseroom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                if(combo_4changefridge_chooseroom.SelectedItem != null)
                {
                    int id_fridge = 0;
                    id_fridge = Convert.ToInt32(combo_4changefridge_chooseroom.SelectedItem);
                    using (hotelDBEntities context = new hotelDBEntities())
                    {
                        Room room = context.Rooms.FirstOrDefault(r => r.id_ro == id_fridge);
                        id_fridge = room.id_fr;
                    }
                    FillCombobox_Fridgenames();
                    //FillCombobox_FridgenamesWithoutRoomFridge(id_fridge);
                }
                else
                {
                    return;
                }
            }
        }

        private void FillCombobox_FridgenamesWithoutRoomFridge(int id_fridge)
        {
            combo_4changefridge_choosefridge.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Fridge> fridgelist = context.Fridges.ToList();
                foreach (Fridge fridge in fridgelist)
                {
                    if(fridge.id_fr == id_fridge)
                    {
                        label_4currentfridge.Content = "Má : " + fridge.name;
                        continue;
                    }
                    combo_4changefridge_choosefridge.Items.Add(fridge.name);
                }
            }
            combo_4changefridge_choosefridge.SelectedIndex = 0;
        }
    }
}
