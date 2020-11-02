using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshModifier))]
public class RotatingPlatform : Platform, IDynamicPlatform
{
     #region Variables
     ///rotation per second
     [SerializeField]
     Vector3 angleVelocity = new Vector3(0, 0, 40);
     //rotate direction
     [SerializeField]
     [Tooltip("Left for right, Right for left.")]
     Direction direction = Direction.Right;

     Rigidbody rb;
     #endregion
     private void Start()
     {
          rb = GetComponent<Rigidbody>();
     }

     private void FixedUpdate()
     {
          PlatformMovement();
     }
     public void PlatformMovement()
     {
          //We are using base class method for rotating object in fixed deltatime
          RotateObject(angleVelocity,direction,rb);
     }
}
