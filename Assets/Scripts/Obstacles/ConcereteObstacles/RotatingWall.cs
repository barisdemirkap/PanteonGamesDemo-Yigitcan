using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : Obstacle, IDynamicObstacle
{
     private Rigidbody rb;
     [SerializeField]
     private Vector3 angularVelocity = new Vector3(0, 40, 0);
     [SerializeField]
     private Direction direction = Direction.Right;
     protected override void Start()
     {
          base.Start();
          rb = GetComponent<Rigidbody>();
     }

     void FixedUpdate()
     {
          Movement();
     }


     public void Movement()
     {
          RotateObject(angularVelocity, direction, rb);
     }


}
