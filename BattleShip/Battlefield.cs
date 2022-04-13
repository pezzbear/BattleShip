//-----------------------------------------------------------------------
// <copyright file="Battlefield.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace BattleShip 
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class used to track grid data for the battlefield board.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public class Battlefield 
    {
        /// <summary>
        /// Size of the battlefield grid
        /// </summary>
        public int Size;

        /// <summary>
        /// A 2d array of enum values that will be used to track what's happening during gameplay.
        /// </summary>
        public GridData[,] DataGrid;

        /// <summary>
        /// Size of the battlefield grid
        /// </summary>
        public Ship[,] ShipGrid; 

        /// <summary>
        /// Initializes a new instance of the Battlefield class.
        /// </summary>
        /// <param name="size_">Size of the Battlefield</param>
        public Battlefield(int size_)
        {
            this.Size = size_;
            this.DataGrid = new GridData[this.Size, this.Size];
            this.ShipGrid = new Ship[this.Size, this.Size];
        }

        /// <summary>
        /// Data entered in the battlefield grids.
        /// </summary>
        public enum GridData
        {
            /// <summary>
            /// Empty space on the board
            /// </summary>
            Empty,

            /// <summary>
            /// Represents when a player misses a ship after firing
            /// </summary>
            Miss,

            /// <summary>
            /// Represents when a player hits a ship after firing
            /// </summary>
            Hit
        }
    }
}
