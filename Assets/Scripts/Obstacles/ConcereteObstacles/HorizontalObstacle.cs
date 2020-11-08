using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorizontalObstacle : Obstacle, IDynamicObstacle
{
     #region Fields
     [SerializeField]
     float movementRange = 5f;
     [SerializeField]
     float speed = 1f;
     [SerializeField]
     float delay = 2f;

     float elapsedTime = 0f;

     Direction direction = Direction.Right;
     Rigidbody rb;

     bool stationary = false;
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
          if (stationary)
          {
               elapsedTime += Time.fixedDeltaTime;
               if (elapsedTime >= delay)
               {
                    stationary = false;
               }
               else
                    return;
          }
          float targetX = movementRange * (int)direction;
          Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
          rb.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, targetPosition.x, speed * Time.fixedDeltaTime),targetPosition.y,targetPosition.z));
          if (Vector3.Distance(transform.position,targetPosition ) < .5f)
          {
               stationary = true;
               elapsedTime = 0;
               switch (direction)
               {
                    case Direction.Left:
                         direction = Direction.Right;
                         break;
                    case Direction.Right:
                         direction = Direction.Left;
                         break;
               }
          }
     } 
     #endregion
}
