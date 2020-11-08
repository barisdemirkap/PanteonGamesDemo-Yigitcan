using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : Obstacle, IDynamicObstacle
{

     #region Fields
     private Rigidbody rb;
     [SerializeField]
     private Vector3 angularVelocity = new Vector3(0, 40, 0);
     [SerializeField]
     private Direction direction = Direction.Right;
     #endregion
     #region Engine Methods
     protected override void Start()
     {
          base.Start();
          rb = GetComponent<Rigidbody>();
     }

     void FixedUpdate()
     {
          Movement();
     }
     #endregion

     #region Script Methods

     public void Movement()
     {
          HelperMethods.RotateObject(angularVelocity, direction, rb);
     }
     #endregion


}
