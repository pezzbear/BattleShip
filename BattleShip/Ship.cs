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
        public shipType type;

        /// <summary>
        ///  Here we have our Length integer, which will keep track of how many grid spaces our ships will take up on our board.
        /// </summary>
        public int length;

        /// <summary>
        ///  Here we have the isSunk which will be used to see if a boat is sunk during the match, having it be eliminated.
        /// </summary>
        public bool isSunk = false;

        /// <summary>
        ///  Here we have our Origin of our ship, which will contain all the different grid positions the ship is taking up.
        /// </summary>
        public int[] origin = new int[2];

        /// <summary>
        ///  Color used to show what ship it is
        /// </summary>
        public SolidColorBrush ShipColor = new SolidColorBrush();

        /// <summary>
        ///  The following will be used during the setup screen, when placing down your boats you will have the option to rotate said ship before placing them.
        /// </summary>
        public string rotation = "Horizontal";

        public bool isPlaced = false;

        public enum shipType 
        {
            carrier,
            battleship,
            curiser,
            submarine,
            destroyer
        }

        public void SetLenght() 
        {
            switch(type) 
            {
                case shipType.carrier:
                    length = 5;
                    ShipColor.Color = Color.FromRgb(237, 99, 255);
                    break;
                case shipType.battleship:
                    length = 4;
                    ShipColor.Color = Color.FromRgb(245, 255, 102);
                    break;
                case shipType.curiser:
                    length = 3;
                    ShipColor.Color = Color.FromRgb(255, 204, 64);
                    break;
                case shipType.submarine:
                    length = 3;
                    ShipColor.Color = Color.FromRgb(66, 255, 107);
                    break;
                case shipType.destroyer:
                    length = 2;
                    ShipColor.Color = Color.FromRgb(3, 1, 112);
                    break;
            }
        }

        public string GetName() 
        {
            switch (type) 
            {
                case shipType.carrier:
                    return "Carrier";
                case shipType.battleship:
                    return "Battleship";
                case shipType.curiser:
                    return "Curiser";
                case shipType.submarine:
                    return "Submarine";
                case shipType.destroyer:
                    return "Destroyer";
                default:
                    return "ERROR";
            }
        }
    }
}
