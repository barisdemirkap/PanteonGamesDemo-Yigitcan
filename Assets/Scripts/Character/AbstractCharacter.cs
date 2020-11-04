using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
     public abstract void ReceiveForce(Vector3 direction, float forceStrenght);

     public virtual float GetDistanceTravelled(Vector3 startPosition,Transform transform)
     {
          return Vector3.Distance(startPosition, transform.position);
     }
}
