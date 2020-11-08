using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : Obstacle, IDynamicObstacle
{
     #region Fields

     Rigidbody rb;

     [SerializeField]
     Vector3 angularVelocity = new Vector3(0, 20, 0);

     [SerializeField]
     [Tooltip("Left for right, Right for left.")]
     Direction direction = Direction.Left;

     #endregion

     #region Engine Methods
     protected override void Start()
     {
          base.Start();
          rb = GetComponent<Rigidbody>();
     }
     private void FixedUpdate()
     {
          Movement();
     }
     #endregion

     #region Script Methods
     public void Movement()
     {
          //We are using base class method for rotating object in fixed deltatime
          HelperMethods.RotateObject(angularVelocity, direction, rb);
     } 
     #endregion
}
