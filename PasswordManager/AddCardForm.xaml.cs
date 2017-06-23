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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace PasswordManager
{
    /// <summary>
    /// Логика взаимодействия для AddCardForm.xaml
    /// </summary>
    public partial class AddCardForm : Window
    {
        BufferCard Card = new BufferCard();

        public AddCardForm(LoginCard Card)
        {
            InitializeComponent();
            Login.Text = Card.Login;
            Password.Text = Card.Password;
            Site.Text = Card.SiteURL;
            Logo.Source = new BitmapImage(new Uri(Card.PictureURL, UriKind.Relative));
        }

        public class BufferCard
        {
           
            public void ApplyImage(MouseButtonEventArgs e)
            {
               

               OpenFileDialog op = new OpenFileDialog()
                {
                    Title = "Выберите изображение",
                    Filter = "Поддерживаемые форматы|*.jpg;*.jpeg;*.png|" +
           "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
           "Portable Network Graphic (*.png)|*.png"
                };
                if (op.ShowDialog() == true)
                {
                    var img = (Image)e.Source;
                    img.Source = new BitmapImage(new Uri(op.FileName));
                    // MessageBox.Show();
                    File.Copy(op.FileName, Directory.GetCurrentDirectory() + "/Resources/" + System.IO.Path.GetFileNameWithoutExtension(op.FileName) + ".png", true);
                }
            }

            public void SendCard(LoginCard card)
            {
                VirtualBuffer.TmpLoginCard = card;
            }
        }

        private void ApplyImgBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ApplyImgBtn.Source = new BitmapImage(new Uri("Resources/apply_green.png", UriKind.Relative));
        }

        private void ApplyImgBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ApplyImgBtn.Source = new BitmapImage(new Uri("Resources/apply.png", UriKind.Relative));
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Card.ApplyImage(e);
        }

        private void ApplyImgBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string URL;
            if (Logo.Source!=null)
             URL = System.IO.Path.GetFileNameWithoutExtension(Logo.Source.ToString())+".png";
            else
            URL = "Resources/no-logo.png";

           var card =
               new LoginCard(Login.Text, Password.Text, Site.Text,  URL);
           Card.SendCard(card);
           this.Close();
        }
    }
}
