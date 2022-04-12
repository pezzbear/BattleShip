//-----------------------------------------------------------------------
// <copyright file="MainWindow.xmal.cs" company="Our Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace BattleShip {
    using System;
    using System.Collections.Generic;
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
    using System.Diagnostics;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public partial class MainWindow : Window 
    {
        public MainWindow() 
        {
            InitializeComponent();
        }

        /// <summary>
        /// Current "Gamestate", basically what window we are on.
        /// </summary>
        private gState GameState = gState.Start;

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
        /// Array used to keep track of highlighing the grid when placing a ship
        /// </summary>
        private Rectangle[,] setupGridArray;

        private Player player1 = new Player();

        private Player player2 = new Player();

        private Ship[] startingShips = new Ship[5];

        private Player CurrentTurn;

        private Ship SelectedShip;

        private bool canPlaceShip = true;

        private List<Ship> lastShipPlacedList = new List<Ship>();

        private Color GetColor(string colorHex) 
        {
            Color color = (Color)ColorConverter.ConvertFromString(colorHex);
            return color;
         }
        private void ChangeGameState(gState state) 
        {

            GameState = state;

            switch (GameState) 
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
            /// <summary>
            /// This is temporary and will be changed when we implement the settings screen
            /// </summary>

            for (int i = 0; i < startingShips.Length; i++) 
            {
                Ship newShip = new Ship();
                newShip.type = (Ship.shipType)i;
                newShip.SetLenght();
                startingShips[i] = newShip;
            }

        }

        private void SetupGrid(Grid grid) 
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            grid.ShowGridLines = true;
            double cellSize = 55;
            GridLength gSize = new GridLength(cellSize);

            for(int i = 0; i < battlefieldSize; i++) 
            { 
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = gSize;
                grid.ColumnDefinitions.Add(colDef);

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = gSize;
                grid.RowDefinitions.Add(rowDef);
            }

            grid.Height = cellSize * battlefieldSize;
            grid.Width = cellSize * battlefieldSize;

            if(grid == grid_ShipSetup) 
            { 
                for (int x = 0; x < battlefieldSize; x++) 
                {
                    for (int y = 0; y < battlefieldSize; y++) 
                    {
                        Rectangle Rect = new Rectangle();
                        Rect.Name = AlphArray[x] + y.ToString();
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        mySolidColorBrush.Color = GetColor("#79dced");
                        Rect.Fill = mySolidColorBrush;
                        Grid.SetRow(Rect, x);
                        Grid.SetColumn(Rect, y);
                        grid.Children.Add(Rect);
                        Rect.MouseDown += SetUpGrid_MouseDown;
                        Rect.MouseEnter += SetUpGrid_MouseEnteredSquare;
                        Rect.MouseLeave += SetUpGrid_MouseLeftSquare;
                        setupGridArray[x, y] = Rect;
                    }
                }
            } 

        }

        private void UpdateBattlefieldColors()
        {
            for(int x = 0; x < battlefieldSize; x++)
            {
                for(int y = 0; y < battlefieldSize; y++)
                {
                    Ship getShip = CurrentTurn.Board.ShipGrid[x, y];
                    if(getShip != null)
                    {
                        setupGridArray[x, y].Fill = getShip.ShipColor;
                    }
                }
            }
        }

        private void ResetPlayerShips()
        {
            foreach (Ship p1Ship in player1.CurrentShips)
            {
                p1Ship.isPlaced = false;
                p1Ship.isSunk = false;
            }

            foreach (Ship p2Ship in player1.CurrentShips)
            {
                p2Ship.isPlaced = false;
                p2Ship.isSunk = false;
            }
        }

        private void MainCanvas_OnLoad(object sender, RoutedEventArgs e) 
        {
            ChangeGameState(gState.Start);
            LoadShips();
            setupGridArray = new Rectangle[battlefieldSize, battlefieldSize];
        }

        #region Start Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]
        private void StartButton_Click(object sender, RoutedEventArgs e) 
        {
            ChangeGameState(gState.PlayerSelect);
        }

        private void CreditsButton_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show(messageBoxText: "Names: Trik Heath, Edan Deno, Robert Jaklin, Alberto Ortiz Aguilar." + "\n" + "Version: 0.1" + "\n" + "Class: #22547 It:Program:Part 2 (C#)", "Credits");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) 
        {
            Close();
        }

        private void Rules_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show("Rules description", "Battleship Rules");
        }

        #endregion

        #region Player Select Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]
        private void BackButton_Click(object sender, RoutedEventArgs e) 
        {
            ChangeGameState(gState.Start);
        }

        private void StartPvp_Click(object sender, RoutedEventArgs e) 
        {
            if (txtPlayerOneName.Text == "") 
            {
                MessageBox.Show("Please enter a name for Player 1");
                return;
            }
            player1.Name = txtPlayerOneName.Text;
            player1.Type = "Player";
            player1.CurrentShips = startingShips.ToList();
            player1.Board = new Battlefield(battlefieldSize);


            if (txtPlayerTwoName.Text == "") 
            {
                MessageBox.Show("Please enter a name for Player 2");
                return;
            }
            player2.Name = txtPlayerTwoName.Text;
            player2.Type = "Player";
            player2.CurrentShips = startingShips.ToList();
            player2.Board = new Battlefield(battlefieldSize);
            
            CurrentTurn = player1;
            ChangeGameState(gState.ShipPlacement);
        }

        private void StartPvpVsCPU_Click(object sender, RoutedEventArgs e) 
        {
            if (txtPlayerOneName_Vs_AI.Text == "") 
            {
                MessageBox.Show("Please enter a name for Player 1");
                return;
            }
            player1.Name = txtPlayerOneName_Vs_AI.Text;
            player1.Type = "Player";
            player1.CurrentShips = startingShips.ToList();
            player1.Board = new Battlefield(battlefieldSize);


            player2.Name = "BATTLEFIELD_BOT_V2.0";
            player2.Type = "CPU";
            player2.CurrentShips = startingShips.ToList();
            player2.Board = new Battlefield(battlefieldSize);

            CurrentTurn = player1;
            ChangeGameState(gState.ShipPlacement);
        }

        private void Settings_Click(object sender, RoutedEventArgs e) 
        {

        }


        #endregion

        #region Ship Placement Screen Buttons [[-----------------------------------------------------------------------------------------------------------------]]

        private void canShipSetup_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) 
        {
            ShipSetupListBox.Items.Clear();
            foreach (Ship getShip in player1.CurrentShips) 
            {
                ShipSetupListBox.Items.Add(getShip.GetName());
                
            }
            SetupGrid(grid_ShipSetup);
            SelectedShip = null;
            Array.Clear(CurrentTurn.Board.ShipGrid, 0, setupGridArray.Length);
            ResetPlayerShips();
        }

        private void ShipSetupListBox_Selected(object sender, SelectionChangedEventArgs e) 
        {
            foreach (Ship getShip in player1.CurrentShips) 
            {
                if (ShipSetupListBox.SelectedItem != null) 
                { 
                    if (ShipSetupListBox.SelectedItem.ToString() == getShip.GetName()) 
                    {
                        SelectedShip = getShip;
                    }
                }
            }
        }

        private void SetUpBckBtn_Click(object sender, RoutedEventArgs e) 
        {
            ChangeGameState(gState.PlayerSelect);
        }

        private void btn_RotateShip_Click(object sender, RoutedEventArgs e) 
        {
            if (SelectedShip != null && !SelectedShip.isPlaced) 
            {
                if(SelectedShip.rotation == "Horizontal") 
                {
                    SelectedShip.rotation = "Vertical";
                } else 
                {
                    SelectedShip.rotation = "Horizontal";
                }
            }
        }

        private void SetUpUndoBtn_Click(object sender, RoutedEventArgs e) 
        {
            if(lastShipPlacedList.Count > 0)
            {
                Ship lastShip = lastShipPlacedList.Last();
                lastShip.isPlaced = false;
                int x = lastShip.origin[0];
                int y = lastShip.origin[1];
                SolidColorBrush Brush = new SolidColorBrush();
                Brush.Color = GetColor("#79dced");
                if (lastShip.rotation == "Horizontal")
                {
                    for (int j = 0; j < lastShip.length; j++)
                    {
                        CurrentTurn.Board.ShipGrid[x, y + j] = null;
                        setupGridArray[x, y + j].Fill = Brush;
                    }
                }
                else if (lastShip.rotation == "Vertical")
                {
                    for (int j = 0; j < lastShip.length; j++)
                    {
                        CurrentTurn.Board.ShipGrid[x + j, y] = null;
                        setupGridArray[x + j, y].Fill = Brush;
                    }
                }
                Array.Clear(lastShip.origin, 0, lastShip.origin.Length);
                UpdateBattlefieldColors();
                lastShipPlacedList.RemoveAt(lastShipPlacedList.Count-1);

                
            }
        }

        private void SetUpResetBtn_Click(object sender, RoutedEventArgs e) 
        {
            Array.Clear(CurrentTurn.Board.ShipGrid, 0, CurrentTurn.Board.ShipGrid.Length);
            foreach (Ship getShip in CurrentTurn.CurrentShips)
            {
                getShip.isPlaced = false;
                getShip.isSunk = false;
            }
            lastShipPlacedList.Clear();

            SolidColorBrush Brush = new SolidColorBrush();
            Brush.Color = GetColor("#79dced");
            for (int x = 0; x < battlefieldSize; x++)
            {
                for (int y = 0; y < battlefieldSize; y++)
                {
                    setupGridArray[x, y].Fill = Brush;
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
            if (SelectedShip == null) 
            {
                Debug.WriteLine("NO SHIP SELECTED");
                return;
            }
            SolidColorBrush Brush = new SolidColorBrush();
            Brush.Color = GetColor("#79dced");
            Rectangle rect = (Rectangle)e.Source;
            rect.Fill = Brush;

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



            if (SelectedShip.rotation == "Horizontal") 
            {
                for (int j = 1; j < SelectedShip.length; j++) 
                {
                    if ((y + j) < battlefieldSize) 
                    {
                        Rectangle shipRect = setupGridArray[x, y + j];
                        shipRect.Fill = Brush;
                    }
                }
            }

            if (SelectedShip.rotation == "Vertical") 
            {
                for (int j = 1; j < SelectedShip.length; j++) 
                {
                    if ((x + j) < battlefieldSize) {
                        Rectangle shipRect = setupGridArray[x + j, y];
                        shipRect.Fill = Brush;
                    }
                }
            }

            UpdateBattlefieldColors();
        }

        private void SetUpGrid_MouseEnteredSquare(object sender, MouseEventArgs e) 
        {
            if (SelectedShip == null) 
            {
                Debug.WriteLine("NO SHIP SELECTED");
                return;
            }

            UpdateBattlefieldColors();

            SolidColorBrush Brush = new SolidColorBrush();
            if(SelectedShip.isPlaced) 
            {
                Brush.Color = GetColor("#ff0800");
            } else {
                Brush.Color = GetColor("#edffef");
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



            if (SelectedShip.rotation == "Horizontal") 
            {
                for (int j = 0; j < SelectedShip.length; j++) 
                {
                    int newY = 0;
                    if ((y + j) >= battlefieldSize) 
                    {
                        Brush.Color = GetColor("#ff0800");
                        newY = battlefieldSize - 1;
                        canPlaceShip = false;
                    }
                    else {
                        newY = y + j;
                        canPlaceShip = true;
                    }
                    if(!CheckForOtherShips(x, newY))
                    {
                        Brush.Color = GetColor("#ff0800");
                        canPlaceShip = false;
                    }
                    Rectangle shipRect = setupGridArray[x, newY];
                    shipRect.Fill = Brush;
                }
            }
            else if (SelectedShip.rotation == "Vertical") 
            {
                for (int j = 0; j < SelectedShip.length; j++) 
                {
                    int newX = 0;
                    if ((x + j) >= battlefieldSize) 
                    {
                        Brush.Color = GetColor("#ff0800");
                        newX = battlefieldSize - 1;
                        canPlaceShip = false;
                    }
                    else {
                        newX = x + j;
                        canPlaceShip = true;
                    }
                    if (!CheckForOtherShips(newX, y))
                    {
                        Brush.Color = GetColor("#ff0800");
                        canPlaceShip = false;
                    }

                    Rectangle shipRect = setupGridArray[newX, y];
                    shipRect.Fill = Brush;
                }
            }

            rect.Fill = Brush;

            
        }

        private void SetUpGrid_MouseDown(object sender, MouseButtonEventArgs e) 
        {
            if (SelectedShip == null) 
            {
                return;
            }

            if(!SelectedShip.isPlaced && canPlaceShip) 
            {
                //setting the origin of the ship being placed
                Rectangle rect = (Rectangle)e.Source;

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

                SelectedShip.origin[0] = x;
                SelectedShip.origin[1] = y;
                SelectedShip.isPlaced = true;

                if (SelectedShip.rotation == "Horizontal")
                {
                    for (int j = 0; j < SelectedShip.length; j++)
                    {
                        CurrentTurn.Board.ShipGrid[x, y + j] = SelectedShip;
                    }
                }
                else if (SelectedShip.rotation == "Vertical")
                {
                    for (int j = 0; j < SelectedShip.length; j++)
                    {
                        CurrentTurn.Board.ShipGrid[x + j, y] = SelectedShip;
                    }
                }

            }

            lastShipPlacedList.Add(SelectedShip);

            //updates the colors on the battlefield from the grid data
            UpdateBattlefieldColors();
        }

        #endregion
 
    }
}
