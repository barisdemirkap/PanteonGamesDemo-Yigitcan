using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     #region Variables
     [SerializeField]
     float moveSpeed = 5f;
     [SerializeField]
     float lerpTime = 5f;

     bool isMoving = false;
     Vector3 newPosition = new Vector3();

     //Input vectors
     Vector2 touchStartPosition, touchEndPosition;

     Vector2 inputDirection = Vector2.zero;

     float min = -100f, max = 100f;

     Animator animator;
     Rigidbody rb;
     #endregion

     private SceneManager sceneManager;

     void Start()
     {
          rb = GetComponent<Rigidbody>();
          animator = GetComponent<Animator>();
          sceneManager = SceneManager.Instance;
     }

     void Update()
     {
          MouseInput();
          TouchInput();
     }
     private void MouseInput()
     {
          if (Input.GetMouseButtonDown(0))
          {
               touchStartPosition = Input.mousePosition;
          }
          else if (Input.GetMouseButton(0))
          {
               touchEndPosition = Input.mousePosition;

               float x = touchEndPosition.x - touchStartPosition.x;
               float y = touchEndPosition.y - touchStartPosition.y;

               x = Mathf.Clamp(x, min, max);
               y = Mathf.Clamp(y, min, max);
               x = x / max;
               y = y / max;
               //get input axis from touch or mouse;
               isMoving = true;
               //calculate new position for player
               newPosition = new Vector3(transform.position.x + moveSpeed * x, 0, transform.position.z + moveSpeed * y);
               inputDirection = new Vector2(x, y);
          }
          else
          {
               isMoving = false;
               //stop player velocity
               newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
          }
     }
     private void TouchInput()
     {

          if (Input.touchCount > 0)
          {
              var touch = Input.GetTouch(0);

               if (touch.phase == TouchPhase.Began)
               {
                    touchStartPosition = touch.position;
               }

               else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
               {
                    touchEndPosition = touch.position;

                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;
                    x=Mathf.Clamp(x, min, max);
                    y=Mathf.Clamp(y, min, max);
                    x = x / max;
                    y = y / max;

                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                    {
                         isMoving = false;
                    }
                    else
                    {
                         isMoving = true;
                         newPosition = new Vector3(transform.position.x + moveSpeed * x, transform.position.y, transform.position.z + moveSpeed * y);
                         inputDirection = new Vector2(x, y);
                    }
               }

               if (touch.phase == TouchPhase.Ended)
                    isMoving = false; 
          }
     }
     private void FixedUpdate()
     {
          if (sceneManager.GameState == GameState.Playing)
          {
               MovePlayer();
          }
     }

     private void MovePlayer()
     {

          if (isMoving)
          {
               //rotate player while moving
               rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(newPosition), 0.15f));
               //move player with rigidbody
               rb.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, newPosition.x, lerpTime * Time.fixedDeltaTime), transform.position.y, Mathf.Lerp(transform.position.z, newPosition.z, lerpTime * Time.fixedDeltaTime)));
               //start moving animation
               animator.SetFloat("VelX",inputDirection.x);
               animator.SetFloat("VelY",inputDirection.y);
          }
          else
          {
               //if player is not moving, set animation to Idle 
               animator.SetFloat("VelX", 0);
               animator.SetFloat("VelY", 0);
          }
     }
}
