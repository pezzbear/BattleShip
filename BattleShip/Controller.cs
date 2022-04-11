//-----------------------------------------------------------------------
// <copyright file="Controller.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace BattleShip 
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class used to track keep track of everything and keep control. Most of the game logic is in here.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public class Controller 
    {
        /// <summary>
        /// Keeps track of the current turn. 
        /// </summary>
        public Player CurrentTurn;
        
        /// <summary>
        /// Keeps track of player one.
        /// </summary>
        public Player PlayerOne;
        
        /// <summary>
        /// Keeps track of player two. 
        /// </summary>
        public Player PlayerTwo;
    }
}
