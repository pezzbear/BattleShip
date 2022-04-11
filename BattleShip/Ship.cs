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
        private string type;

        /// <summary>
        ///  Here we have our Length integer, which will keep track of how many grid spaces our ships will take up on our board.
        /// </summary>
        private int length;

        /// <summary>
        ///  Here we have the isSunk which will be used to see if a boat is sunk during the match, having it be eliminated.
        /// </summary>
        private bool isSunk;

        /// <summary>
        ///  Here we have our Origin of our ship, which will contain all the different grid positions the ship is taking up.
        /// </summary>
        private int[] origin;

        /// <summary>
        ///  The following will be used during the setup screen, when placing down your boats you will have the option to rotate said ship before placing them.
        /// </summary>
        private bool rotation;
    }
}
