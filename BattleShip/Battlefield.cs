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
        public int Width;

        /// <summary>
        /// Size of grid.
        /// </summary>
        public int Height;

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
        /// <param name="width_">Width of the Battlefield</param>
        /// /// <param name="height_">Height of the Battlefield</param>
        public Battlefield(int width_, int height_)
        {
            this.Width = width_;
            this.Height = height_;
            this.DataGrid = new GridData[this.Width, this.Height];
            this.ShipGrid = new Ship[this.Width, this.Height];
        }
    }
}
