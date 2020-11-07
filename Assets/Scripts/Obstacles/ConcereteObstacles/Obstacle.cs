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

          foreach (ContactPoint item in collision.contacts)
          {
               Instantiate(fxPrefab, item.point, Quaternion.identity);
          }


          AbstractCharacter abstractCharacter;
          if (collision.collider.TryGetComponent(out abstractCharacter))
          {
               Vector3 direction = collision.contacts[0].point;
               direction.y = 0;
               abstractCharacter.ReceiveForce(-direction, 5f);
          }

     }
}

