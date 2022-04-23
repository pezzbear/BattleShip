//-----------------------------------------------------------------------
// <copyright file="Player.cs" company="Our Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace BattleShip
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Here we is our Player class, which will keep track of all data related to said Player.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public class Player
    {
        /// <summary>
        /// Here we is our type for the player class, this will show if we are dealing with a human player or AI.
        /// </summary>
        public string Type;

        /// <summary>
        /// Here we is our currentShips which uses the Ship class. The array contains all the ships the player has to their disposal.
        /// </summary>
        public List<Ship> CurrentShips = new List<Ship>();

        /// <summary>
        /// Here we have our board which uses the Battlefield class, this will contain all the information of the board the player has control of.
        /// </summary>
        public Battlefield Board;

        /// <summary>
        /// Here we will store the player's name once they enter it in the setup screen.
        /// </summary>
        public string Name;

    }
}
