//-----------------------------------------------------------------------
// <copyright file="CPUShootingMode.cs" company="Our Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

/// <summary>
/// CPUs shooting mode.
/// </summary>
public enum CPUShootingMode
{
    /// <summary>
    /// Represents doing a random shot as the first move.
    /// </summary>
    FirstShot,

    /// <summary>
    /// Represents shooting in a checkerboard until we find a hit.
    /// </summary>
    LookingForShip,

    /// <summary>
    /// After getting a hit, randomly shoot around the hit to find the direction.
    /// </summary>
    RandomAttack,

    /// <summary>
    /// After determining the direction to be horizontal, check the left and right of the hit until sinking the ship.
    /// </summary>
    HorizontalAttack,

    /// <summary>
    /// After determining the direction to be vertical, check the up and down of the hit until sinking the ship.
    /// </summary>
    VerticalAttack
} 