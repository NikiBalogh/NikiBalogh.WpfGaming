using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RockPaperScissors
{
    /// <summary>
    /// Interaction logic for WriteName.xaml
    /// </summary>
    public partial class WriteName : Window
    {
        public string username;
        MainWindow window;
        public WriteName()
        {
            InitializeComponent();
        }
        public WriteName(MainWindow main)
        {

            InitializeComponent();
            window = main;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            username = textboxUserinput.Text.Trim();
            window.username = username;
            Close();
        }
    }
}
