﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : Obstacle, IDynamicObstacle
{
     #region Variables

     Rigidbody rb;

     [SerializeField]
     Vector3 angularVelocity = new Vector3(0, 20, 0);

     [SerializeField]
     [Tooltip("Left for right, Right for left.")]
     Direction direction = Direction.Left;

     #endregion
     private void Start()
     {
          rb = GetComponent<Rigidbody>();
     }
     private void FixedUpdate()
     {
          Movement();
     }
     public void Movement()
     {
          //We are using base class method for rotating object in fixed deltatime
          RotateObject(angularVelocity, direction, rb);
     }
}