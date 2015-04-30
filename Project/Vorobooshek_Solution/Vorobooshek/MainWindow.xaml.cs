using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Threading;

namespace Vorobooshek
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int i;

        string srcPath = "user_config.txt";
        public static string globlog;
        string Connect = Properties.Resources.connect;
        bool outp = false, inchat = false;


        static private Socket Client;
        private IPAddress ip = IPAddress.Parse(Properties.Resources.ip);
        private int port = 7770;
        public Thread th;

        public string chat_login;

        #region Работа с основным окном

        public MainWindow()
        {
            InitializeComponent();

            #region Чтение и авторизация

            string[] text, parser;
            string login, password;

            if (File.Exists(srcPath) == false)
            {
                File.Create(srcPath);
            }
            
            while (outp == false)
            {
                try
                {
                    text = File.ReadAllLines(srcPath);

                    parser = text[0].Split(new char[] { ':' });
                    login = parser[1].Trim();
                    parser = text[1].Split(new char[] { ':' });
                    password = parser[1].Trim();
                    string loginCompare = "", passwordCompare = "";

                    string Check = "SELECT * FROM `accounts` WHERE login = '" + login + "' LIMIT 1;";

                    MySqlConnection myConnection = new MySqlConnection(Connect);
                    MySqlCommand logCh = new MySqlCommand(Check, myConnection);
                   
                        myConnection.Open();

                        MySqlDataReader rdr = logCh.ExecuteReader();
                        while (rdr.Read())
                        {
                            loginCompare = Convert.ToString(rdr[0]);
                            passwordCompare = Convert.ToString(rdr[1]);
                        }

                        if (loginCompare == login && passwordCompare == password)
                        {                     
                            outName.Content += login + "!";
                            chat_login = login;
                            outp = true;
                        }
                        else
                        {
                            MessageBox.Show("В файле конфигурации указан неверный логин и/или пароль. Пожалуйста введите данные вручную.");
                        
                            Autorize auth = new Autorize();
                            auth.ShowDialog();

                         //   outName.Content += globlog + "!";
                        }

                    }
                catch
                {
                    Autorize auth = new Autorize();
                    auth.ShowDialog();
                }

            }

            try
            {
                text = File.ReadAllLines(srcPath);

                parser = text[2].Split(new char[] { ':' });
                i = Convert.ToInt32(parser[1].Trim());

                ThemeSelector.SelectedIndex = i;
            }
            catch
            {
                ThemeSelector.SelectedIndex = 3;
            }

            if (outp == false) { chat_login = globlog; outName.Content += globlog + "!"; }
            
            #endregion



            Messages.IsReadOnly = true;
            SendMessages.IsEnabled = false;
            EnterChat.IsEnabled = true;
            SM.IsEnabled = false;

        }

        #region Чат

        void SendMessage(string message, string themeGet)
        {
            if (message != "" && message != " ") 
            {
                byte[] buffer = new byte[1024];
                byte[] byte_theme = new byte[10];

                buffer = Encoding.UTF8.GetBytes(message);
                byte_theme = Encoding.UTF8.GetBytes(themeGet);
                Client.Send(buffer);
                Client.Send(byte_theme);
            }
        }       

        void RecvMessage()
        {
            byte[] buffer = new byte[1024];
            byte[] byte_theme = new byte[10];

          /*  for (int f = 0; f < buffer.Length; f++)
            {
                buffer[f] = 0;
            } */

            Array.Clear(buffer, 0, buffer.Length);
            Array.Clear(byte_theme, 0, byte_theme.Length);

            for(; ; )
            {
                try
                {
                    Client.Receive(buffer);
                    Client.Receive(byte_theme);
                    string message = Encoding.UTF8.GetString(buffer);
                    string themeGet = Encoding.UTF8.GetString(byte_theme);
                    int count1 = themeGet.IndexOf(";;;5");
                    int count = message.IndexOf(";;;5");

                    if (count1 == -1)
                    {
                        continue;
                    }

                    string theme_string = "";
                    
                    for (int f = 0; f < count1; f++)
                    {
                        theme_string += themeGet[f];
                    }

                    int client_themeInt = Convert.ToInt32(theme_string.Trim());

                    if (count ==-1)
                    {
                        continue;
                    }

                    string Clear_Message = "";
                        
                    for(int f = 0; f < count; f++)
                    {
                        Clear_Message += message[f];
                    }

                    Array.Clear(buffer, 0, buffer.Length);
                    Array.Clear(byte_theme, 0, byte_theme.Length);

                    this.Dispatcher.Invoke((Action)delegate()
                    {

                        string[] splited_Clear_Message = Clear_Message.Split('`'); 

                      //  this.paragraph = new Paragraph();
                      //  Messages.Document = new FlowDocument(paragraph);

                        SolidColorBrush nick_brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(themeColor[client_themeInt]));

                        Console.WriteLine(client_themeInt);
                        Console.WriteLine("длина сообщения" + splited_Clear_Message.Length);

                        if (splited_Clear_Message.Length >= 2)
                        {
                            var from = splited_Clear_Message[0];
                            var text = splited_Clear_Message[1];
                            paragraph.Inlines.Add(new Bold(new Run(from + ":"))
                            {

                                Foreground = nick_brush
                            });
                            paragraph.Inlines.Add(text);
                         //   paragraph.Inlines.Add(new LineBreak());
                            this.DataContext = this;
                        }
                        else
                        {
                            Messages.AppendText(Clear_Message);
                        }
                         Messages.ScrollToEnd();

                         Console.WriteLine(Clear_Message);
                    });
                }
                catch
                {
                    MessageBox.Show("Сервер выключен");
                    if (th!= null) {th.Abort();}
                }
            }
        }

        private void SM_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#bdc3c7");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            SM.Fill = myBrush;

            SendMessage("\n" + chat_login + "` " + SendMessages.Text + ";;;5","\n" + Convert.ToString(i) + ";;;5");
            SendMessages.Text = "";

            Messages.ScrollToEnd();
        }

        private void SendMessages_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage("\n" + chat_login + "` " + SendMessages.Text + ";;;5", Convert.ToString(i) + ";;;5");
                SendMessages.Text = "";

                Messages.ScrollToEnd();
            }
        }

        private void EnterChat_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (inchat == false)
            { 
                Color colorize = (Color)ColorConverter.ConvertFromString("#bdc3c7");
                SolidColorBrush myBrush = new SolidColorBrush(colorize);
                EnterChat.Fill = myBrush;

                bool trying = false;


                try 
                { 
                    Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    Client.Connect(ip, port);

                    th = new Thread(delegate() { RecvMessage(); });
                    th.Start();
                    trying = true;
                    paragraph.Inlines.Add("Вы были успешно подключены к серверу чата...");  
                }
                catch
                {
                    MessageBox.Show("Ошибка, невозможно соедниниться с сервером. \n IP Сервера: "+ ip +"\n Порт сервера: " + port);
                }

                if (trying == true)
                {
                    SM.IsEnabled = true;
                    SendMessages.IsEnabled = true;
                    inchat = true;
                }
            }
            else
            {
                MessageBox.Show("Вы уже вошли в чат.");
            }
        }

    #endregion

        #region Работа с переключением темы приложения

        public string[] themeImage = new string[] 
        {
            "pack://application:,,,/Imgs/logo_greensea.png",
            "pack://application:,,,/Imgs/logo_orange.png",
            "pack://application:,,,/Imgs/logo_pumpkin.png",
            "pack://application:,,,/Imgs/logo_alizarin.png",
            "pack://application:,,,/Imgs/logo_peterriver.png",
            "pack://application:,,,/Imgs/logo_nephritis.png",
            "pack://application:,,,/Imgs/logo_amethyst.png",
            "pack://application:,,,/Imgs/logo_wetasphalt.png",
            "pack://application:,,,/Imgs/logo_asbestos.png"
        };

        public string[] themeColor = new string[] 
        {
            "#16a085",           
            "#f39c12",
            "#d35400",
            "#e74c3c",
            "#3498db",
            "#27ae60",
            "#9b59b6",
            "#34495e",
            "#7f8c8d" 
        };
        
        public void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            i = ThemeSelector.SelectedIndex;

            ImageSourceConverter imgs = new ImageSourceConverter();
            logo.Source = (ImageSource)imgs.ConvertFromString(themeImage[i]);

            string[] t = File.ReadAllLines(srcPath);
            string overwrite = null;

            if (t.Length == 3)           
            {
                t[2] = String.Empty;
            }    
                overwrite = "theme: " + i;
                StreamWriter sw = new StreamWriter(srcPath);
                sw.WriteLine(t[0]);
                sw.WriteLine(t[1]);
                sw.WriteLine(overwrite);
                sw.Close();
  /*          else if (t.Length <= 3)
            {
                overwrite = "theme: " + i;

                FileStream fs = new FileStream(srcPath, FileMode.Open);
                StreamWriter sw = new StreamWriter(fs);
                //byte rasch = Convert.ToByte(Convert.ToInt32(fs.Length) - 33);
                sw.BaseStream.Seek(fs.Length, SeekOrigin.End);
                sw.WriteLine(overwrite);
                sw.Close();
                fs.Close();
            } */
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#bdc3c7");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            (sender as Rectangle).Fill = myBrush;
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#FFE5E5E5");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            (sender as Rectangle).Fill = myBrush;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString(themeColor[i]);
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            (sender as Rectangle).Fill = myBrush;
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#bdc3c7");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            (sender as Rectangle).Fill = myBrush;
        }

        #region Системные кнопки

        private void ex_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            ex.Source = (ImageSource)imgs.ConvertFromString("pack://application:,,,/Imgs/buttons/x/x_hov.png");
        }

        private void ex_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            ex.Source = (ImageSource)imgs.ConvertFromString("pack://application:,,,/Imgs/buttons/x/x_click.png");
        }

        private void ex_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            ex.Source = (ImageSource)imgs.ConvertFromString("pack://application:,,,/Imgs/buttons/x/x_steady.png");
        }

        private void ex_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            ex.Source = (ImageSource)imgs.ConvertFromString("pack://application:,,,/Imgs/buttons/x/x_hov.png");

            AutorizEx aux = new AutorizEx();
            aux.ShowDialog();
            aux.Owner = this;

        }

        private void min_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            min.Source = (ImageSource)imgs.ConvertFromString("pack://application:,,,/Imgs/buttons/min/min_hov.png");
        }

        private void min_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            min.Source = (ImageSource)imgs.ConvertFromString("pack://application:,,,/Imgs/buttons/min/min_click.png");
        }

        private void min_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            min.Source = (ImageSource)imgs.ConvertFromString("pack://application:,,,/Imgs/buttons/min/min_steady.png");
        }

        private void min_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            min.Source = (ImageSource)imgs.ConvertFromString("pack://application:,,,/Imgs/buttons/min/min_hov.png");

            this.WindowState = System.Windows.WindowState.Minimized;
        }

        #endregion

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }

        #endregion

        private void Tit_Initialized(object sender, EventArgs e)
        {
            Tit.Content = this.Title;
        }

        #endregion

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (th != null) { th.Abort(); }
        }

    }
}
