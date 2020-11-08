using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshModifier))]
public class RotatingPlatform : AbstractPlatform, IDynamicPlatform
{
     #region Fields
     ///rotation per second
     [SerializeField]
     Vector3 angleVelocity = new Vector3(0, 0, 40);
     //rotate direction
     [SerializeField]
     [Tooltip("Left for right, Right for left.")]
     Direction direction = Direction.Right;

     Rigidbody rb;
     #endregion

     #region Engine Methods
     private void Start()
     {
          rb = GetComponent<Rigidbody>();
     }

     private void FixedUpdate()
     {
          PlatformMovement();
     }
     #endregion

     #region Script Methods
     public void PlatformMovement()
     {
          HelperMethods.RotateObject(angleVelocity, direction, rb);
     } 
     #endregion
}
