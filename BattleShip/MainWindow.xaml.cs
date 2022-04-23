//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Our Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel;

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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules",
        "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Current "Game state", basically what window we are on.
        /// </summary>
        private GState gameState = GState.Start;

        /// <summary>
        /// Array with all the letters of the alphabet used to create the "coordinates"
        /// </summary>
        private string[] alphArray =
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y", "Z"
        };

        /// <summary>
        /// Size of the battlefield.
        /// </summary>
        private int battlefieldSize = 10;

        /// <summary>
        /// Array of Rectangles used to change the colors of the xmal Grid (Setup ship grid and the battlefield grid)
        /// </summary>
        private Rectangle[,] battleFieldGridArray;

        ///<summary>
        /// Stores the grid coordinates where the players wants to fire their shot
        ///</summary>
        public int[] cellToShoot = new int[2];

        ///<summary>
        /// This is where the player wants to fire
        ///</summary>
        public bool isCellSelected = false;

        ///<summary>
        /// If the player has shot this turn.
        ///</summary>
        public bool hasPlayerShot = false;

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
        /// Current Player Turn
        /// </summary>
        private Player currentEnemy;

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
        private SolidColorBrush GetColor(string colorHex)
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            Color color = (Color)ColorConverter.ConvertFromString(colorHex);
            mySolidColorBrush.Color = color;
            return mySolidColorBrush;
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
            Ship newShip;
            for (int i = 0; i < this.startingShips.Length; i++)
            {
                newShip = new Ship();
                newShip.Type = (Ship.ShipType)i;
                newShip.SetLength();
                this.player1.CurrentShips.Add(newShip);

                newShip = new Ship();
                newShip.Type = (Ship.ShipType)i;
                newShip.SetLength();
                this.player2.CurrentShips.Add(newShip);
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

                        rect.Fill = GetColor("#79dced");
                        Grid.SetRow(rect, x);
                        Grid.SetColumn(rect, y);
                        grid.Children.Add(rect);
                        rect.MouseDown += this.SetUpGrid_MouseDown;
                        rect.MouseEnter += this.SetUpGrid_MouseEnteredSquare;
                        rect.MouseLeave += this.SetUpGrid_MouseLeftSquare;
                        this.battleFieldGridArray[x, y] = rect;
                    }
                }
            }
            else if (grid == this.grid_Battlefield)
            {
                for (int x = 0; x < this.battlefieldSize; x++)
                {
                    for (int y = 0; y < this.battlefieldSize; y++)
                    {
                        Rectangle rect = new Rectangle();
                        rect.Name = this.alphArray[x] + y.ToString();
                        rect.Fill = GetColor("#79dced");
                        Grid.SetRow(rect, x);
                        Grid.SetColumn(rect, y);
                        grid.Children.Add(rect);
                        rect.MouseDown += this.BattlefieldGrid_MouseDown;
                        rect.MouseEnter += this.BattlefieldGrid_MouseEnteredSquare;
                        rect.MouseLeave += this.BattlefieldGrid_MouseLeftSquare;
                        this.battleFieldGridArray[x, y] = rect;
                    }
                }
            }
        }

        /// <summary>
        /// UpdateBattlefieldColors allow for the grid colors to update as the match goes on
        /// </summary>
        private void UpdateBattlefieldColors()
        {
            if (canShipSetup.Visibility == Visibility.Visible)
            {
                for (int x = 0; x < this.battlefieldSize; x++)
                {
                    for (int y = 0; y < this.battlefieldSize; y++)
                    {
                        Ship getShip = this.currentTurn.Board.ShipGrid[x, y];
                        if (getShip != null)
                        {
                            this.battleFieldGridArray[x, y].Fill = getShip.ShipColor;
                        }
                    }
                }
            }
            else if (canBattleScreen.Visibility == Visibility.Visible)
            {
                for (int x = 0; x < this.battlefieldSize; x++)
                {
                    for (int y = 0; y < this.battlefieldSize; y++)
                    {
                        GridData getData = this.currentTurn.Board.DataGrid[x, y];
                        switch (getData)
                        {
                            case GridData.Empty:
                                this.battleFieldGridArray[x, y].Fill = GetColor("#79dced");
                                break;
                            case GridData.Miss:
                                this.battleFieldGridArray[x, y].Fill = GetColor("#f5ff30");
                                break;
                            case GridData.Hit:
                                this.battleFieldGridArray[x, y].Fill = GetColor("#ff1c1c");
                                break;
                        }
                    }
                }
                if (isCellSelected)
                {
                    this.battleFieldGridArray[cellToShoot[0], cellToShoot[1]].Fill = GetColor("#cc66ff");
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
            this.battleFieldGridArray = new Rectangle[this.battlefieldSize, this.battlefieldSize];
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
            this.LoadShips();
        }

        /// <summary>
        /// the CreditsButton_Click Event which summons a message box
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                messageBoxText: "Names: Trik Heath, Edan Deno, Robert Jaklin, Alberto Ortiz Aguilar." + "\n" +
                                "Version: 0.1" + "\n" + "Class: #22547 It:Program:Part 2 (C#)", "Credits");
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

            this.player1.Board = new Battlefield(this.battlefieldSize);

            if (txtPlayerTwoName.Text == string.Empty)
            {
                MessageBox.Show("Please enter a name for Player 2");
                return;
            }

            this.player2.Name = txtPlayerTwoName.Text;
            this.player2.Type = "Player";

            this.player2.Board = new Battlefield(this.battlefieldSize);

            this.currentTurn = this.player1;
            Array.Clear(this.currentTurn.Board.ShipGrid, 0, this.battleFieldGridArray.Length);
            this.ResetPlayerShips();
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
            this.player1.Board = new Battlefield(this.battlefieldSize);

            this.player2.Name = "BATTLEFIELD_BOT_V2.0";
            this.player2.Type = "CPU";
            this.player2.Board = new Battlefield(this.battlefieldSize);

            this.currentTurn = this.player1;
            Array.Clear(this.currentTurn.Board.ShipGrid, 0, this.battleFieldGridArray.Length);
            this.ResetPlayerShips();
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
        }

        /// <summary>
        /// Allows for a list box of all the ship options
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void ShipSetupListBox_Selected(object sender, SelectionChangedEventArgs e)
        {
            foreach (Ship getShip in this.currentTurn.CurrentShips)
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
            if (this.currentTurn == player2)
            {
                Array.Clear(this.currentTurn.Board.ShipGrid, 0, this.currentTurn.Board.ShipGrid.Length);
                foreach (Ship getShip in this.currentTurn.CurrentShips)
                {
                    getShip.IsPlaced = false;
                    getShip.IsSunk = false;
                }

                this.lastShipPlacedList.Clear();

                selectedShip = null;

                ShipSetupListBox.Items.Clear();
                foreach (Ship getShip in this.player1.CurrentShips)
                {
                    ShipSetupListBox.Items.Add(getShip.GetName());
                }

                for (int x = 0; x < this.battlefieldSize; x++)
                {
                    for (int y = 0; y < this.battlefieldSize; y++)
                    {
                        this.battleFieldGridArray[x, y].Fill = GetColor("#79dced");
                    }
                }

                this.currentTurn = player1;

                Array.Clear(this.currentTurn.Board.ShipGrid, 0, this.battleFieldGridArray.Length);
                this.ResetPlayerShips();
                foreach (Ship getShip in this.currentTurn.CurrentShips)
                {
                    getShip.IsPlaced = false;
                    getShip.IsSunk = false;
                }

                this.lastShipPlacedList.Clear();

                for (int x = 0; x < this.battlefieldSize; x++)
                {
                    for (int y = 0; y < this.battlefieldSize; y++)
                    {
                        this.battleFieldGridArray[x, y].Fill = GetColor("#79dced");
                    }
                }

                lb_CurrentPlayerSetup.Content = "Current Turn: Player 1";
            }
            else
            {
                this.ChangeGameState(GState.PlayerSelect);
            }

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
                if (lastShip.Rotation == "Horizontal")
                {
                    for (int j = 0; j < lastShip.Length; j++)
                    {
                        this.currentTurn.Board.ShipGrid[x, y + j] = null;
                        this.battleFieldGridArray[x, y + j].Fill = GetColor("#79dced");
                    }
                }
                else if (lastShip.Rotation == "Vertical")
                {
                    for (int j = 0; j < lastShip.Length; j++)
                    {
                        this.currentTurn.Board.ShipGrid[x + j, y] = null;
                        this.battleFieldGridArray[x + j, y].Fill = GetColor("#79dced");
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

            for (int x = 0; x < this.battlefieldSize; x++)
            {
                for (int y = 0; y < this.battlefieldSize; y++)
                {
                    this.battleFieldGridArray[x, y].Fill = GetColor("#79dced");
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
            if (this.currentTurn == player1)
            {
                BlackoutScreen.Visibility = Visibility.Visible;

                var playerConfirmation = MessageBox.Show("Player 2, Press Ok When Ready", "Player Switch Initiated",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (playerConfirmation == MessageBoxResult.OK)
                {
                    this.currentTurn = player2;

                    BlackoutScreen.Visibility = Visibility.Collapsed;

                    Array.Clear(this.currentTurn.Board.ShipGrid, 0, this.currentTurn.Board.ShipGrid.Length);
                    foreach (Ship getShip in this.currentTurn.CurrentShips)
                    {
                        getShip.IsPlaced = false;
                        getShip.IsSunk = false;
                    }

                    this.lastShipPlacedList.Clear();

                    selectedShip = null;

                    ShipSetupListBox.Items.Clear();
                    foreach (Ship getShip in this.player2.CurrentShips)
                    {
                        ShipSetupListBox.Items.Add(getShip.GetName());
                    }

                    for (int x = 0; x < this.battlefieldSize; x++)
                    {
                        for (int y = 0; y < this.battlefieldSize; y++)
                        {
                            this.battleFieldGridArray[x, y].Fill = GetColor("#79dced");
                        }
                    }

                    lb_CurrentPlayerSetup.Content = "Current Turn: Player 2";

                    ////Runs the CPU Ship placement code
                    if (currentTurn.Type == "CPU")
                    {
                        Debug.WriteLine("FUGMA");
                        DoCPUShipSetup();
                    }

                }
            }
            else
            {
                this.ChangeGameState(GState.Battle);
            }
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

            Rectangle rect = (Rectangle)e.Source;
            rect.Fill = GetColor("#79dced");

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
                        Rectangle shipRect = this.battleFieldGridArray[x, y + j];
                        shipRect.Fill = GetColor("#79dced");
                    }
                }
            }

            if (this.selectedShip.Rotation == "Vertical")
            {
                for (int j = 1; j < this.selectedShip.Length; j++)
                {
                    if ((x + j) < this.battlefieldSize)
                    {
                        Rectangle shipRect = this.battleFieldGridArray[x + j, y];
                        shipRect.Fill = GetColor("#79dced");
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

            SolidColorBrush brush;
            if (this.selectedShip.IsPlaced)
            {
                brush = this.GetColor("#ff0800");
            }
            else
            {
                brush = this.GetColor("#edffef");
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
                        brush = GetColor("#ff0800");
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
                        brush = GetColor("#ff0800");

                        this.canPlaceShip = false;
                    }

                    Rectangle shipRect = this.battleFieldGridArray[x, newY];
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
                        brush = GetColor("#ff0800");
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
                        brush = this.GetColor("#ff0800");
                        this.canPlaceShip = false;
                    }

                    Rectangle shipRect = this.battleFieldGridArray[newX, y];
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

        #region Battle Screen Buttons[[-----------------------------------------------------------------------------------------------------------------]]

        private void UpdateShipHealthListBox(Player p)
        {
            BattleHPListBox.Items.Clear();
            foreach (Ship s in p.CurrentShips)
            {
                if (!s.IsSunk)
                {
                    string msg = "";
                    msg += s.GetName() + ": " + s.Health.ToString();
                    BattleHPListBox.Items.Add(msg);
                }
            }
        }

        /// <summary>
        /// Method to make BattleScreen Visible
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void CanBattleScreen_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.SetupGrid(this.grid_Battlefield);

            currentTurn = player1;
            currentEnemy = player2;

            UpdateShipHealthListBox(player2);
            UpdateBattlefieldColors();
        }

        /// <summary>
        /// Exit button for when game starts, exits back to player select.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void BattleExitBtn_Click(object sender, RoutedEventArgs e)
        {
            var playerConfirmation = MessageBox.Show("Really Quit? All progress will be lost", "Really Quit?!", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (playerConfirmation == MessageBoxResult.Yes)
            {
                player1.CurrentShips.Clear();
                player2.CurrentShips.Clear();
                ChangeGameState(GState.Start);
            }
        }

        /// <summary>
        /// Button that is used to fire.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void BattleFireBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("FIRED!");
            if (hasPlayerShot == true)
            {
                MessageBox.Show("You have already shot this turn!");
                return;
            }
            if (isCellSelected == false)
            {
                MessageBox.Show("You need to choose a cell to shoot!");
                return;
            }

            //// Look at the enemie's ship grid and see if there is a ship at the fired slot. 

            Ship getShip = currentEnemy.Board.ShipGrid[cellToShoot[0], cellToShoot[1]];

            if (getShip != null)
            {
                currentTurn.Board.DataGrid[cellToShoot[0], cellToShoot[1]] = GridData.Hit;
                getShip.Health -= 1;
                MessageBox.Show("HIT!");
                UpdateShipsIsSunk(getShip);
            }
            else
            {
                currentTurn.Board.DataGrid[cellToShoot[0], cellToShoot[1]] = GridData.Miss;
                MessageBox.Show("MISS!");
            }
            hasPlayerShot = true;
            Array.Clear(cellToShoot, 0, cellToShoot.Length);
            isCellSelected = false;
            UpdateBattlefieldColors();
            CheckWinCondition();
        }

        /// <summary>
        /// Ends turn after fire.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void BattleEndTurn_Click(object sender, RoutedEventArgs e)
        {
            if (hasPlayerShot == false)
            {
                MessageBox.Show("You need to fire a shot!");
                return;
            }
            BattlefieldBlackoutScreen.Visibility = Visibility.Visible;

            ////Used for the message box telling who's turn it's about to be.
            string player = "Player 1";
            if (currentEnemy == player2)
            {
                player = "Player 2";
            }

            var playerConfirmation = MessageBox.Show(player + ", Press Ok When Ready", "Player Switch Initiated",
                MessageBoxButton.OK, MessageBoxImage.Information);

            if (playerConfirmation == MessageBoxResult.OK)
            {
                if (currentTurn == player1)
                {
                    currentEnemy = player1;
                    currentTurn = player2;
                    UpdateShipHealthListBox(player1);
                }
                else
                {
                    currentEnemy = player2;
                    currentTurn = player1;
                    UpdateShipHealthListBox(player2);
                }

                BattlefieldBlackoutScreen.Visibility = Visibility.Collapsed;

                lb_CurrentPlayer.Content = "Current Turn: " + player;
                hasPlayerShot = false;
                Array.Clear(cellToShoot, 0, cellToShoot.Length);

                UpdateBattlefieldColors();

                if (currentTurn.Type == "CPU")
                {
                    DoCPUTurn();
                }

            }
        }

        private void BattleSwitchGrids_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckWinCondition()
        {
            foreach (Ship s in currentEnemy.CurrentShips)
            {
                if (s.IsSunk == false)
                {
                    return;
                }
            }

            var playerConfirmation = MessageBox.Show(currentTurn.Name + " Won!", "Someone Won!", MessageBoxButton.OK, MessageBoxImage.Information);

            if (playerConfirmation == MessageBoxResult.OK)
            {
                player1.CurrentShips.Clear();
                player2.CurrentShips.Clear();
                ChangeGameState(GState.Start);
            }
        }

        private void UpdateShipsIsSunk(Ship s)
        {
            if (s.Health == 0)
            {
                s.IsSunk = true;
                MessageBox.Show(s.GetName() + " has been sunk!");
            }
        }





        #endregion

        #region Battle Field Functionality [[-----------------------------------------------------------------------------------------------------------------]]

        /// <summary>
        /// Checks when the mouse is over a grid square in battle
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void BattlefieldGrid_MouseEnteredSquare(object sender, MouseEventArgs e)
        {
            if (hasPlayerShot == true)
            {
                return;
            }
            Rectangle rect = (Rectangle)e.Source;
            rect.Fill = GetColor("#e1a6ff");

            ////If the player mouses over a square that has already been checked change the color to a deep red
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

            if (currentTurn.Board.DataGrid[x, y] != GridData.Empty)
            {
                rect.Fill = GetColor("#4d0000");
            }

        }

        /// <summary>
        /// Checks when the mouse leaves a grid square in battle
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void BattlefieldGrid_MouseLeftSquare(object sender, MouseEventArgs e)
        {
            Rectangle rect = (Rectangle)e.Source;
            rect.Fill = GetColor("#79dced");

            UpdateBattlefieldColors();
        }

        /// <summary>
        /// Checks when the mouse clicks a grid square in battle
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event</param>
        private void BattlefieldGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (hasPlayerShot == true)
            {
                return;
            }

            Rectangle rect = (Rectangle)e.Source;
            int x = 0;
            int y = 0;

            //// gets the x and y of where the player clicked
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

            if (currentTurn.Board.DataGrid[x, y] != GridData.Empty)
            {
                return;
            }

            rect.Fill = GetColor("#cc66ff");

            this.cellToShoot[0] = x;
            this.cellToShoot[1] = y;
            isCellSelected = true;
            this.UpdateBattlefieldColors();
            Debug.WriteLine("SHOT PLACED AT: " + cellToShoot[0].ToString() + ", " + cellToShoot[1].ToString());
        }

        #endregion

        #region CPU Functionality [[-----------------------------------------------------------------------------------------------------------------]]
        private void DoCPUTurn()
        {
            //randomly choose a cell x and y within the specified grid size (battlefieldSize)
            //if the CPU has already fired at that spot, run the same code again, and choose another spot. 
            bool cellIsEmpty = false;
            while (!cellIsEmpty)
            {
                cellIsEmpty = GetRandomCell();
            }

            Ship getShip = currentEnemy.Board.ShipGrid[cellToShoot[0], cellToShoot[1]];

            if (getShip != null)
            {
                currentTurn.Board.DataGrid[cellToShoot[0], cellToShoot[1]] = GridData.Hit;
                getShip.Health -= 1;
                MessageBox.Show("HIT!");
                UpdateShipsIsSunk(getShip);
            }
            else
            {
                currentTurn.Board.DataGrid[cellToShoot[0], cellToShoot[1]] = GridData.Miss;
                MessageBox.Show("MISS!");
            }
            hasPlayerShot = true;
            Array.Clear(cellToShoot, 0, cellToShoot.Length);
            isCellSelected = false;
            UpdateBattlefieldColors();
            CheckWinCondition();
        }

        private bool GetRandomCell()
        {
            //get random x and y coord to look at and set them to cellToShoot[0] and [1]
            //loop through the currentTurn's dataGrid, If the cell IS NOT EMPTY, return false. If it is empty you return true.
            Random Rand = new Random();
            //choose a random cell x and y to check, loop through the dataGrid and make sure the cells are empty

            cellToShoot[0] = Rand.Next(0, battlefieldSize - 1);
            cellToShoot[1] = Rand.Next(0, battlefieldSize - 1);
            if (currentTurn.Board.DataGrid[cellToShoot[0], cellToShoot[1]] == GridData.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoCPUShipSetup()
        {
            foreach (Ship s in currentTurn.CurrentShips)
            {
                selectedShip = s;

                if (this.selectedShip == null)
                {
                    return;
                }

                if (!this.selectedShip.IsPlaced && this.canPlaceShip)
                {
                    int[] cellToCheck = new int[2];

                    bool cellIsEmpty = false;

                    List<int[]> cellsChecked = new List<int[]>();

                    //brute force and find an acceptable cell to place the ship in.
                    while (!cellIsEmpty)
                    {
                        bool IsNotInList = false;

                        Random Rand = new Random();
                        //choose a random cell x and y to check, loop through the cellsChecked list and make sure the cells havent already been checked.
                        while (!IsNotInList)
                        {
                            cellToCheck[0] = Rand.Next(0, battlefieldSize - 1);
                            cellToCheck[1] = Rand.Next(0, battlefieldSize - 1);
                            Debug.WriteLine(cellToCheck[0].ToString() + " - " + cellToCheck[1]);
                            if (cellsChecked.Count != 0)
                            {
                                foreach (int[] c in cellsChecked)
                                {
                                    if (cellToCheck[0] == c[0] && cellToCheck[1] == c[1])
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        IsNotInList = true;
                                        break;
                                    }
                                }
                            }
                            IsNotInList = true;
                        }


                        //randomly choose a ship rotation
                        int getRand = Rand.Next(0, 100);
                        if (getRand < 50)
                        {
                            this.selectedShip.Rotation = "Horizontal";
                        }
                        else if (getRand >= 50)
                        {
                            this.selectedShip.Rotation = "Vertical";
                        }

                        Debug.WriteLine(selectedShip.Length);

                        if (this.selectedShip.Rotation == "Horizontal")
                        {
                            for (int j = 0; j < this.selectedShip.Length; j++)
                            {
                                if ((cellToCheck[1] + j) >= this.battlefieldSize)
                                {
                                    //if the ship goes outside of the battlefield size, check the vertical rotation
                                    for (int k = 0; k < this.selectedShip.Length; k++)
                                    {
                                        if ((cellToCheck[0] + k) >= this.battlefieldSize)
                                        {
                                            //if the vertical rotaion is ALSO outside of the battlefield size, add the cell to the cellsChecked List.
                                            cellsChecked.Add(cellToCheck);
                                            break;
                                        }
                                        if (!this.CheckForOtherShips(cellToCheck[0], cellToCheck[1] + k))
                                        {
                                            //if a ship is in the cells we are checking add the cell we checked to the list
                                            cellsChecked.Add(cellToCheck);
                                            break;
                                        }
                                    }
                                    cellIsEmpty = true;
                                    this.selectedShip.Rotation = "Vertical";
                                    break;
                                }
                                if (!this.CheckForOtherShips(cellToCheck[0], cellToCheck[1] + j))
                                {
                                    //if the ship goes outside of the battlefield size, check the vertical rotation
                                    for (int k = 0; k < this.selectedShip.Length; k++)
                                    {
                                        if ((cellToCheck[0] + k) >= this.battlefieldSize)
                                        {
                                            //if the vertical rotaion is ALSO outside of the battlefield size, add the cell to the cellsChecked List.
                                            cellsChecked.Add(cellToCheck);
                                            break;
                                        }
                                        if (!this.CheckForOtherShips(cellToCheck[0], cellToCheck[1] + k))
                                        {
                                            //if a ship is in the cells we are checking add the cell we checked to the list
                                            cellsChecked.Add(cellToCheck);
                                            break;
                                        }
                                    }
                                    cellIsEmpty = true;
                                    this.selectedShip.Rotation = "Vertical";
                                    break;
                                }

                            }
                            cellIsEmpty = true;
                            break;
                        }
                        else if (this.selectedShip.Rotation == "Vertical")
                        {
                            for (int j = 0; j < this.selectedShip.Length; j++)
                            {
                                if ((cellToCheck[0] + j) >= this.battlefieldSize)
                                {
                                    //if the ship goes outside of the battlefield size, check the Horizontal rotation
                                    for (int k = 0; k < this.selectedShip.Length; k++)
                                    {
                                        if ((cellToCheck[1] + k) >= this.battlefieldSize)
                                        {
                                            //if the vertical rotaion is ALSO outside of the battlefield size, add the cell to the cellsChecked List.
                                            cellsChecked.Add(cellToCheck);
                                            break;
                                        }
                                        if (!this.CheckForOtherShips(cellToCheck[0] + k, cellToCheck[1]))
                                        {
                                            //if a ship is in the cells we are checking add the cell we checked to the list
                                            cellsChecked.Add(cellToCheck);
                                            break;
                                        }
                                    }
                                    cellIsEmpty = true;
                                    this.selectedShip.Rotation = "Horizontal";
                                    break;
                                }
                                if (!this.CheckForOtherShips(cellToCheck[0] + j, cellToCheck[1]))
                                {
                                    //if the ship goes outside of the battlefield size, check the Horizontal rotation
                                    for (int k = 0; k < this.selectedShip.Length; k++)
                                    {
                                        if ((cellToCheck[1] + k) >= this.battlefieldSize)
                                        {
                                            //if the vertical rotaion is ALSO outside of the battlefield size, add the cell to the cellsChecked List.
                                            cellsChecked.Add(cellToCheck);
                                            break;
                                        }
                                        if (!this.CheckForOtherShips(cellToCheck[0] + k, cellToCheck[1]))
                                        {
                                            //if a ship is in the cells we are checking add the cell we checked to the list
                                            cellsChecked.Add(cellToCheck);
                                            break;
                                        }
                                    }
                                    cellIsEmpty = true;
                                    this.selectedShip.Rotation = "Horizontal";
                                    break;
                                }

                            }
                            cellIsEmpty = true;
                            break;
                        }
                    }

                    this.selectedShip.Origin[0] = cellToCheck[0];
                    this.selectedShip.Origin[1] = cellToCheck[1];
                    this.selectedShip.IsPlaced = true;

                    Debug.WriteLine(selectedShip.Rotation);

                    if (this.selectedShip.Rotation == "Horizontal")
                    {
                        for (int j = 0; j < this.selectedShip.Length; j++)
                        {
                            this.currentTurn.Board.ShipGrid[cellToCheck[0], cellToCheck[1] + j] = this.selectedShip;
                        }
                    }
                    else if (this.selectedShip.Rotation == "Vertical")
                    {
                        for (int j = 0; j < this.selectedShip.Length; j++)
                        {
                            this.currentTurn.Board.ShipGrid[cellToCheck[0] + j, cellToCheck[1]] = this.selectedShip;
                        }
                    }
                }

                //// the colors on the battlefield from the grid data
                this.UpdateBattlefieldColors();
            }
        }

        #endregion
    }
}
