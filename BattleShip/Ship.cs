//-----------------------------------------------------------------------
// <copyright file="Ship.cs" company="Our Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace BattleShip
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Media;

    /// <summary>
    /// Here we will have our Ship class, which will contain the important information that
    /// each ship will need, such as type, length, its location on the board, and if it has been sunk or not .
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public class Ship
    {
        /// <summary>
        /// The following String is known as the Type string. This will contain the name of what type of ship we have on our hands.
        /// </summary>
        public ShipType Type;

        /// <summary>
        ///  Here we have our Length integer, which will keep track of how many grid spaces our ships will take up on our board.
        /// </summary>
        public int Length;

        /// <summary>
        /// Health of the ships. How many hits it has left until its sunk
        /// </summary>
        public int Health;

        /// <summary>
        ///  Here we have the isSunk which will be used to see if a boat is sunk during the match, having it be eliminated.
        /// </summary>
        public bool IsSunk = false;

        /// <summary>
        ///  Here we have our Origin of our ship, which will contain all the different grid positions the ship is taking up.
        /// </summary>
        public int[] Origin = new int[2];

        /// <summary>
        ///  The following will be used during the setup screen, when placing down your boats you will have the option to rotate said ship before placing them.
        /// </summary>
        public string Rotation = "Horizontal";

        /// <summary>
        /// The following will be used during the setup screen, when placing a boat this will detect if the boat was actually placed down on the board.
        /// </summary>
        public bool IsPlaced = false;

        /// <summary>
        ///  Color used to show what ship it is
        /// </summary>
        public SolidColorBrush ShipColor = new SolidColorBrush();

        /// <summary>
        /// Initializes a new instance of the Ship class.
        /// </summary>
        /// <param name="type">Type of ship</param>
        public Ship(ShipType type)
        {
            this.Type = type;
            this.SetLength();
        }

        /// <summary>
        /// Sets the Length of the Ship returns the highlight and amount of tiles it brings up 
        /// </summary>
        public void SetLength()
        {
            switch (this.Type)
            {
                case ShipType.warship:
                    this.Length = 6;
                    this.ShipColor.Color = Color.FromRgb(45, 13, 79);
                    break;
                case ShipType.carrier:
                    this.Length = 5;
                    this.ShipColor.Color = Color.FromRgb(237, 99, 255);
                    break;
                case ShipType.battleship:
                    this.Length = 4;
                    this.ShipColor.Color = Color.FromRgb(245, 255, 102);
                    break;
                case ShipType.cruiser:
                    this.Length = 3;
                    this.ShipColor.Color = Color.FromRgb(255, 204, 64);
                    break;
                case ShipType.submarine:
                    this.Length = 3;
                    this.ShipColor.Color = Color.FromRgb(66, 255, 107);
                    break;
                case ShipType.destroyer:
                    this.Length = 2;
                    this.ShipColor.Color = Color.FromRgb(3, 1, 112);
                    break;
                case ShipType.scout:
                    this.Length = 2;
                    this.ShipColor.Color = Color.FromRgb(168, 50, 82);
                    break;
            } 

            this.Health = this.Length;
        }

        /// <summary>
        /// Grabs the Ship Name and returns it
        /// </summary>
        /// <returns>Ship Type.</returns>
        public string GetName() 
        {
            switch (this.Type) 
            {
                case ShipType.scout:
                    return "Scout";
                case ShipType.warship:
                    return "Warship";
                case ShipType.carrier:
                    return "Carrier";
                case ShipType.battleship:
                    return "Battleship";
                case ShipType.cruiser:
                    return "Curiser";
                case ShipType.submarine:
                    return "Submarine";
                case ShipType.destroyer:
                    return "Destroyer";
                default:
                    return "ERROR";
            }
        }
    }
}
