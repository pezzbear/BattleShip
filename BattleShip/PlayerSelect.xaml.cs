//-----------------------------------------------------------------------

// <copyright file="PlayerSelect.xaml.cs" company="CompanyName">

////    Company copyright tag.

// </copyright>

//-----------------------------------------------------------------------
namespace BattleShip
{
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

    /// <summary>
    /// Interaction logic for PlayerSelect.xaml
    /// </summary>
    
    public partial class PlayerSelect : Window
    {
        public PlayerSelect()
        {
            InitializeComponent();
        }

        private void StartPvp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartPvpVsCPU_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            StartScreen objStartScreen = new StartScreen();
            objStartScreen.Show();
        }

        private void Rules_Click2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Rules description", "Battleship Rules");
        }
    }
}
