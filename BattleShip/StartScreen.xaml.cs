//-----------------------------------------------------------------------

// <copyright file="StartScreen.xaml.cs" company="CompanyName">

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
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartScreen"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not taught.")]
        public StartScreen()
        {
            InitializeComponent();
        }

        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(messageBoxText:"Names: Trik Heath, Edan Deno, Robert Jaklin, Alberto Ortiz Aguilar." + "\n" + "Version: 0.1" + "\n" + "Class: #22547 It:Program:Part 2 (C#)", "Credits");
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
           // this.Close();
            PlayerSelect objPlayerSelect = new PlayerSelect();
            objPlayerSelect.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Rules description", "Battleship Rules");
        }
    }
}
