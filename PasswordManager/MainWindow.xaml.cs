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
using System.IO;
using System.Security.Cryptography;
using System.Data;
using Mono.Data.Sqlite;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using WindowsInput.Native;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;



namespace PasswordManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public class FormMaster
    {
       
        public Grid MainGrid;
        public Manager manager { get; set; }
        List<Grid> CurrentGrid = new List<Grid>();

        public List<LoginCard> CurrenCard = new List<LoginCard>();
        public List<LoginCard> FavorCards = new List<LoginCard>();

        public FormMaster(Grid grid, Manager manager)
        {
            MainGrid = grid;
            this.manager = manager;
            FavorCards = FileUtils.ReadCardList("Favor");
        }

        Thickness StartMargin = new Thickness(10, 10, 406, 239);

        public void ShowCardList(List<LoginCard> LoginList)
        {

            MainGrid.Children.Clear();
            MainGrid.Height = 540;
            CurrenCard = new List<LoginCard>();
            if (LoginList != null)
            {
                for (int i = 0; i < LoginList.Count; i++)
                {
                    AddCard(LoginList[i], i);
                }
            }
        }


        public void AddCard(LoginCard Card, int index)
        {

            Grid grid = new Grid();

            int deltaX = 0, deltaY = 0;

            if (index % 2 == 0)
            {
                deltaX = 0;
                deltaY = 200 * index / 2;
            }
            else
            {
                deltaX = 340;
                deltaY = (index - 1) / 2 * 200;
            }

            grid.Margin = new Thickness(
                StartMargin.Left + deltaX,
                StartMargin.Top + deltaY,
                StartMargin.Right - deltaX,
               0
                );

            CurrenCard.Add(Card);

            Border br = new Border()
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(62, 62, 66)),
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 175,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 315,
                Margin = new Thickness(0, 0, 0, 0),
                BorderThickness = new Thickness(2, 2, 2, 2)
            };
            br.CornerRadius = new CornerRadius(3);
            br.Background = new SolidColorBrush(Color.FromRgb(62, 62, 66));
            grid.Children.Add(br);

            //Rectangle rect = new Rectangle()
            //{

            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Height = 170,
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Width = 32,
            //    Margin = new Thickness(5, 5, 5, 0),


            //};
            //rect.Fill = new SolidColorBrush(Color.FromRgb(62, 62, 66));
            // rect.Background = new SolidColorBrush(Color.FromRgb(62, 62, 66));
            // grid.Children.Add(rect);


            System.Windows.Controls.Label label = new System.Windows.Controls.Label()
            {
                Content = "Логин",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(128, 22, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = new SolidColorBrush(Color.FromRgb(181, 181, 184)),
                FontSize = 13,
                FontFamily = new FontFamily("Segoe UI Semibold"),
                Height = 27,
                Width = 56
            };
            grid.Children.Add(label);

            System.Windows.Controls.Button btn = new System.Windows.Controls.Button()
            {
                Content = Card.SiteURL,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 132, 0, 10),
                VerticalAlignment = VerticalAlignment.Top,
                Background = new SolidColorBrush(Color.FromRgb(181, 181, 184)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(41, 38, 38)),
                FontSize = 14,
                FontFamily = new FontFamily("Segoe UI Semibold"),
                Name = "Button_" + index
                
            };  
            btn.MouseDown += BtnWeb_Click;
            btn.PreviewMouseDown += BtnWeb_Click;
            btn.Height = 36;
            btn.Width = 294;   
            grid.Children.Add(btn);

            label = new System.Windows.Controls.Label()
            {
                Content = "Пароль",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(128, 72, 120, 0),
                VerticalAlignment = VerticalAlignment.Top,
               Foreground = new SolidColorBrush(Color.FromRgb(181, 181, 184)),
                FontSize = 13,
                FontFamily = new FontFamily("Segoe UI Semibold"),
                Height = 27,
                Width = 56
            };
            grid.Children.Add(label);

            TextBox pass_tb = new TextBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 23,
                Width = 172,
                Margin = new Thickness(132, 97, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                Text = Card.Password
            };
            pass_tb.VerticalAlignment = pass_tb.VerticalAlignment = VerticalAlignment.Top;
            pass_tb.Background = new SolidColorBrush(Color.FromRgb(104, 104, 104));
            pass_tb.BorderBrush = new SolidColorBrush(Color.FromRgb(62, 62, 66));
            pass_tb.Foreground = new SolidColorBrush(Color.FromRgb(181, 181, 184));
            pass_tb.FontSize = 14;
            pass_tb.Visibility = Visibility.Hidden;
            pass_tb.Name = "PassBox" + index;
            pass_tb.PreviewMouseDown += Passbox_MouseDown;
            //grid.Children.Add(pass_tb);

            PasswordBox passbox = new PasswordBox()
            {
                HorizontalAlignment = label.HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(132, 97, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 172,
                Height = 23,
                Background = new SolidColorBrush(Color.FromRgb(104, 104, 104)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(62, 62, 66)),
            Foreground = new SolidColorBrush(Color.FromRgb(181, 181, 184)),
            
            Password = Card.Password,
                Name = "PassBox" + index
            };
            //passbox.PreviewMouseDown += Passbox_MouseDown;
            //grid.Children.Add(passbox);

            TextBlock block = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 172,
                Height = 23,
                Margin = new Thickness(132, 97, 0, 0),
                Background = new SolidColorBrush(Color.FromRgb(104, 104, 104)),
                Foreground = new SolidColorBrush(Color.FromRgb(181, 181, 184)),
                Name = "PassLbl_" + index,
                Text = "     Скопировать пароль",
                FontSize = 13
            };
            block.MouseEnter += Block_MouseEnter;
            block.MouseLeave += Block_MouseLeave;
            //Label l = new Label()
            //{
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Width = 172,
            //    Height = 23,
            //    Margin = new Thickness(132, 97, 0, 0),
            //    Background = new SolidColorBrush(Color.FromRgb(104, 104, 104)),
            //    BorderBrush = new SolidColorBrush(Color.FromRgb(62, 62, 66)),
            //    Foreground = new SolidColorBrush(Color.FromRgb(181, 181, 184)),
            //    Name = "PassLbl_" + index,
            //    Content = "Copy password",
                
                
            //};
           // l.FontSize = 10;
            block.PreviewMouseDown += LabelCopy_PreviewMouseDown;
            grid.Children.Add(block);


            TextBox tb = new TextBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 23,
                Width = 172,
                Margin = new Thickness(132, 49, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                Text = Card.Login
            };
            tb.VerticalAlignment = tb.VerticalAlignment = VerticalAlignment.Top;
            tb.Background = new SolidColorBrush(Color.FromRgb(104, 104, 104));
            tb.Foreground = new SolidColorBrush(Color.FromRgb(181, 181, 184));
            tb.BorderBrush = new SolidColorBrush(Color.FromRgb(62, 62, 66));
            tb.FontSize = 14;
            grid.Children.Add(tb);

            Ellipse Elps = new Ellipse()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 90,
                Height = 90,
                Margin = new Thickness(21, 30, 0, 0),
            };
            Elps.Fill = new ImageBrush(new BitmapImage(new Uri("Resources/" + Card.PictureURL, UriKind.Relative)));
            Elps.PreviewMouseDown += Img_MouseDownChangePic;
            grid.Children.Add(Elps);


            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            //{
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Height = 90,
            //    Width = 120,
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Margin = new Thickness(21, 10, 0, 0),
            //    Source = new BitmapImage(new Uri("Resources/" + Card.PictureURL, UriKind.Relative))
            //};
            //img.MouseDown += Img_MouseDownChangePic;


            img = new System.Windows.Controls.Image()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 29,
                Width = 25,
                Margin = new Thickness(258, 133, 0, 0),
                Source = new BitmapImage(new Uri("Resources/browser.ico", UriKind.Relative))
            };
            //grid.Children.Add(img);


            img = new System.Windows.Controls.Image()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 14,
                Width = 14,
                Margin = new Thickness(290, 10, 0, 0),
                Source = new BitmapImage(new Uri("Resources/cross.png", UriKind.Relative))

            };
            img.PreviewMouseDown += ImgCross_PreviewMouseDown;
            img.Name = "Cross_" + index.ToString();
            img.MouseEnter += ImgCross_MouseEnter;
            img.MouseLeave += ImgCross_MouseLeave;
            grid.Children.Add(img);

            img = new System.Windows.Controls.Image()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 14,
                Width = 14,
                Margin = new Thickness(271, 10, 0, 0),
                Source = new BitmapImage(new Uri("Resources/edit.png", UriKind.Relative))

            };
            img.PreviewMouseDown += ImgEdit_PreviewMouseDown; ;
            img.Name = "Edit_" + index.ToString();
            grid.Children.Add(img);


            img = new System.Windows.Controls.Image()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 20,
                Width = 20,
                Margin = new Thickness(276, 98, 0, 0),
                Source = new BitmapImage(new Uri("Resources/clipboard.png", UriKind.Relative))
            };
            img.MouseLeftButtonDown += Img_MouseDown;
            img.MouseLeftButtonUp += Img_MouseLeftButtonUp;
            img.Name = "PassBox" + index;
            //grid.Children.Add(img);

            string url = "";

            // if (! (FavorCards.Any(card => card.SiteURL == Card.SiteURL) && FavorCards.Any(card => card.Login == Card.Login) && FavorCards.Any(card => card.PictureURL == Card.PictureURL)))
            //{
            //    url = "Resources/star-curved-outline.png";
            //}
            // else
            //{
            //    url = "Resources/star_gold.png";
            //}

            if (Card.Favor)
            {
                url = "Resources/star_gold.png";
            }
            else
            {
                url = "Resources/star-curved-outline.png";
            }

            img = new System.Windows.Controls.Image()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 14,
                Width = 14,
                Margin = new Thickness(252, 10, 0, 0),
                Source = new BitmapImage(new Uri(url, UriKind.Relative))
            };
            img.MouseDown += ImgStar_MouseDown;
            img.Name = "Star_" + index.ToString();
            grid.Children.Add(img);

         
           // grid.Background = new SolidColorBrush(Color.FromRgb(62, 62, 66));
            MainGrid.Children.Add(grid);

            MainGrid.Height += (index % 2) * 190;

            CurrentGrid.Add(grid);
        }


        private void BtnWeb_Click(object sender, RoutedEventArgs e)
        {
            var btn = (System.Windows.Controls.Button)sender;
            int index = int.Parse(btn.Name.Split('_')[1]);
            manager.BrowserAutoComplete(CurrenCard[index]);

            //string url = "\"" + btn.Content.ToString() + "\"";
            //try
            //{
            //    System.Diagnostics.Process.Start(url);
            //}
            //catch { }
        }

        private void Block_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock TB = (TextBlock)e.Source;
            TB.Foreground = new SolidColorBrush(Color.FromRgb(181, 181, 184));

        }

        private void Block_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock TB = (TextBlock)e.Source;
            TB.Foreground = new SolidColorBrush(Color.FromRgb(17, 180, 32));
        }

        private void LabelCopy_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var label = (TextBlock)e.Source;
            int index = int.Parse(label.Name.Split('_')[1]);
            VirtualBuffer.Password = Crypto.EncryptBuffer(CurrenCard[index].Password);
            Clipboard.SetText("");
        }

        private void ImgCross_MouseLeave(object sender, MouseEventArgs e)
        {
           var Image = (System.Windows.Controls.Image)e.Source;
            Image.Source = new BitmapImage(new Uri("Resources/cross.png", UriKind.Relative));
        }

        private void ImgCross_MouseEnter(object sender, MouseEventArgs e)
        {
            var Image = (System.Windows.Controls.Image)e.Source;
            Image.Source = new BitmapImage(new Uri("Resources/cross_red.png", UriKind.Relative));
        }

        private void ImgEdit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var img = (System.Windows.Controls.Image)e.Source;
            int index = int.Parse(img.Name.Split('_')[1]);

            manager.EditCard(index);
        }

        private void ImgCross_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var img = (System.Windows.Controls.Image)e.Source;
            int index = int.Parse(img.Name.Split('_')[1]);
            manager.DeleteCard(index);
        }

        private void ImgStar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var img = (System.Windows.Controls.Image)e.Source;
            int index = int.Parse(img.Name.Split('_')[1]);

            // MessageBox.Show(img.Source.ToString());
            if (img.Source.ToString().Contains("curved"))
            {
                if (FavorCards == null)
                    FavorCards = new List<LoginCard>();
                //manager.Favorites.Add(CurrentGrid[index]);
                FavorCards.Add(CurrenCard[index]);
                FavorCards.Last().Favor = true;

                img.Source = new BitmapImage(new Uri("Resources/star_gold.png", UriKind.Relative));

            }
            else
            {

                // manager.Favorites.Remove(CurrentGrid[index]);
                FavorCards.Last().Favor = false;
                FavorCards.Remove(CurrenCard[index]);

                img.Source = new BitmapImage(new Uri("Resources/star-curved-outline.png", UriKind.Relative));
            }

            FileUtils.WriteCardList(FavorCards, "Favor");

            //manager.WriteCardList(CurrenCard, "Vivaldi");
        }

        private void Img_MouseDownChangePic(object sender, MouseButtonEventArgs e)
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
                var img = (System.Windows.Controls.Image)e.Source;
                img.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void Img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var img = (System.Windows.Controls.Image)e.Source;
            img.Source = new BitmapImage(new Uri("Resources/clipboard.png", UriKind.Relative));
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var img = (System.Windows.Controls.Image)e.Source;
            img.Source = new BitmapImage(new Uri("Resources/clipboard_green.png", UriKind.Relative));

            var MainGridList = MainGrid.Children;
            foreach (var item in MainGridList)
            {
                var grid = (Grid)item;
                var GridChildren = grid.Children;
                foreach (var gridChildren in GridChildren)
                {
                    TextBox textbox = null;
                    if (gridChildren is TextBox)
                    {
                        textbox = (TextBox)gridChildren;

                        if (textbox.Name == img.Name)
                        {
                            Clipboard.SetText(textbox.Text);
                        }
                    }
                }
            }
        }

        private void Passbox_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.Source is PasswordBox)
            {
                PasswordBox pb = (PasswordBox)e.Source;
                var GridList = MainGrid.Children;
                foreach (var item in GridList)
                {
                    var i = (Grid)item;
                    var GridChildren = i.Children;
                    foreach (var gc in GridChildren)
                    {
                        TextBox j = null;
                        if (gc is TextBox)
                        {
                            j = (TextBox)gc;
                            if (j.Name == pb.Name)
                            {
                                pb.Visibility = Visibility.Hidden;
                                j.Visibility = Visibility.Visible;
                                break;
                            }
                        }

                    }
                }
            }
            else
              if (e.Source is TextBox pb)
            {
                var GridList = MainGrid.Children;
                foreach (var item in GridList)
                {
                    var i = (Grid)item;
                    var GridChildren = i.Children;
                    foreach (var gc in GridChildren)
                    {
                        PasswordBox j = null;
                        if (gc is PasswordBox)
                        {
                            j = (PasswordBox)gc;
                            if (j.Name == pb.Name)
                            {
                                pb.Visibility = Visibility.Hidden;
                                j.Visibility = Visibility.Visible;
                                break;
                            }
                        }

                    }
                }
            }

        }

        public void Item_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string browser = "";
            for (int i = 0; i < manager.BrowsersList.Count; i++)
            {
                if (e.Source.ToString().Contains(manager.BrowsersList[i]))
                {
                    browser = manager.BrowsersList[i];
                    break;
                }
            }
            ShowCardList(manager.BrowsersPass[browser]);
        }

    }

    public class DpApi
    {
        [DllImport("crypt32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]

        private static extern bool CryptProtectData(ref DATA_BLOB pPlainText, string szDescription, ref DATA_BLOB pEntropy, IntPtr pReserved, ref CRYPTPROTECT_PROMPTSTRUCT pPrompt, int dwFlags, ref DATA_BLOB pCipherText);

        [DllImport("crypt32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern
        bool CryptUnprotectData(ref DATA_BLOB pCipherText, ref string pszDescription, ref DATA_BLOB pEntropy, IntPtr pReserved, ref CRYPTPROTECT_PROMPTSTRUCT pPrompt, int dwFlags, ref DATA_BLOB pPlainText);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]

        internal struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CRYPTPROTECT_PROMPTSTRUCT
        {
            public int cbSize;
            public int dwPromptFlags;
            public IntPtr hwndApp;
            public string szPrompt;
        }
        public static byte[] Decrypt(byte[] cipherTextBytes, byte[] entropyBytes, out string description)
        {
            DATA_BLOB plainTextBlob = new DATA_BLOB();
            DATA_BLOB cipherTextBlob = new DATA_BLOB();
            DATA_BLOB entropyBlob = new DATA_BLOB();
            CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT()
            {
                cbSize = Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT)),
                dwPromptFlags = 0,
                hwndApp = IntPtr.Zero,
                szPrompt = null
            };
            description = String.Empty;
            try
            {
                try
                {
                    if (cipherTextBytes == null)
                        cipherTextBytes = new byte[0];
                    cipherTextBlob.pbData = Marshal.AllocHGlobal(cipherTextBytes.Length);
                    if (cipherTextBlob.pbData == IntPtr.Zero) throw new Exception(String.Empty);
                    cipherTextBlob.cbData = cipherTextBytes.Length;
                    Marshal.Copy(cipherTextBytes, 0, cipherTextBlob.pbData, cipherTextBytes.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Empty, ex);
                }
                try
                {
                    if (entropyBytes == null)
                        entropyBytes = new byte[0];
                    entropyBlob.pbData = Marshal.AllocHGlobal(entropyBytes.Length);
                    if (entropyBlob.pbData == IntPtr.Zero) throw new Exception(String.Empty);
                    entropyBlob.cbData = entropyBytes.Length;
                    Marshal.Copy(entropyBytes, 0, entropyBlob.pbData, entropyBytes.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Empty, ex);
                }
                int flags = 0x1;
                bool success = CryptUnprotectData(ref cipherTextBlob, ref description, ref entropyBlob, IntPtr.Zero, ref prompt, flags, ref plainTextBlob);
                if (!success)
                {
                    int errCode = Marshal.GetLastWin32Error();
                    // throw new Exception(String.Empty, new Exc(errCode));
                }
                byte[] plainTextBytes = new byte[plainTextBlob.cbData];
                Marshal.Copy(plainTextBlob.pbData, plainTextBytes, 0, plainTextBlob.cbData);
                return plainTextBytes;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Empty, ex);
            }
            finally
            {
                if (plainTextBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(plainTextBlob.pbData);
                if (cipherTextBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(cipherTextBlob.pbData);
                if (entropyBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(entropyBlob.pbData);
            }
        }
    }

    public static class Crypto
    {

       static Crypto()
        {
            Generatekey();
        }

        static public string EncryptBuffer(string pass)
        {
            string result = "";
            for (int i = 0; i < pass.Length; i++)
                result += (char)(pass[i] ^ key);
            return result;
        }

        static public string DecryptBuffer(string pass)
        {
            return EncryptBuffer(pass);
        }
    
        static public string skey { get; set; }

        static int key { get; set; }
     
        public static List<string> AgregateParams()
        {
            List<string> ParamList = new List<string>();

            ParamList.Add(Environment.MachineName);
            ParamList.Add(Environment.UserName);
            ParamList.Add(Environment.OSVersion.ToString());
            ParamList.Add(Environment.ProcessorCount.ToString());
            ParamList.Add(Environment.OSVersion.Platform.ToString());

            return ParamList;
        }

        public static string GetHash(List<string> list)
        {

            MD5 md5 = MD5.Create();
            string source = "";
            foreach (var str in list)
                source += str;

            byte[] input = Encoding.ASCII.GetBytes(source);

            byte[] hash = md5.ComputeHash(input);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static void Generatekey()
        {
          var Hash = GetHash(AgregateParams());

            string Key = "";

            for (int i = 15; i >= 0; i--)
                    Key += Hash[i];
            
            skey = Key;
            key = skey.Length;
        }

        public static void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                {
                                    int data;
                                    while ((data = fsIn.ReadByte()) != -1)
                                    {
                                        cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // failed to encrypt file
            }
        }

        public static void DecryptFile(string inputFile, string outputFile)
        {

            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                    /* This is for demostrating purposes only. 
                     * Ideally you will want the IV key to be different from your key and you should always generate a new one for each encryption in other to achieve maximum security*/
                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // failed to decrypt file
            }
        }
    }

    static class FileUtils
    {
        public static List<string> FilePaths = new List<string>();


        static public void WriteCardList(List<LoginCard> list, string FileName)
        {
            File.WriteAllText(FileName, "");
            for (int i = 0; i < list.Count; i++)
            {
                string[] CardStr = new string[] { "" + i, "" + list[i].Login, "" + list[i].Password, "" + list[i].SiteURL, "" + list[i].PictureURL, "" + list[i].Favor };
                File.AppendAllLines(FileName, CardStr);
            }

           Crypto.EncryptFile(FileName,"DB/"+FileName);
            //File.Delete(FileName);
            //File.Move(FileName + "E", FileName);
        }

        static public List<LoginCard> ReadCardList(string path)
        {
            List<LoginCard> CardList = new List<LoginCard>();

            //cryptor.DecryptFile(path,path+"D");
            //File.Delete(path);
            //File.Move(path +"D", path);

            if (!File.Exists(path))
                File.Create(path);

            string[] Text = new string[0];
            try
            {
                Text = File.ReadAllLines(path);
            }
            catch
            { }

            if (Text.Length < 5)
                return null;

            for (int i = 0; i < Text.Length; i += 6)
            {
                bool flag;
                if (Text[i + 5] == "True")
                    flag = true;
                else
                    flag = false;

                LoginCard card = new LoginCard(
                    Text[i + 1],
                    Text[i + 2],
                    Text[i + 3],
                    Text[i + 4],
                    flag
                    );
                CardList.Add(card);
            }

            return CardList;
        }
    }

    public class Manager
    {
        public FormMaster FM { set; get; }

        public List<LoginCard> BaseCard = new List<LoginCard>();

        public string[] PathList =
                {
               Environment.GetEnvironmentVariable("LocalAppData") + "\\Google\\Chrome\\User Data\\Default\\Login Data",
                Environment.GetEnvironmentVariable("LocalAppData") +"\\Vivaldi\\User Data\\Default\\Login Data",
                Environment.GetEnvironmentVariable("LocalAppData") + "\\Yandex\\YandexBrowser\\User Data\\Default\\LoginData",
                Environment.GetEnvironmentVariable("LocalAppData") + "\\Kometa\\User Data\\Default\\Login Data",
                Environment.GetEnvironmentVariable("LocalAppData") + "\\Amigo\\User\\User Data\\Default\\Login Data",
                Environment.GetEnvironmentVariable("LocalAppData") + "\\Torch\\User Data\\Default\\Login Data",
                Environment.GetEnvironmentVariable("LocalAppData") + "\\Orbitum\\User Data\\Default\\Login Data",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Opera Software\\Opera Stable\\Login Data"

            }; // - Массив с путями к SQLite базам разных браузеров

        public List<string> BrowsersList = new List<string> { "Vivaldi", "Yandex", "Chrome", "Firefox", "Opera","Edge","InternetExplorer" };

        public Dictionary<string, List<List<string>>> BrowserDB = new Dictionary<string, List<List<string>>>()
     {
            {"Vivaldi", new List<List<string>>()},
            {"Chrome",  new List<List<string>>() },
            {"Yandex", new List<List<string>>() },
            {"Opera",  new List<List<string>>() },
            {"Firefox",  new List<List<string>>() }
     };

        public Dictionary<string, List<LoginCard>> BrowsersPass = new Dictionary<string, List<LoginCard>>();

        public Manager()
        {
            //Crypto crypt = new Crypto();
            //crypt.EncryptFile("BaseCards1","BaseCards");
            BaseCard = FileUtils.ReadCardList("BaseCards");
        }

        public void ScanBrowser(string browser)
        {
            var LoginList = BrowserDB[browser];

            DataTable DB = new DataTable();
            int key = 0;

            for (int i = 0; i < PathList.Length; i++)
            {
                if (PathList[i].Contains(browser))
                {
                    key = i;
                    break;
                }
            }

            string ConnectionStrin = "URI = file:" + PathList[key];

            using (SqliteConnection Connection = new SqliteConnection(ConnectionStrin))
            {
                Connection.Open();
                string Select = "SELECT  username_value, password_value, signon_realm, username_element, password_element FROM logins ";

                LoginList = new List<List<string>>();

                using (SqliteCommand Command = new SqliteCommand(Select, Connection))
                {
                    SqliteDataAdapter adapter = new SqliteDataAdapter(Select, Connection);
                    adapter.Fill(DB);
                    byte[] entropy = null;

                    for (int i = 0; i < DB.Rows.Count; i++)
                    {
                        var ArrBytes = (byte[])DB.Rows[i][1];
                        byte[] decrypted = DpApi.Decrypt(ArrBytes, entropy, out string description);
                        string password = new UTF8Encoding(true).GetString(decrypted);
                        List<string> tmp = new List<string>
                        {
                            DB.Rows[i][0].ToString(),
                            password,
                            DB.Rows[i][2].ToString(),
                            DB.Rows[i][3].ToString(),
                            DB.Rows[i][4].ToString()
                        };
                        LoginList.Add(tmp);
                    }
                }
                BrowserDB[browser] = LoginList;
                Connection.Close();
            }
        }

        public List<LoginCard> MakeCardList(string browser)
        {
            List<LoginCard> list = new List<LoginCard>();
            var tmp = BrowserDB[browser];

            for (int i = 0; i < tmp.Count; i++)
            {
                LoginCard card = new LoginCard(
                    tmp[i][0],
                    tmp[i][1],
                    tmp[i][2],
                    browser + ".png",
                    false,
                    tmp[i][3],
                    tmp[i][4]
                    );
                list.Add(card);
            }

            return list;
        }

        public void ScanAllBrowsers(TreeViewItem TreeViewitem)
        {
            int counterValidBrowser = 0;

            foreach (var browser in BrowsersList)
            {
                try
                {
                    ScanBrowser(browser);
                    if (BrowserDB[browser].Count > 0)
                    {
                        TreeViewItem item = new TreeViewItem()
                        {
                            Header = browser + $"({BrowserDB[browser].Count})"
                        };
                        item.PreviewMouseDown += FM.Item_PreviewMouseDown;
                        item.Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                        TreeViewitem.Items.Add(item);
                        counterValidBrowser++;

                        var tmpList = MakeCardList(browser);
                        FileUtils.WriteCardList(tmpList, browser);
                        BrowsersPass.Add(browser, new List<LoginCard>());
                        FileUtils.FilePaths.Add(browser);
                        foreach (var card in tmpList)
                        {
                            BrowsersPass[browser].Add(card);
                        }
                    }
                }
                catch { }
            }
            TreeViewitem.Header += $"({counterValidBrowser})";

        }

        public List<LoginCard> SearchCard(string str)
        {
            List<LoginCard> list = new List<LoginCard>();

            foreach (var browser in BrowsersPass)
            {
                foreach (var card in browser.Value)
                {
                    if (card.SiteURL.Contains(str) || card.Login.Contains(str))
                    {
                        list.Add(card);
                    }
                }
            }

            foreach (var card in BaseCard)
            {
                if (card.SiteURL.Contains(str) || card.Login.Contains(str))
                {
                    list.Add(card);
                }
            }

            return list;
        }

        public void DeleteCard(int index)
        {
            FM.CurrenCard.Remove(FM.CurrenCard[index]);
            FileUtils.WriteCardList(FM.CurrenCard, "BaseCards");
            FM.ShowCardList(FM.CurrenCard);
        }

        public void EditCard(int index)
        {
            AddCardForm EditForm = new AddCardForm(FM.CurrenCard[index]);
            EditForm.Closed += EditForm_Closed;
            EditForm.Show();
            FM.CurrenCard.Remove(FM.CurrenCard[index]);

        }

        private void EditForm_Closed(object sender, EventArgs e)
        {
            FM.CurrenCard.Add(VirtualBuffer.TmpLoginCard);
            FileUtils.WriteCardList(FM.CurrenCard, "BaseCards");
            FM.ShowCardList(FM.CurrenCard);
        }
        
        public string IdentifyDefaultBrowser()
        {
            string browser = "Edge";

            RegistryKey readKey = Registry.CurrentUser.OpenSubKey(@"software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice");
            string loadString = (string)readKey.GetValue("ProgId");

            foreach (var item in BrowsersList)
            {
                if (loadString.Contains(item))
                    return item;
            }

            return browser;
        }

        public void BrowserAutoComplete(LoginCard card)
        {
            string browser = IdentifyDefaultBrowser();
            string target_name = "";
            OpenQA.Selenium.IWebDriver driver = null;
   
            switch(browser)
            {
                case "Firefox": { driver = new OpenQA.Selenium.Firefox.FirefoxDriver(); target_name = "geckodriver"; break; }
                case "Chrome": { driver = new OpenQA.Selenium.Chrome.ChromeDriver(); target_name = "chromedriver"; break; }
                case "Edge": { driver = new OpenQA.Selenium.Edge.EdgeDriver(); target_name = "MicrosoftWebDriver"; break; }
                default: {  driver = new OpenQA.Selenium.IE.InternetExplorerDriver(); target_name = "IEDriverServer"; break;  }
            }

            try
            {
                driver.Navigate().GoToUrl(card.SiteURL);
                driver.FindElement(By.Name(card.LogElem)).Clear();
                driver.FindElement(By.Name(card.LogElem)).SendKeys(card.Login);
                driver.FindElement(By.Name(card.PassElem)).Clear();
                driver.FindElement(By.Name(card.PassElem)).SendKeys(card.Password);
            }
            catch
            { }

            try
            {
                System.Diagnostics.Process[] local_procs = System.Diagnostics.Process.GetProcesses();
                System.Diagnostics.Process target_proc = local_procs.First(p => p.ProcessName == target_name);
                target_proc.Kill();
            }
            catch { }


        }
    }

    public static class VirtualBuffer
    {

        static public LoginCard TmpLoginCard { get; set; }

        static string password;

        static public string Password
        {
            get { return password; }
            set { password = value; Clipboard.SetText(password);
            }
        }

    }

    public class LoginCard
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string SiteURL { get; set; }
        public string PictureURL { get; set; }
        public bool Favor { get; set; }
        public string LogElem { get; set; }
        public string PassElem { get; set; }


        public LoginCard (string login="", string pass="", string url = "", string picture = "Resources/no-logo.png", bool favor= false, string logEl = "username", string passEl = "password")
        {
            Login = login;
            Password = pass;
            PictureURL = picture;
            SiteURL = url;
            LogElem = logEl;
            PassElem = passEl;
            Favor = favor;
        }    
    }



     public partial class MainWindow : Window
    {

        Manager manager = new Manager();

        public MainWindow()
        {
            //Crypto.EncryptFile("BaseCards", "BaseCards2");
            //crypt.DecryptFile("BaseCards","BaseCards1");
          
            InitializeComponent();

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            FormMaster FM = new FormMaster(CardGrid, manager);
            manager.FM = FM;
            
            FavorsTreeView_PreviewMouseDown(null,null);

        }


        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if ((Keyboard.GetKeyStates(Key.V) &KeyStates.Down) > 0  && (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down)>0 && VirtualBuffer.Password!= null)
            {
                Clipboard.SetText(Crypto.DecryptBuffer(VirtualBuffer.Password));
                WindowsInput.InputSimulator s = new WindowsInput.InputSimulator();
                s.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONVERT, VirtualKeyCode.VK_V);
            }
        }

        private void FavorsTreeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            manager.FM.ShowCardList(FileUtils.ReadCardList("Favor"));
        }

        private void Search_RTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Search_RTB.Document.Blocks.Clear();
            Search_RTB.Document.Blocks.Add(new Paragraph(new Run("")));
        }

        private void Search_pic_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string query = new TextRange(Search_RTB.Document.ContentStart, Search_RTB.Document.ContentEnd).Text;
            query = query.Trim();

            manager.FM.ShowCardList(manager.SearchCard(query));
        }

        private void Img_Plus_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            AddCardForm AddForm = new AddCardForm(new LoginCard());
            AddForm.Owner = this;
            AddForm.Show();
            AddForm.Closed += AddForm_Closed;
         
        }

        private void AddForm_Closed(object sender, EventArgs e)
        {
            if (VirtualBuffer.TmpLoginCard != null)
            {
                if (manager.BaseCard == null)
                    manager.BaseCard = new List<LoginCard>();
                manager.BaseCard.Add(VirtualBuffer.TmpLoginCard);
                FileUtils.WriteCardList(manager.BaseCard, "BaseCards");
                manager.FM.ShowCardList(manager.BaseCard);
            }
        }

        private void TreeViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            manager.FM.ShowCardList(FileUtils.ReadCardList("BaseCards"));
        }

        private void Search_pic_PreviewMouseDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = new TextRange(Search_RTB.Document.ContentStart, Search_RTB.Document.ContentEnd).Text;
                query = query.Trim();
                manager.FM.ShowCardList(manager.SearchCard(query));
            }

        }

        private void Refresh_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            manager.ScanAllBrowsers(TreeVItemBrowsers);
        }
    }

}
