//-----------------------------------------------------------------------
// <copyright file="MainWindow.xmal.cs" company="Our Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace BattleShip 
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public partial class MainWindow : Window 
    {
        public MainWindow() 
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Current "Game state", basically what window we are on.
        /// </summary>
        private gState GameState = gState.Start;

        /// <summary>
        /// Current "Game state", basically what window we are on.
        /// </summary>
        private enum gState 
        {
            Start,
            PlayerSelect,
            ShipPlacement,
            Battle
        }

        /// <summary>
        /// Array with all the letters of the alphabet used to create the "coordinates"
        /// </summary>
        private string[] AlphArray = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// <summary>
        /// Size of the battlefield.
        /// </summary>
        private int battlefieldSize = 10;

        /// <summary>
        /// Array used to keep track of highlighting the grid when placing a ship
        /// </summary>
        private Rectangle[,] setupGridArray;

        /// <summary>
        /// Player 1
        /// </summary>
        private Player player1 = new Player();

        /// <summary>
        /// Player 2
        /// </summary>
        private Player player2 = new Player();

        /// <summary>
        /// Starting ship
        /// </summary>
        private Ship[] startingShips = new Ship[5];

        /// <summary>
        /// Current Player Turn
        /// </summary>
        private Player CurrentTurn;

        /// <summary>
        /// Selected BattleShip
        /// </summary>
        private Ship SelectedShip;

        /// <summary>
        /// If it's possible to place ship where they position
        /// </summary>
        private bool canPlaceShip = true;

        private List<Ship> lastShipPlacedList = new List<Ship>();

        private Color GetColor(string colorHex) 
        {
            Color color = (Color)ColorConverter.ConvertFromString(colorHex);
            return color;
        }

        /// <summary>
        /// The Method to Change the Game State To show Start, Player, Select Ship, Placement, and Battle Screens
        /// </summary>
        private void ChangeGameState(gState state) 
        {
            this.GameState = state;

            switch (this.GameState) 
            {
                case gState.Start:
                    canStartScreen.Visibility = Visibility.Visible;
                    canShipSetup.Visibility = Visibility.Collapsed;
                    canPlayerSelect.Visibility = Visibility.Collapsed;
                    canBattleScreen.Visibility = Visibility.Collapsed;
                    break;
                case gState.PlayerSelect:
                    canStartScreen.Visibility = Visibility.Collapsed;
                    canShipSetup.Visibility = Visibility.Collapsed;
                    canPlayerSelect.Visibility = Visibility.Visible;
                    canBattleScreen.Visibility = Visibility.Collapsed;
                    break;
                case gState.ShipPlacement:
                    canStartScreen.Visibility = Visibility.Collapsed;
                    canShipSetup.Visibility = Visibility.Visible;
                    canPlayerSelect.Visibility = Visibility.Collapsed;
                    canBattleScreen.Visibility = Visibility.Collapsed;
                    break;
                case gState.Battle:
                    canStartScreen.Visibility = Visibility.Collapsed;
                    canShipSetup.Visibility = Visibility.Collapsed;
                    canPlayerSelect.Visibility = Visibility.Collapsed;
                    canBattleScreen.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void LoadShips() 
        {
            // <summary>
            // This is temporary and will be changed when we implement the settings screen
            // </summary>
            for (int i = 0; i < this.startingShips.Length; i++) 
            {
                Ship newShip = new Ship();
                newShip.type = (Ship.shipType)i;
                newShip.SetLenght();
                this.startingShips[i] = newShip;
            }

        }

        // <summary>
        // This method will set up the grid. 
        // </summary>
        private void SetupGrid(Grid grid) 
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            grid.ShowGridLines = true;
            double cellSize = 55;
            GridLength gSize = new GridLength(cellSize);

            for(int i = 0; i < this.battlefieldSize; i++) 
            { 
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = gSize;
                grid.ColumnDefinitions.Add(colDef);

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = gSize;
                grid.RowDefinitions.Add(rowDef);
            }

            grid.Height = cellSize * this.battlefieldSize;
            grid.Width = cellSize * this.battlefieldSize;

            if(grid == this.grid_ShipSetup) 
            { 
                for (int x = 0; x < this.battlefieldSize; x++) 
                {
                    for (int y = 0; y < this.battlefieldSize; y++) 
                    {
                        Rectangle Rect = new Rectangle();
                        Rect.Name = this.AlphArray[x] + y.ToString();
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = this.GetColor("#79dced");
                        Rect.Fill = mySolidColorBrush;
                        Grid.SetRow(Rect, x);
                        Grid.SetColumn(Rect, y);
                        grid.Children.Add(Rect);
                        Rect.MouseDown += this.SetUpGrid_MouseDown;
                        Rect.MouseEnter += this.SetUpGrid_MouseEnteredSquare;
                        Rect.MouseLeave += this.SetUpGrid_MouseLeftSquare;
                        this.setupGridArray[x, y] = Rect;
                    }
                }
            } 

        }

        private void UpdateBattlefieldColors()
        {
            for (int x = 0; x < this.battlefieldSize; x++)
            {
                for (int y = 0; y < this.battlefieldSize; y++)
                {
                    Ship getShip = this.CurrentTurn.Board.ShipGrid[x, y];
                    if (getShip != null)
                    {
                        this.setupGridArray[x, y].Fill = getShip.ShipColor;
                    }
                }
            }
        }

        // <summary>
        // This method will Reset the Player Ships 
        // </summary>
        private void ResetPlayerShips()
        {
            foreach (Ship p1Ship in this.player1.CurrentShips)
            {
                p1Ship.isPlaced = false;
                p1Ship.isSunk = false;
            }

            foreach (Ship p2Ship in this.player1.CurrentShips)
            {
                p2Ship.isPlaced = false;
                p2Ship.isSunk = false;
            }
        }

        private void MainCanvas_OnLoad(object sender, RoutedEventArgs e) 
        {
            this.ChangeGameState(gState.Start);
            this.LoadShips();
            this.setupGridArray = new Rectangle[this.battlefieldSize, this.battlefieldSize];
        }

        #region Start Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]
        private void StartButton_Click(object sender, RoutedEventArgs e) 
        {
            this.ChangeGameState(gState.PlayerSelect);
        }

        private void CreditsButton_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show(messageBoxText: "Names: Trik Heath, Edan Deno, Robert Jaklin, Alberto Ortiz Aguilar." + "\n" + "Version: 0.1" + "\n" + "Class: #22547 It:Program:Part 2 (C#)", "Credits");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) 
        {
            this.Close();
        }

        private void Rules_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show("Rules description", "Battleship Rules");
        }

        #endregion

        #region Player Select Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]
        private void BackButton_Click(object sender, RoutedEventArgs e) 
        {
            this.ChangeGameState(gState.Start);
        }

        private void StartPvp_Click(object sender, RoutedEventArgs e) 
        {
            if (txtPlayerOneName.Text == string.Empty) 
            {
                MessageBox.Show("Please enter a name for Player 1");
                return;
            }
            this.player1.Name = txtPlayerOneName.Text;
            this.player1.Type = "Player";
            this.player1.CurrentShips = this.startingShips.ToList();
            this.player1.Board = new Battlefield(this.battlefieldSize);


            if (txtPlayerTwoName.Text == string.Empty) 
            {
                MessageBox.Show("Please enter a name for Player 2");
                return;
            }
            this.player2.Name = txtPlayerTwoName.Text;
            this.player2.Type = "Player";
            this.player2.CurrentShips = this.startingShips.ToList();
            this.player2.Board = new Battlefield(this.battlefieldSize);
            
            this.CurrentTurn = this.player1;
            this.ChangeGameState(gState.ShipPlacement);
        }

        private void StartPvpVsCPU_Click(object sender, RoutedEventArgs e) 
        {
            if (txtPlayerOneName_Vs_AI.Text == string.Empty) 
            {
                MessageBox.Show("Please enter a name for Player 1");
                return;
            }
            this.player1.Name = txtPlayerOneName_Vs_AI.Text;
            this.player1.Type = "Player";
            this.player1.CurrentShips = this.startingShips.ToList();
            this.player1.Board = new Battlefield(this.battlefieldSize);


            this.player2.Name = "BATTLEFIELD_BOT_V2.0";
            this.player2.Type = "CPU";
            this.player2.CurrentShips = this.startingShips.ToList();
            this.player2.Board = new Battlefield(this.battlefieldSize);

            this.CurrentTurn = this.player1;
            this.ChangeGameState(gState.ShipPlacement);
        }

        private void Settings_Click(object sender, RoutedEventArgs e) 
        {

        }

        #endregion

        #region Ship Placement Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]

        private void canShipSetup_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) 
        {
            ShipSetupListBox.Items.Clear();
            foreach (Ship getShip in this.player1.CurrentShips) 
            {
                ShipSetupListBox.Items.Add(getShip.GetName());
            }

            this.SetupGrid(this.grid_ShipSetup);
            this.SelectedShip = null;
            Array.Clear(this.CurrentTurn.Board.ShipGrid, 0, this.setupGridArray.Length);
            this.ResetPlayerShips();
        }

        private void ShipSetupListBox_Selected(object sender, SelectionChangedEventArgs e) 
        {
            foreach (Ship getShip in this.player1.CurrentShips) 
            {
                if (ShipSetupListBox.SelectedItem != null) 
                { 
                    if (ShipSetupListBox.SelectedItem.ToString() == getShip.GetName()) 
                    {
                        this.SelectedShip = getShip;
                    }
                }
            }
        }

        /// <summary>
        /// Goes Back to Player Select Screen
        /// </summary>
        private void SetUpBckBtn_Click(object sender, RoutedEventArgs e) 
        {
            this.ChangeGameState(gState.PlayerSelect);
        }

        /// <summary>
        /// Method to Rotate Ship placement 
        /// </summary>
        private void btn_RotateShip_Click(object sender, RoutedEventArgs e) 
        {
            if (this.SelectedShip != null && !this.SelectedShip.isPlaced) 
            {
                if (this.SelectedShip.rotation == "Horizontal") 
                {
                    this.SelectedShip.rotation = "Vertical";
                } 
                else 
                {
                    this.SelectedShip.rotation = "Horizontal";
                }
            }
        }

        private void SetUpUndoBtn_Click(object sender, RoutedEventArgs e) 
        {
            if (this.lastShipPlacedList.Count > 0)
            {
                Ship lastShip = this.lastShipPlacedList.Last();
                lastShip.isPlaced = false;
                int x = lastShip.origin[0];
                int y = lastShip.origin[1];
                SolidColorBrush Brush = new SolidColorBrush();
                Brush.Color = this.GetColor("#79dced");
                if (lastShip.rotation == "Horizontal")
                {
                    for (int j = 0; j < lastShip.length; j++)
                    {
                        this.CurrentTurn.Board.ShipGrid[x, y + j] = null;
                        this.setupGridArray[x, y + j].Fill = Brush;
                    }
                }
                else if (lastShip.rotation == "Vertical")
                {
                    for (int j = 0; j < lastShip.length; j++)
                    {
                        this.CurrentTurn.Board.ShipGrid[x + j, y] = null;
                        this.setupGridArray[x + j, y].Fill = Brush;
                    }
                }
                Array.Clear(lastShip.origin, 0, lastShip.origin.Length);
                this.UpdateBattlefieldColors();
                this.lastShipPlacedList.RemoveAt(this.lastShipPlacedList.Count -1);

                
            }
        }

        private void SetUpResetBtn_Click(object sender, RoutedEventArgs e) 
        {
            Array.Clear(this.CurrentTurn.Board.ShipGrid, 0, this.CurrentTurn.Board.ShipGrid.Length);
            foreach (Ship getShip in this.CurrentTurn.CurrentShips)
            {
                getShip.isPlaced = false;
                getShip.isSunk = false;
            }
            this.lastShipPlacedList.Clear();

            SolidColorBrush Brush = new SolidColorBrush();
            Brush.Color = this.GetColor("#79dced");
            for (int x = 0; x < this.battlefieldSize; x++)
            {
                for (int y = 0; y < this.battlefieldSize; y++)
                {
                    this.setupGridArray[x, y].Fill = Brush;
                }
            }
        }

        private void btn_deployShips_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Ship Placement Grid Functionality [[-----------------------------------------------------------------------------------------------------------------]]

        private bool CheckForOtherShips(int x, int y)
        {
            if(CurrentTurn.Board.ShipGrid[x, y] != null)
            {
                return false;
            } else
            {
                return true;
            }
        }

        private void SetUpGrid_MouseLeftSquare(object sender, MouseEventArgs e) 
        {
            if (this.SelectedShip == null) 
            {
                Debug.WriteLine("NO SHIP SELECTED");
                return;
            }
            SolidColorBrush Brush = new SolidColorBrush();
            Brush.Color = this.GetColor("#79dced");
            Rectangle rect = (Rectangle)e.Source;
            rect.Fill = Brush;

            //fill the others
            int x = 0;
            int y = 0;

            int i = 0;
            foreach (string letter in this.AlphArray) 
            {
                if (rect.Name[0] == letter[0]) 
                {
                    x = i;
                    break;
                }
                i++;
            }

            y = rect.Name[1] - '0';



            if (this.SelectedShip.rotation == "Horizontal") 
            {
                for (int j = 1; j < this.SelectedShip.length; j++) 
                {
                    if ((y + j) < this.battlefieldSize) 
                    {
                        Rectangle shipRect = this.setupGridArray[x, y + j];
                        shipRect.Fill = Brush;
                    }
                }
            }

            if (this.SelectedShip.rotation == "Vertical") 
            {
                for (int j = 1; j < this.SelectedShip.length; j++) 
                {
                    if ((x + j) < this.battlefieldSize) {
                        Rectangle shipRect = this.setupGridArray[x + j, y];
                        shipRect.Fill = Brush;
                    }
                }
            }

            this.UpdateBattlefieldColors();
        }

        private void SetUpGrid_MouseEnteredSquare(object sender, MouseEventArgs e) 
        {
            if (this.SelectedShip == null) 
            {
                Debug.WriteLine("NO SHIP SELECTED");
                return;
            }

            this.UpdateBattlefieldColors();

            SolidColorBrush Brush = new SolidColorBrush();
            if (this.SelectedShip.isPlaced) 
            {
                Brush.Color = this.GetColor("#ff0800");
            } else {
                Brush.Color = this.GetColor("#edffef");
            }
            
            Rectangle rect = (Rectangle)e.Source;

            //fill the others
            int x = 0;
            int y = 0;

            int i = 0;
            foreach (string letter in AlphArray) 
            {
                if (rect.Name[0] == letter[0]) 
                {
                    x = i;
                    break;
                }
                i++;
            }

            y = rect.Name[1] - '0';



            if (this.SelectedShip.rotation == "Horizontal") 
            {
                for (int j = 0; j < this.SelectedShip.length; j++) 
                {
                    int newY = 0;
                    if ((y + j) >= this.battlefieldSize) 
                    {
                        Brush.Color = this.GetColor("#ff0800");
                        newY = this.battlefieldSize - 1;
                        this.canPlaceShip = false;
                    }
                    else {
                        newY = y + j;
                        this.canPlaceShip = true;
                    }
                    if(!this.CheckForOtherShips(x, newY))
                    {
                        Brush.Color = this.GetColor("#ff0800");
                        this.canPlaceShip = false;
                    }
                    Rectangle shipRect = setupGridArray[x, newY];
                    shipRect.Fill = Brush;
                }
            }
            else if (this.SelectedShip.rotation == "Vertical") 
            {
                for (int j = 0; j < this.SelectedShip.length; j++) 
                {
                    int newX = 0;
                    if ((x + j) >= this.battlefieldSize) 
                    {
                        Brush.Color = this.GetColor("#ff0800");
                        newX = this.battlefieldSize - 1;
                        this.canPlaceShip = false;
                    }
                    else {
                        newX = x + j;
                        this.canPlaceShip = true;
                    }
                    if (!this.CheckForOtherShips(newX, y))
                    {
                        Brush.Color = this.GetColor("#ff0800");
                        this.canPlaceShip = false;
                    }

                    Rectangle shipRect = this.setupGridArray[newX, y];
                    shipRect.Fill = Brush;
                }
            }

            rect.Fill = Brush;

            
        }

        private void SetUpGrid_MouseDown(object sender, MouseButtonEventArgs e) 
        {
            if (this.SelectedShip == null) 
            {
                return;
            }

            if(!this.SelectedShip.isPlaced && this.canPlaceShip) 
            {
                //setting the origin of the ship being placed
                Rectangle rect = (Rectangle)e.Source;

                int x = 0;
                int y = 0;

                int i = 0;
                foreach (string letter in this.AlphArray)
                {
                    if (rect.Name[0] == letter[0])
                    {
                        x = i;
                        break;
                    }
                    i++;
                }

                y = rect.Name[1] - '0';

                this.SelectedShip.origin[0] = x;
                this.SelectedShip.origin[1] = y;
                this.SelectedShip.isPlaced = true;

                if (this.SelectedShip.rotation == "Horizontal")
                {
                    for (int j = 0; j < this.SelectedShip.length; j++)
                    {
                        this.CurrentTurn.Board.ShipGrid[x, y + j] = this.SelectedShip;
                    }
                }
                else if (this.SelectedShip.rotation == "Vertical")
                {
                    for (int j = 0; j < this.SelectedShip.length; j++)
                    {
                        this.CurrentTurn.Board.ShipGrid[x + j, y] = this.SelectedShip;
                    }
                }

            }

            this.lastShipPlacedList.Add(this.SelectedShip);

            //updates the colors on the battlefield from the grid data
            this.UpdateBattlefieldColors();
        }

        #endregion
 
    }
}
