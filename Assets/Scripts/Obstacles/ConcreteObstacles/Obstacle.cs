using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Concrete obstacle class
/// </summary>
[RequireComponent(typeof(NavMeshObstacle))]
public class Obstacle : AbstractObstacle
{
     #region Fields
     [SerializeField]
     protected GameObject fxPrefab;
     [SerializeField]
     protected float forceAmount = 5f;
     #endregion

     #region Engine Methods
     protected virtual void Start()
     {
          // get fxPrefab from scriptable object if its not defined from inspector
          if (fxPrefab == null)
          {
               ObsactleVFX vfx = Resources.Load("Obstacle VFX") as ObsactleVFX;
               fxPrefab = vfx.vfxPrefab;
          }
     }
     protected override void OnCollisionEnter(Collision collision)
     {
          foreach (ContactPoint item in collision.contacts)
          {
               Instantiate(fxPrefab, item.point, Quaternion.identity);
          }
          if (collision.collider.TryGetComponent(out AbstractCharacter abstractCharacter))
          {
               Vector3 direction = collision.contacts[0].point;
               direction.y = 0;
               abstractCharacter.ReceiveForce(-direction, forceAmount);
          }
     }
     #endregion
}

