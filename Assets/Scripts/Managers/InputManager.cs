using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
     Vector2 inputStartPosition, inputEndPosition;

     public Vector2 DeltaPosition { get { return deltaPosition; } }
     private Vector2 deltaPosition = Vector2.zero;
     float max = 100f;

     private bool userInput = false;

     private void Update()
     {
          userInput = Input.touchCount > 0 || Input.GetMouseButton(0);
          if (Input.GetMouseButtonDown(0))
          {
               inputStartPosition = Input.mousePosition;
          }
          else if (Input.GetMouseButton(0))
          {
               inputEndPosition = Input.mousePosition;
          }
          
          if (Input.touchCount > 0)
          {
               var touch = Input.GetTouch(0);

               if (touch.phase == TouchPhase.Began)
               {
                    inputStartPosition = touch.position;
               }
               else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
               {
                    inputEndPosition = touch.position;
               }
          }
          if (userInput)
          {
               Vector2 pos = inputEndPosition - inputStartPosition;
               deltaPosition.x = Mathf.Clamp(pos.x, -max, max) / max;
               deltaPosition.y = Mathf.Clamp(pos.y, -max, max) / max;
          }
          else
          {
               deltaPosition = Vector2.zero;
          }
     }
}
