using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
     Vector2 inputStartPosition, inputEndPosition;

     public Vector2 DeltaPosition { get { return deltaPosition; } }
     private Vector2 deltaPosition;

     bool userInput = false;

     private void Update()
     {
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
          userInput = Input.touchCount > 0 || Input.GetMouseButton(0);

          if (userInput)
          {
               deltaPosition = inputEndPosition - inputStartPosition;
               deltaPosition = deltaPosition.normalized;
          }
          else
          {
               deltaPosition = Vector2.zero;
          }
     }
}
