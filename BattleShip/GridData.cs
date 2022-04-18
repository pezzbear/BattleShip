//-----------------------------------------------------------------------
// <copyright file="GridData.cs" company="Our Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace BattleShip
{
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