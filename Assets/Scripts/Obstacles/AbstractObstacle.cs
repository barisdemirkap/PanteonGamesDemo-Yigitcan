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

     /// <summary>
     /// Base class method for rotating object in fixed delta
     /// </summary>
     /// <param name="angleVelocity">Rotation amount per second</param>
     /// <param name="direction">Rotation direction</param>
     /// <param name="rigidbody">Game objects rigidbody component reference</param>
     protected void RotateObject(Vector3 angleVelocity, Direction direction, Rigidbody rigidbody)
     {
          //casting direction enum value for calculation
          Quaternion deltaRotation = Quaternion.Euler(angleVelocity * (int)direction * Time.fixedDeltaTime);
          rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
     }
}