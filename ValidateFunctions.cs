using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace HotelApplication
{
    class ValidateFunctions
    {
        public void TestFunction()
        {
            MessageBox.Show("TEST FUNKCNOSTI VALIDATEFUNCTIONS");
        }

        public bool ValidateTexboxLettersOnly(TextBox txtbox)
        {
            return Regex.IsMatch(txtbox.Text, @"^[a-zA-Z]+$");
        }

        public bool ValidateTextboxNumbersOnly(TextBox txtbox)
        {
            string pattern = @"^[0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(txtbox.Text);
        }

        public bool ValidateTextboxDecimalOnly(TextBox txtbox)
        {
            string pattern = @"^[0-9]+([.,][0-9]{1,2})?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(txtbox.Text);
        }

        public bool ValidateTextboxLettersAndNumbersOnly(TextBox txtbox)
        {
            string pattern = @"^\w+([\s]\w+)*$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(txtbox.Text);
        }

        public bool ValidateAddress(TextBox txtbox)
        {
            string pattern = @"^[a-zA-Z0-9\/ ]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(txtbox.Text);
        }

        public bool ValidateEmail(TextBox txtbox)
        {
            try
            {
                MailAddress m = new MailAddress(txtbox.Text);
                return true;
            }
            catch (FormatException) { return false; }
        }
    }
}
