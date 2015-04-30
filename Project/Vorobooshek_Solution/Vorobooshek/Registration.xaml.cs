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

namespace Vorobooshek
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        string Connect = Properties.Resources.connect;

        public Registration()
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

             if (log.Text == String.Empty || log.Text == " " || pass.Text == String.Empty || pass.Text == " " || pass_conf.Text == String.Empty || pass_conf.Text == " " || pass.Text == "Пароль" || pass_conf.Text == "Подтвердите пароль" || log.Text == "Логин" )
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
             else if (pass.Text != pass_conf.Text) {MessageBox.Show("Пароли должны быть одинаковыми");}
            else
            {

                string login = log.Text,
                       password = pass.Text;

                string CommandText1 = "INSERT INTO `accounts` (`login`, `password`) VALUES ('"+ login +"', '"+ password +"');";                

                MySqlConnection myConnection = new MySqlConnection(Connect);
                MySqlCommand myCommand1 = new MySqlCommand(CommandText1, myConnection);

               
                try
                {
                    myConnection.Open();

                    try
                    {
                        myCommand1.ExecuteNonQuery();

                        MessageBox.Show("Поздравляем, вы успешно зарегистрировались");

                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Неверные данные");
                    }
                    
                    myConnection.Close();
                }
                catch 
                {
                    MessageBox.Show("Вы не подключены к серверу, проверьте подключение к интернету или попробуйте повторить попытку");
                }
            }
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#bdc3c7");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            (sender as Rectangle).Fill = myBrush;
        }

        private void log_LostFocus(object sender, RoutedEventArgs e)
        {
            string login = log.Text, logsel = null;

            string availCheck = "SELECT login FROM `accounts` WHERE login = '"+login+"' LIMIT 1;";

            MySqlConnection myConnection = new MySqlConnection(Connect);
            MySqlCommand selection = new MySqlCommand(availCheck, myConnection);

            try
            {
                myConnection.Open();
                MySqlDataReader SelRead = selection.ExecuteReader();
                while (SelRead.Read())
                { logsel = Convert.ToString(SelRead[0]); }
               
                if (login == " " || login == "")
                { avlbl.Content = "Ошибка"; }
                else
                if (logsel != null) 
                { 
                    SolidColorBrush Brush2 = new SolidColorBrush(Colors.Red);
                    avlbl.Foreground = Brush2;
                    avlbl.Content = "Занят";
                    myConnection.Close();
                }
                else if (logsel == null)
                {
                    SolidColorBrush Brush1 = new SolidColorBrush(Colors.Green);
                    avlbl.Foreground = Brush1;
                    avlbl.Content = "Cвободен";
                    myConnection.Close();
                }

            }
            catch
            {
                avlbl.Content = "Ошибка";
            }
        }
    }
}
