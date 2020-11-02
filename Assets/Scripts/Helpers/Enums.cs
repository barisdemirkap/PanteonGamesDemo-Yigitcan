using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for controling game flow.
/// </summary>
public enum GameState
{
     Initialized,
     Playing,
     Paused,
     Painting,
     GameOver
}

/// <summary>
/// Enum for turn values negative or positive.
/// </summary>
public enum Direction : int
{

     Left = -1,
     Right = 1
}
