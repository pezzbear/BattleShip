//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Our Team">
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
        /// <summary>
        /// Current "Game state", basically what window we are on.
        /// </summary>
        private GState gameState = GState.Start;

        /// <summary>
        /// Array with all the letters of the alphabet used to create the "coordinates"
        /// </summary>
        private string[] alphArray = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

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
        private Player currentTurn;

        /// <summary>
        /// Selected BattleShip
        /// </summary>
        private Ship selectedShip;

        /// <summary>
        /// If it's possible to place ship where they position
        /// </summary>
        private bool canPlaceShip = true;

        /// <summary>
        /// takes a list of the last ship placed
        /// </summary>
        private List<Ship> lastShipPlacedList = new List<Ship>();

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Current "Game state", basically what window we are on.
        /// </summary>
        private enum GState
        {
            /// <summary>
            /// states that the game state is at its Start Stage
            /// </summary>
            Start,

            /// <summary>
            /// states that the game state is in the Player Select Stage
            /// </summary>
            PlayerSelect,

            /// <summary>
            /// states that the game state is in the ShipPlacement Stage
            /// </summary>
            ShipPlacement,

            /// <summary>
            /// states that the game state is in the Battle Stage
            /// </summary>
            Battle
        }

        /// <summary>
        /// the following allows for color to be properly converted using string colorHex
        /// </summary>
        /// <param name="colorHex">Hex String</param>
        /// <returns>The new Color.</returns>
        private Color GetColor(string colorHex) 
        {
            Color color = (Color)ColorConverter.ConvertFromString(colorHex);
            return color;
        }

        /// <summary>
        /// The Method to Change the Game State To show Start, Player, Select Ship, Placement, and Battle Screens
        /// using the GState.
        /// </summary>
        /// <param name="state">Game state to change to</param>
        private void ChangeGameState(GState state) 
        {
            this.gameState = state;

            switch (this.gameState) 
            {
                case GState.Start:
                    canStartScreen.Visibility = Visibility.Visible;
                    canShipSetup.Visibility = Visibility.Collapsed;
                    canPlayerSelect.Visibility = Visibility.Collapsed;
                    canBattleScreen.Visibility = Visibility.Collapsed;
                    break;
                case GState.PlayerSelect:
                    canStartScreen.Visibility = Visibility.Collapsed;
                    canShipSetup.Visibility = Visibility.Collapsed;
                    canPlayerSelect.Visibility = Visibility.Visible;
                    canBattleScreen.Visibility = Visibility.Collapsed;
                    break;
                case GState.ShipPlacement:
                    canStartScreen.Visibility = Visibility.Collapsed;
                    canShipSetup.Visibility = Visibility.Visible;
                    canPlayerSelect.Visibility = Visibility.Collapsed;
                    canBattleScreen.Visibility = Visibility.Collapsed;
                    break;
                case GState.Battle:
                    canStartScreen.Visibility = Visibility.Collapsed;
                    canShipSetup.Visibility = Visibility.Collapsed;
                    canPlayerSelect.Visibility = Visibility.Collapsed;
                    canBattleScreen.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// this allows for the ships to be properly loaded into the game
        /// </summary>
        private void LoadShips() 
        {
            // <summary>
            // This is temporary and will be changed when we implement the settings screen
            // </summary>
            for (int i = 0; i < this.startingShips.Length; i++) 
            {
                Ship newShip = new Ship();
                newShip.Type = (Ship.ShipType)i;
                newShip.SetLenght();
                this.startingShips[i] = newShip;
            }
        }
       
        /// <summary>
        /// This method will set up the grid. 
        /// </summary>
        /// <param name="grid">Grid to setup</param>
        private void SetupGrid(Grid grid) 
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            grid.ShowGridLines = true;
            double cellSize = 55;
            GridLength gSize = new GridLength(cellSize);

            for (int i = 0; i < this.battlefieldSize; i++) 
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

            if (grid == this.grid_ShipSetup) 
            { 
                for (int x = 0; x < this.battlefieldSize; x++) 
                {
                    for (int y = 0; y < this.battlefieldSize; y++) 
                    {
                        Rectangle rect = new Rectangle();
                        rect.Name = this.alphArray[x] + y.ToString();
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = this.GetColor("#79dced");
                        rect.Fill = mySolidColorBrush;
                        Grid.SetRow(rect, x);
                        Grid.SetColumn(rect, y);
                        grid.Children.Add(rect);
                        rect.MouseDown += this.SetUpGrid_MouseDown;
                        rect.MouseEnter += this.SetUpGrid_MouseEnteredSquare;
                        rect.MouseLeave += this.SetUpGrid_MouseLeftSquare;
                        this.setupGridArray[x, y] = rect;
                    }
                }
            }
        }

        /// <summary>
        /// UpdateBattlefieldColors allow for the grid colors to update as the match goes on
        /// </summary>
        private void UpdateBattlefieldColors()
        {
            for (int x = 0; x < this.battlefieldSize; x++)
            {
                for (int y = 0; y < this.battlefieldSize; y++)
                {
                    Ship getShip = this.currentTurn.Board.ShipGrid[x, y];
                    if (getShip != null)
                    {
                        this.setupGridArray[x, y].Fill = getShip.ShipColor;
                    }
                }
            }
        }

        /// <summary>
        /// This method will Reset the Player Ships 
        /// </summary>
        private void ResetPlayerShips()
        {
            foreach (Ship playerShip in this.player1.CurrentShips)
            {
                playerShip.IsPlaced = false;
                playerShip.IsSunk = false;
            }

            foreach (Ship playerShip in this.player1.CurrentShips)
            {
                playerShip.IsPlaced = false;
                playerShip.IsSunk = false;
            }
        }

        /// <summary>
        /// allows for the Main Canvas to make the proper addition of game elements when loaded in
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void MainCanvas_OnLoad(object sender, RoutedEventArgs e) 
        {
            this.ChangeGameState(GState.Start);
            this.LoadShips();
            this.setupGridArray = new Rectangle[this.battlefieldSize, this.battlefieldSize];
        }

        #region Start Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]

        /// <summary>
        /// the StartButton_Click Event which activates the ChangeGameState to the GState PlayerSelect
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void StartButton_Click(object sender, RoutedEventArgs e) 
        {
            this.ChangeGameState(GState.PlayerSelect);
        }

        /// <summary>
        /// the CreditsButton_Click Event which summons a message box
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void CreditsButton_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show(messageBoxText: "Names: Trik Heath, Edan Deno, Robert Jaklin, Alberto Ortiz Aguilar." + "\n" + "Version: 0.1" + "\n" + "Class: #22547 It:Program:Part 2 (C#)", "Credits");
        }

        /// <summary>
        /// the ExitButton_Click Event which will close the application
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void ExitButton_Click(object sender, RoutedEventArgs e) 
        {
            this.Close();
        }

        /// <summary>
        /// the Rules Button Click Event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void Rules_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show("Rules description", "Battleship Rules");
        }

        #endregion

        #region Player Select Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]

        /// <summary>
        /// The Back Button Click Event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void BackButton_Click(object sender, RoutedEventArgs e) 
        {
            this.ChangeGameState(GState.Start);
        }

        /// <summary>
        /// The StartPvp Button Click Event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
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
            
            this.currentTurn = this.player1;
            this.ChangeGameState(GState.ShipPlacement);
        }

        /// <summary>
        /// The StartPvpVsCPU Click Event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
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

            this.currentTurn = this.player1;
            this.ChangeGameState(GState.ShipPlacement);
        }

        /// <summary>
        /// Method to get the Settings Panel out
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void Settings_Click(object sender, RoutedEventArgs e) 
        {
        }

        #endregion

        #region Ship Placement Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]

        /// <summary>
        /// Method to make ShipSetup Visible
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void CanShipSetup_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) 
        {
            ShipSetupListBox.Items.Clear();
            foreach (Ship getShip in this.player1.CurrentShips) 
            {
                ShipSetupListBox.Items.Add(getShip.GetName());
            }

            this.SetupGrid(this.grid_ShipSetup);
            this.selectedShip = null;
            Array.Clear(this.currentTurn.Board.ShipGrid, 0, this.setupGridArray.Length);
            this.ResetPlayerShips();
        }

        /// <summary>
        /// Allows for a list box of all the ship options
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void ShipSetupListBox_Selected(object sender, SelectionChangedEventArgs e) 
        {
            foreach (Ship getShip in this.player1.CurrentShips) 
            {
                if (ShipSetupListBox.SelectedItem != null) 
                { 
                    if (ShipSetupListBox.SelectedItem.ToString() == getShip.GetName()) 
                    {
                        this.selectedShip = getShip;
                    }
                }
            }
        }

        /// <summary>
        /// Goes Back to Player Select Screen
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void SetUpBckBtn_Click(object sender, RoutedEventArgs e) 
        {
            this.ChangeGameState(GState.PlayerSelect);
        }

        /// <summary>
        /// Method to Rotate Ship placement 
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void Btn_RotateShip_Click(object sender, RoutedEventArgs e) 
        {
            if (this.selectedShip != null && !this.selectedShip.IsPlaced) 
            {
                if (this.selectedShip.Rotation == "Horizontal") 
                {
                    this.selectedShip.Rotation = "Vertical";
                } 
                else 
                {
                    this.selectedShip.Rotation = "Horizontal";
                }
            }
        }

        /// <summary>
        /// Method to Undo Ship placement
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void SetUpUndoBtn_Click(object sender, RoutedEventArgs e) 
        {
            if (this.lastShipPlacedList.Count > 0)
            {
                Ship lastShip = this.lastShipPlacedList.Last();
                lastShip.IsPlaced = false;
                int x = lastShip.Origin[0];
                int y = lastShip.Origin[1];
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = this.GetColor("#79dced");
                if (lastShip.Rotation == "Horizontal")
                {
                    for (int j = 0; j < lastShip.Length; j++)
                    {
                        this.currentTurn.Board.ShipGrid[x, y + j] = null;
                        this.setupGridArray[x, y + j].Fill = brush;
                    }
                }
                else if (lastShip.Rotation == "Vertical")
                {
                    for (int j = 0; j < lastShip.Length; j++)
                    {
                        this.currentTurn.Board.ShipGrid[x + j, y] = null;
                        this.setupGridArray[x + j, y].Fill = brush;
                    }
                }

                Array.Clear(lastShip.Origin, 0, lastShip.Origin.Length);
                this.UpdateBattlefieldColors();
                this.lastShipPlacedList.RemoveAt(this.lastShipPlacedList.Count - 1);
            }
        }

        /// <summary>
        /// Button Click event that allows the player to reset their ship placements
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void SetUpResetBtn_Click(object sender, RoutedEventArgs e) 
        {
            Array.Clear(this.currentTurn.Board.ShipGrid, 0, this.currentTurn.Board.ShipGrid.Length);
            foreach (Ship getShip in this.currentTurn.CurrentShips)
            {
                getShip.IsPlaced = false;
                getShip.IsSunk = false;
            }

            this.lastShipPlacedList.Clear();

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = this.GetColor("#79dced");

            for (int x = 0; x < this.battlefieldSize; x++)
            {
                for (int y = 0; y < this.battlefieldSize; y++)
                {
                    this.setupGridArray[x, y].Fill = brush;
                }
            }
        }

        /// <summary>
        /// Confirms the placements of all placed ships
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void Btn_deployShips_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region Ship Placement Grid Functionality [[-----------------------------------------------------------------------------------------------------------------]]

        /// <summary>
        /// a boolean to check for the positions of all the ships and store said data
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>True or False</returns>
        private bool CheckForOtherShips(int x, int y)
        {
            if (this.currentTurn.Board.ShipGrid[x, y] != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks when the mouse leaves a grid square
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void SetUpGrid_MouseLeftSquare(object sender, MouseEventArgs e) 
        {
            if (this.selectedShip == null) 
            {
                Debug.WriteLine("NO SHIP SELECTED");
                return;
            }

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = this.GetColor("#79dced");
            Rectangle rect = (Rectangle)e.Source;
            rect.Fill = brush;

            ////fill the others
            int x = 0;
            int y = 0;

            int i = 0;
            foreach (string letter in this.alphArray) 
            {
                if (rect.Name[0] == letter[0]) 
                {
                    x = i;
                    break;
                }

                i++;
            }

            y = rect.Name[1] - '0';

            if (this.selectedShip.Rotation == "Horizontal") 
            {
                for (int j = 1; j < this.selectedShip.Length; j++) 
                {
                    if ((y + j) < this.battlefieldSize) 
                    {
                        Rectangle shipRect = this.setupGridArray[x, y + j];
                        shipRect.Fill = brush;
                    }
                }
            }

            if (this.selectedShip.Rotation == "Vertical") 
            {
                for (int j = 1; j < this.selectedShip.Length; j++) 
                {
                    if ((x + j) < this.battlefieldSize) 
                    {
                        Rectangle shipRect = this.setupGridArray[x + j, y];
                        shipRect.Fill = brush;
                    }
                }
            }

            this.UpdateBattlefieldColors();
        }

        /// <summary>
        /// Checks when the mouse enters a grid square
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void SetUpGrid_MouseEnteredSquare(object sender, MouseEventArgs e) 
        {
            if (this.selectedShip == null) 
            {
                Debug.WriteLine("NO SHIP SELECTED");
                return;
            }

            this.UpdateBattlefieldColors();

            SolidColorBrush brush = new SolidColorBrush();
            if (this.selectedShip.IsPlaced) 
            {
                brush.Color = this.GetColor("#ff0800");
            }
            else 
            {
                brush.Color = this.GetColor("#edffef");
            }
            
            Rectangle rect = (Rectangle)e.Source;

            ////fill the others
            int x = 0;
            int y = 0;

            int i = 0;
            foreach (string letter in this.alphArray) 
            {
                if (rect.Name[0] == letter[0]) 
                {
                    x = i;

                    break;
                }

                i++;
            }

            y = rect.Name[1] - '0';

            if (this.selectedShip.Rotation == "Horizontal") 
            {
                for (int j = 0; j < this.selectedShip.Length; j++) 
                {
                    int newY = 0;
                    if ((y + j) >= this.battlefieldSize) 
                    {
                        brush.Color = this.GetColor("#ff0800");
                        newY = this.battlefieldSize - 1;
                        this.canPlaceShip = false;
                    }
                    else 
                    {
                        newY = y + j;
                        this.canPlaceShip = true;
                    }

                    if (!this.CheckForOtherShips(x, newY))
                    {
                        brush.Color = this.GetColor("#ff0800");

                        this.canPlaceShip = false;
                    }

                    Rectangle shipRect = this.setupGridArray[x, newY];
                    shipRect.Fill = brush;
                }
            }
            else if (this.selectedShip.Rotation == "Vertical") 
            {
                for (int j = 0; j < this.selectedShip.Length; j++) 
                {
                    int newX = 0;
                    if ((x + j) >= this.battlefieldSize) 
                    {
                        brush.Color = this.GetColor("#ff0800");
                        newX = this.battlefieldSize - 1;
                        this.canPlaceShip = false;
                    }
                    else 
                    {
                        newX = x + j;
                        this.canPlaceShip = true;
                    }

                    if (!this.CheckForOtherShips(newX, y))
                    {
                        brush.Color = this.GetColor("#ff0800");
                        this.canPlaceShip = false;
                    }

                    Rectangle shipRect = this.setupGridArray[newX, y];
                    shipRect.Fill = brush;
                }
            }

            rect.Fill = brush;
        }

        /// <summary>
        /// Checks when the mouse clicks a grid square
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void SetUpGrid_MouseDown(object sender, MouseButtonEventArgs e) 
        {
            if (this.selectedShip == null) 
            {
                return;
            }

            if (!this.selectedShip.IsPlaced && this.canPlaceShip) 
            {
                ////setting the origin of the ship being placed
                Rectangle rect = (Rectangle)e.Source;

                int x = 0;
                int y = 0;

                int i = 0;
                foreach (string letter in this.alphArray)
                {
                    if (rect.Name[0] == letter[0])
                    {
                        x = i;
                        break;
                    }

                    i++;
                }

                y = rect.Name[1] - '0';
                this.selectedShip.Origin[0] = x;
                this.selectedShip.Origin[1] = y;
                this.selectedShip.IsPlaced = true;

                if (this.selectedShip.Rotation == "Horizontal")
                {
                    for (int j = 0; j < this.selectedShip.Length; j++)
                    {
                        this.currentTurn.Board.ShipGrid[x, y + j] = this.selectedShip;
                    }
                }
                else if (this.selectedShip.Rotation == "Vertical")
                {
                    for (int j = 0; j < this.selectedShip.Length; j++)
                    {
                        this.currentTurn.Board.ShipGrid[x + j, y] = this.selectedShip;
                    }
                }
            }

            this.lastShipPlacedList.Add(this.selectedShip);

            //// the colors on the battlefield from the grid data
            this.UpdateBattlefieldColors();
        }
        #endregion
    }
}
