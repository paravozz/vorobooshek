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
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace Vorobooshek
{
    /// <summary>
    /// Логика взаимодействия для Autorize.xaml
    /// </summary>
    public partial class Autorize : Window
    {
        string Connect = Properties.Resources.connect;

        public Autorize()
        {
            InitializeComponent();
        }

        private void login_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
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
            Color colorize = (Color)ColorConverter.ConvertFromString("#e74c3c");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            (sender as Rectangle).Fill = myBrush;
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#bdc3c7");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            (sender as Rectangle).Fill = myBrush;

            string login = log.Text,
                   password = pass.Text, loginCompare = "", passwordCompare = "";

            string Check = "SELECT * FROM `accounts` WHERE login = '" + login + "' LIMIT 1;";

            MySqlConnection myConnection = new MySqlConnection(Connect);
            MySqlCommand logCh = new MySqlCommand(Check, myConnection);

            try
            {
                myConnection.Open();

                MySqlDataReader rdr = logCh.ExecuteReader();
                while (rdr.Read())
                {
                    loginCompare = Convert.ToString(rdr[0]);
                    passwordCompare = Convert.ToString(rdr[1]);
                }

                if (loginCompare == login && passwordCompare == password)
                {
                    using (StreamWriter sw = new StreamWriter("user_config.txt"))
                    {
                        sw.WriteLine("login: " + login);
                        sw.WriteLine("password: " + password);

                        MainWindow.globlog = login;
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Вы ввели неверные данные.");
                }
                myConnection.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка соединения");
            }
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#e74c3c");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            Reg.Foreground = myBrush;
        }

        private void Reg_MouseLeave(object sender, MouseEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#FF4D4D4D");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            Reg.Foreground = myBrush;
        }

        private void Reg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#e74c3c");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            Reg.Foreground = myBrush;
        }

        private void Reg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#e74c3c");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            Reg.Foreground = myBrush;


            Registration rg = new Registration();
            rg.Owner = this;
            rg.ShowDialog();
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
    }
}
