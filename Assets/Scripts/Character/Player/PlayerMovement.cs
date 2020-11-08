using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : Player
{
     #region Variables
     [SerializeField]
     float moveSpeed = 5f;
     [SerializeField]
     float lerpTime = 5f;
     [SerializeField]
     float fallingThreshold = 5f;

     Vector3 newPosition =Vector3.zero;

     Vector2 inputDirection = Vector2.zero;

     #endregion
     void SetInputDirections()
     {
          inputDirection = inputManager.DeltaPosition;
     }
     private void Update()
     {
          SetInputDirections();
          if (transform.position.y < -fallingThreshold)
          {
               transform.position = levelManager.startPoint;
          }
     }
     private void FixedUpdate()
     {
          if (levelManager.LevelState == LevelState.Playing)
          {
               MovePlayer();
          }
     }
     private void MovePlayer()
     {
          //rotate our player if its not downed state
          if (!isDown)
          {
               newPosition = new Vector3(transform.position.x + moveSpeed * inputDirection.x, 0, transform.position.z + moveSpeed * inputDirection.y);
               //rotate player while moving
               rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(newPosition), 0.15f));
               ////move player with rigidbody
               rb.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, newPosition.x, lerpTime * Time.fixedDeltaTime), transform.position.y, Mathf.Lerp(transform.position.z, newPosition.z, lerpTime * Time.fixedDeltaTime)));
               //start moving animation
               animator.SetFloat("VelX", inputDirection.x);
               animator.SetFloat("VelY", inputDirection.y);
          }
     }
}
