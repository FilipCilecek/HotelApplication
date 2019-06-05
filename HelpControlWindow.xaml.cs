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
using System.Net;
using System.Net.Mail;

namespace HotelApplication
{
    /// <summary>
    /// Interaction logic for HelpControlWindow.xaml
    /// </summary>
    public partial class HelpControlWindow : UserControl
    {
        public HelpControlWindow()
        {
            InitializeComponent();
        }

        private void Btn_sendEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential("metholiahelp@gmail.com", "helpmathi");

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("metholiahelp@gmail.com"),
                    Subject = txt_emailsubject.Text,
                    Body = "E-mail odesílatele : " + txt_youremail.Text + "\n\n" + txt_emailmessage.Text
                };

                mail.To.Add(new MailAddress("metholiahelp@gmail.com"));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };

                client.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR : " + ex.Message);
                return;
            }

            MessageBox.Show("E-mail na podporu odeslán !");
            ResetTxtboxes();
        }

        private void ResetTxtboxes()
        {
            txt_youremail.Text = "@";
            txt_emailsubject.Text = String.Empty;
            txt_emailmessage.Text = String.Empty;
        }
    }
}
