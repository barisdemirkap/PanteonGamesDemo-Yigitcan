using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Base character class
/// </summary>
public abstract class AbstractCharacter : MonoBehaviour
{
     /// <summary>
     /// Receives force from given direction
     /// </summary>
     /// <param name="direction">Force direction</param>
     /// <param name="forceStrenght">Force strenght</param>
     public abstract void ReceiveForce(Vector3 direction, float forceStrenght);
     /// <summary>
     /// Calculate distance between start position and transform
     /// </summary>
     /// <param name="startPosition">Start position</param>
     /// <param name="transform">Character transform</param>
     /// <returns></returns>
     public virtual float GetDistanceTravelled(Vector3 startPosition, Transform transform)
     {
          return Vector3.Distance(startPosition, transform.position);
     }
     /// <summary>
     /// Updates players position text
     /// </summary>
     /// <param name="positionText">Text object</param>
     public abstract void UpdatePositionText(TextMeshPro positionText);

}

