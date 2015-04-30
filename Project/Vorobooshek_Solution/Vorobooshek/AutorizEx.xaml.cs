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

namespace Vorobooshek
{
    /// <summary>
    /// Логика взаимодействия для AutorizEx.xaml
    /// </summary>
    public partial class AutorizEx : Window
    {
        public AutorizEx()
        {
            InitializeComponent();
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

            Environment.Exit(0);
        }

        private void Rectangle_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Color colorize = (Color)ColorConverter.ConvertFromString("#bdc3c7");
            SolidColorBrush myBrush = new SolidColorBrush(colorize);
            (sender as Rectangle).Fill = myBrush;

            this.Close();
        }
    }
}
