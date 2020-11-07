using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfDonutMovingObstacle : Obstacle, IDynamicObstacle
{
     #region Variables
     Rigidbody rb;

     [SerializeField]
     float moveSpeed = 5f, lerpThreshold = 0.1f;

     Vector3 startPosition = Vector3.zero;

     Vector3 targetPosition = Vector3.zero;


     #endregion

     #region Engine Methods
     void Start()
     {
          rb = GetComponent<Rigidbody>();
          startPosition = transform.localPosition;
          targetPosition = new Vector3(-startPosition.x, startPosition.y, startPosition.z);
          Movement();
     }
     #endregion


     #region OtherMethods
     public void Movement()
     {
          StartCoroutine(MoveDonut());
     }

     private IEnumerator MoveDonut()
     {
          while (true)
          {
               int interval = UnityEngine.Random.Range(1, 5);
               yield return new WaitForSecondsRealtime(interval);
               while (Vector3.Distance(transform.localPosition, targetPosition) > lerpThreshold)
               {
                   transform.localPosition=Vector3.Lerp(transform.localPosition, targetPosition, moveSpeed * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
               }
               while (Vector3.Distance(transform.localPosition, startPosition) > lerpThreshold)
               {
                    transform.localPosition = Vector3.Lerp(transform.localPosition,startPosition, moveSpeed * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
               }
               yield return null;
          }
     }

     #endregion

}
