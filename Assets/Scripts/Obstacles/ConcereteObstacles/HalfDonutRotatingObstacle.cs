using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfDonutRotatingObstacle : Obstacle, IDynamicObstacle
{
     #region Fields
     Rigidbody rb;

     [SerializeField]
     float rotationSpeed = 5f;
     [SerializeField]
     float lerpThreshold = 1f;
     [SerializeField]
     float rotateAmount = 90f;
     #endregion

     #region Engine Methods
     protected override void Start()
     {
          base.Start();
          rb = GetComponent<Rigidbody>();
          Movement();
     }
     #endregion

     #region Script Method
     public void Movement()
     {
          StartCoroutine(RotateDonut());
     }
     IEnumerator RotateDonut()
     {
          while (true)
          {
               int interval = UnityEngine.Random.Range(1, 5);
               Quaternion targetRotation = Quaternion.Euler(new Vector3(rotateAmount, 0, 0));
               yield return new WaitForSecondsRealtime(interval);
               while (Quaternion.Angle(transform.localRotation, targetRotation) > lerpThreshold)
               {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
               }
               targetRotation = Quaternion.Euler(Vector3.zero);
               while (Quaternion.Angle(transform.localRotation, targetRotation) > lerpThreshold)
               {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
               }
               yield return null;
          }
     }
     #endregion
}
