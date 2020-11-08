using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : Platform
{
     [SerializeField]
     private float forwardForce = 5f, upForce = 10f;

     protected override void OnCollisionEnter(Collision collision)
     {
          
          if (collision.gameObject.TryGetComponent(out Player character))
          {
                    character.Jump((character.transform.forward * forwardForce) + (Vector3.up * upForce));
          }
     }
}
