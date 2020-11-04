using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfDonutObstacle : Obstacle, IDynamicObstacle
{
     #region Variables
     Rigidbody rb;

     [SerializeField]
     float rotationSpeed = 5f;
     [SerializeField]
     float lerpTreshold = 1f;
     [SerializeField]
     float rotateAmount = 90f;
     #endregion


     private void Start()
     {
          rb = GetComponent<Rigidbody>();
          Movement();
     }
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
               while (Quaternion.Angle(transform.localRotation, targetRotation)>lerpTreshold)
               {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
               }
               targetRotation = Quaternion.Euler(Vector3.zero);
               while (Quaternion.Angle(transform.localRotation, targetRotation)> lerpTreshold)
               {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
               }
               yield return null; 
          }
     }

}
