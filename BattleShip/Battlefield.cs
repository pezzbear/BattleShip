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
        public Battlefield(int _size) 
        {
            Size = _size;
            DataGrid = new GridData[Size, Size];
            ShipGrid = new Ship[Size, Size];
        }

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
