using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState : int { Idle = 0, Moving, Down, Ended }
/// <summary>
/// Concerete player class
/// </summary>
/// 
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Player : AbstractCharacter
{
     #region Variables
     protected Animator animator;
     protected Rigidbody rb;
     protected LevelManager levelManager;
     protected InputManager inputManager;

     protected bool isDown;

     #endregion

     #region EngineMethods
     private void Start()
     {
          rb = GetComponent<Rigidbody>();
          animator = GetComponent<Animator>();
          levelManager = LevelManager.Instance;
          inputManager = FindObjectOfType<InputManager>();
          levelManager.OnPlayerFinished += StopAnimations;
     }

     private void OnDisable()
     {
          levelManager.OnPlayerFinished -= StopAnimations;
     }
     #endregion

     #region OtherMethods
     private void StopAnimations()
     {
          animator.SetFloat("VelX", 0f);
          animator.SetFloat("VelY", 0f);
     }
     public override void ReceiveForce(Vector3 direction, float forceStrength)
     {
          isDown = true;
          rb.AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
          animator.SetBool("GetHit", isDown);
          StartCoroutine(HelperMethods.WaitForAnimationFinish(animator, "Downed", () =>
          {
               isDown = false;
               animator.SetBool("GetHit", isDown);
          }, 0f));
     }
     #endregion
}
