using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : AbstractPlatform
{
     #region Fields
     [SerializeField]
     private float forwardForce = 5f, upForce = 10f;
     #endregion
     #region Engine Methods
     void OnCollisionEnter(Collision collision)
     {
          if (collision.gameObject.TryGetComponent(out Player character))
          {
               character.Jump((character.transform.forward * forwardForce) + (Vector3.up * upForce));
          }
     } 
     #endregion
}
