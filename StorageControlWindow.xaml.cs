using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StorageControlWindow.xaml
    /// </summary>
    public partial class StorageControlWindow : UserControl
    {
        ValidateFunctions validateFunctions = new ValidateFunctions();

        public StorageControlWindow()
        {
            InitializeComponent();
            FillCombobox_Productnames();
            FillCombobox_Storagenames();
            FillCombobox_Fridgenames();
            FillDatagrid_Storage();
            FillDatagrid_Fridge();
        }

        private void FillDatagrid_Fridge()
        {
            datagrid_fridge.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<view_FridgeProductAmount> fridgeproductlist = context.view_FridgeProductAmount.ToList();

                List<Fridge> fridgelist = context.Fridges.ToList();
                Fridge fridge = fridgelist.FirstOrDefault(r => r.name == combo_gridfridges.SelectedItem.ToString());

                foreach (view_FridgeProductAmount fridgeproduct in fridgeproductlist)
                {
                    if (fridgeproduct.id_fr == fridge.id_fr)
                    {
                        datagrid_fridge.Items.Add(fridgeproduct);
                    }
                }
            }
        }

        private void FillDatagrid_Storage()
        {
            datagrid_storage.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<view_StorageProductAmount> storageproductlist = context.view_StorageProductAmount.ToList();

                List<Storage> storagelist = context.Storages.ToList();
                Storage storage = storagelist.FirstOrDefault(r => r.name == combo_gridstorages.SelectedItem.ToString());

                foreach(view_StorageProductAmount storageproduct in storageproductlist)
                {
                    if(storageproduct.id_st == storage.id_st)
                    {
                        datagrid_storage.Items.Add(storageproduct);
                    }
                }
            }
        }

        private void FillCombobox_Fridgenames()
        {
            combo_fridges.Items.Clear();
            combo_gridfridges.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Fridge> fridgelist = context.Fridges.ToList();
                foreach (Fridge fridge in fridgelist)
                {
                    combo_fridges.Items.Add(fridge.name);
                    combo_gridfridges.Items.Add(fridge.name);
                }
            }
            combo_fridges.SelectedIndex = 0;
            combo_gridfridges.SelectedIndex = 0;
        }

        private void FillCombobox_Storagenames()
        {
            combo_storages.Items.Clear();
            combo_gridstorages.Items.Clear();
            combo_storagenames_tostorage.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Storage> storagelist = context.Storages.ToList();
                foreach (Storage storage in storagelist)
                {
                    combo_storages.Items.Add(storage.name);
                    combo_gridstorages.Items.Add(storage.name);
                    combo_storagenames_tostorage.Items.Add(storage.name);
                }
            }
            combo_storages.SelectedIndex = 0;
            combo_gridstorages.SelectedIndex = 0;
            combo_storagenames_tostorage.SelectedIndex = 0;
        }

        private void FillCombobox_Productnames()
        {
            combo_productnames_tostorage.Items.Clear();
            combo_products.Items.Clear();
            using (hotelDBEntities context = new hotelDBEntities())
            {
                List<Product> productlist = context.Products.ToList();
                foreach (Product product in productlist)
                {
                    combo_productnames_tostorage.Items.Add(product.name);
                    combo_products.Items.Add(product.name);
                }
            }
            combo_productnames_tostorage.SelectedIndex = 0;
            combo_products.SelectedIndex = 0;
        }


        private void Btn_addProduct_Click(object sender, RoutedEventArgs e)
        {
            if ((validateFunctions.ValidateTextboxLettersAndNumbersOnly(txt_productname) == true) && (validateFunctions.ValidateTextboxDecimalOnly(txt_productprice) == true))
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

        private void Btn_addProductsToStorage_Click(object sender, RoutedEventArgs e)
        {
            if (validateFunctions.ValidateTextboxNumbersOnly(txt_howmuchproducts_tostorage) == true)
            {
                AddProductsToStorage();
                FillDatagrid_Storage();
            }
            else{ MessageBox.Show("Špatně zadán počet kusů !"); }
            
        }

        private void AddProductsToStorage()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                int id_product = GetProductIdByComboboxText(combo_productnames_tostorage);
                int id_storage = GetStorageIdByComboboxText(combo_storagenames_tostorage);
                if(DoesProductInStorageExists(id_product, id_storage) == true)
                {
                    UpdateStorageProducts(id_product, id_storage, Int32.Parse(txt_howmuchproducts_tostorage.Text));
                }
                else
                {
                    context.Storage_Product.Add(new Storage_Product { id_pr = id_product, id_st = id_storage, amount = Int32.Parse(txt_howmuchproducts_tostorage.Text) });
                    context.SaveChanges();
                }
                MessageBox.Show("Produkty přidány do skladu !");
            }
        }

        private bool DoesProductInStorageExists(int id_product, int id_storage)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Storage_Product product_storage = context.Storage_Product.FirstOrDefault(r => (r.id_pr == id_product) && (r.id_st == id_storage));
                if (product_storage != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void UpdateStorageProducts(int id_product, int id_storage, int amount)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Storage_Product product_storage = context.Storage_Product.FirstOrDefault(r => (r.id_pr == id_product) && (r.id_st == id_storage));
                int? currentamount = product_storage.amount;
                MessageBox.Show("BYLO : " + currentamount.ToString());
                currentamount = currentamount + amount;
                MessageBox.Show("PO PRIDANI : " + currentamount.ToString());
                product_storage.amount = currentamount;
                context.SaveChanges();
                MessageBox.Show("OK PRIDANO");
            }
        }

        private int GetProductIdByComboboxText(ComboBox combo)
        {
            int id_pr = 0;
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Product product = context.Products.FirstOrDefault(r => r.name == combo.Text);
                id_pr = product.id_pr;
            }
            return id_pr;
        }

        private int GetStorageIdByComboboxText(ComboBox combo)
        {
            int id_st = 0;
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Storage storage = context.Storages.FirstOrDefault(r => r.name == combo.Text);
                id_st = storage.id_st;
            }
            return id_st;
        }

        private int GetFridgeIdByComboboxText(ComboBox combo)
        {
            int id_fr = 0;
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Fridge fridge = context.Fridges.FirstOrDefault(r => r.name == combo.Text);
                id_fr = fridge.id_fr;
            }
            return id_fr;
        }

        private void Btn_ProductsStorageToFridge_Click(object sender, RoutedEventArgs e)
        {
            if (validateFunctions.ValidateTextboxNumbersOnly(txt_howmuchproducts_tofridge) == true)
            {
                AddProductsFromStorageToFridge();
                FillDatagrid_Storage();
            }
            else { MessageBox.Show("Špatně zadán počet kusů !"); }
                
        }

        private void AddProductsFromStorageToFridge()
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                int id_storage = GetStorageIdByComboboxText(combo_storages); 
                int id_product = GetProductIdByComboboxText(combo_products); 
                int id_fridge = GetFridgeIdByComboboxText(combo_fridges);
                int amount = Int32.Parse(txt_howmuchproducts_tofridge.Text);
                if (IsEnoughAmountInStorage(id_storage, id_product, amount) == true)
                {
                    if(DoesProductInFridgeExists(id_fridge, id_product) == true)
                    {
                        DeleteStorageProductAmount(id_storage, id_product, amount); // Smazat počet produktů ze skladu ("přesunutí") OK
                        UpdateFridgeProducts(id_fridge, id_product, amount); // Pokud záznam existuje, updatuju (+amount)
                    }
                    else
                    {
                        DeleteStorageProductAmount(id_storage, id_product, amount); // Smazat počet produktů ze skladu ("přesunutí") OK
                        context.Product_Fridge.Add(new Product_Fridge { id_pr = id_product, id_fr = id_fridge, amount = amount }); // Pokud záznam neexistuje, vytvořím s daným počtem
                    }
                    context.SaveChanges();
                    MessageBox.Show("Produkty přesunuty ze skladu do lednice !");
                }
                else
                {
                    MessageBox.Show("Nedostatek produktů na přesunutí !");
                }
            }
        }

        private void DeleteStorageProductAmount(int id_storage, int id_product, int amount)
        {
                using (hotelDBEntities context = new hotelDBEntities())
                {
                    Storage_Product storage_product = context.Storage_Product.FirstOrDefault(r => (r.id_st == id_storage) && (r.id_pr == id_product));
                    int? currentamount = storage_product.amount;
                    currentamount = currentamount - amount;
                    storage_product.amount = currentamount;
                    context.SaveChanges();
                }
        }

        private bool DoesProductInFridgeExists(int id_fridge, int id_product)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Product_Fridge fridge_product = context.Product_Fridge.FirstOrDefault(r => (r.id_fr == id_fridge) && (r.id_pr == id_product));
                if (fridge_product != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public void UpdateFridgeProducts(int id_fridge, int id_product, int amount)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Product_Fridge fridge_product = context.Product_Fridge.FirstOrDefault(r => (r.id_fr == id_fridge) && (r.id_pr == id_product));
                int? currentamount = fridge_product.amount;
                currentamount = currentamount + amount;
                fridge_product.amount = currentamount;
                context.SaveChanges();
            }
        }

        public void DeleteFridgeProducts(int id_fridge, int id_product, int amount)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Product_Fridge fridge_product = context.Product_Fridge.FirstOrDefault(r => (r.id_fr == id_fridge) && (r.id_pr == id_product));
                int? currentamount = fridge_product.amount;
                currentamount = currentamount - amount;
                fridge_product.amount = currentamount;
                context.SaveChanges();
            }
        }

        private bool IsEnoughAmountInStorage(int id_storage, int id_product, int amount)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Storage_Product storage_product = context.Storage_Product.FirstOrDefault(r => (r.id_st == id_storage) && (r.id_pr == id_product));
                if( storage_product == null) // Pokud produkt neexistuje
                {
                    return false;
                }
                int? currentamount = storage_product.amount; // Pokud existuje, zkontruji zda je dostatek amountu
                if(currentamount >= amount)
                {
                    return true;
                }
                else { return false; }
            }
        }

        public bool IsEnoughAmountInFridge(int id_fridge, int id_product, int amount)
        {
            using (hotelDBEntities context = new hotelDBEntities())
            {
                Product_Fridge fridge_product = context.Product_Fridge.FirstOrDefault(r => (r.id_fr == id_fridge) && (r.id_pr == id_product));
                if (fridge_product == null) // Pokud produkt neexistuje
                {
                    return false;
                }
                int? currentamount = fridge_product.amount; // Pokud existuje, zkontruji zda je dostatek amountu
                if (currentamount >= amount)
                {
                    return true;
                }
                else { return false; }
            }
        }

        private decimal TextboxText_To_Decimal(TextBox txtbox)
        {
            string decimalString = txtbox.Text.Replace(".", ","); // Musím změnit tečku za čárku, jinak nejde zkonvertovat
            decimal price = Convert.ToDecimal(decimalString);
            return price;
        }

        private void RegexPatternCreate(string patternparameter)
        {
            string pattern = @"" + patternparameter + "";
            Regex regex = new Regex(pattern);
        }

        private void Combo_gridstorages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.IsLoaded == true)
            {
                FillDatagrid_Storage();
            }
        }

        private void Combo_gridfridges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded == true)
            {
                FillDatagrid_Fridge();
            }
        }
    }
}
