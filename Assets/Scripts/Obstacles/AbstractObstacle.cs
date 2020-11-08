using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enum for turn values negative or positive.
/// </summary>
public enum Direction : int { Left = -1, Right = 1 }
/// <summary>
/// Abstact obstacle for mandatory methods all obstacles must implement
/// </summary>
public abstract class AbstractObstacle : MonoBehaviour
{
     protected abstract void OnCollisionEnter(Collision collision);
}