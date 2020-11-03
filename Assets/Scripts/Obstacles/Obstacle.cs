using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Concerete obstacle class
/// </summary>
[RequireComponent(typeof(NavMeshObstacle))]
public class Obstacle : AbstractObstacle
{
     [SerializeField]
     private GameObject fxPrefab;
     
     protected override void OnCollisionEnter(Collision collision)
     {
          //Vector3 contactPoint = collision.contacts[0].point;
          //contactPoint.y = 1.5f;
          foreach (ContactPoint item in collision.contacts)
          {
               Instantiate(fxPrefab, item.point, Quaternion.identity);
          }


          AbstractCharacter abstractCharacter;
          if (collision.collider.TryGetComponent(out abstractCharacter))
          {
               abstractCharacter.ReceiveForce(-collision.contacts[0].point,5f);
          }
         
     }
}

