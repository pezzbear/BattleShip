// <copyright file=this.file company="BattleShip Team">
// </copyright>
// <author>Robert,Alberto,Trik,Edan</author>
namespace BattleShip
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
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
    /// Interaction logic for SetUpScreen.xaml
    /// </summary>
    public partial class SetUpScreen : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetUpScreen"/> class
        /// </summary>
        public SetUpScreen()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event that enables the grid buttons on selection can use methods and classes to grab list items and configure the highlights and buttons pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DestroyerShipItem_Selected(object sender, RoutedEventArgs e)
        {
            A1Btn.IsEnabled = true;
            A2Btn.IsEnabled = true;
            A3Btn.IsEnabled = true;
            A4Btn.IsEnabled = true;
        }

        private void A1Btn_Click(object sender, RoutedEventArgs e)
        {
            {
                if (!DestroyerShipItem.IsSelected) return;
                A1Btn.Opacity = +99;
                A1Btn.Background = new SolidColorBrush(Colors.Aqua);

                A2Btn.Opacity = +99;
                A2Btn.Background = new SolidColorBrush(Colors.Aqua);
            }
        }
    }
}
