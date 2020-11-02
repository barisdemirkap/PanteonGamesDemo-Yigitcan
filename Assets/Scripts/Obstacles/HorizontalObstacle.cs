using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorizontalObstacle : Obstacle, IDynamicObstacle
{
     #region Variables
     [SerializeField]
     float movementRange = 5f;
     [SerializeField]
     float speed = 1f;
     [SerializeField]
     float delay = 2f;

     float elapsedTime = 0;

     Direction direction = Direction.Right;
     Rigidbody rb;

     bool stationary = false;
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
          rb.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, targetX, speed * Time.fixedDeltaTime), transform.position.y, transform.position.z));
          if (Vector3.Distance(transform.position, new Vector3(targetX, transform.position.y, transform.position.z)) < 0.5f)
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
}
