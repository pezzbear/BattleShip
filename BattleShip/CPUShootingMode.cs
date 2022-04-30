//-----------------------------------------------------------------------
// <copyright file="CPUShootingMode.cs" company="Our Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
public enum CPUShootingMode
{
    //does a random shot as the first.
    FirstShot,

    //randomly shooting in a checkerboard until we find a hit.
    LookingForShip,

    //after getting a hit, randomly shoot around the hit to find the direction.
    RandomAttack,

    //after determining the direction to be horizontal, check the left and right of the hit until sinking the ship.
    HorizontalAttack,

    //after determining the direction to be vertical, check the up and down of the hit until sinking the ship.
    VerticalAttack

} 