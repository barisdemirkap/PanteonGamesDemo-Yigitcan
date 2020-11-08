using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PlayerState : int { Idle = 0, Moving, Down, Ended }
/// <summary>
/// Concrete player class
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
     [SerializeField]
     protected TextMeshPro positionText;
     [SerializeField]
     protected InputManager inputManager;
     [SerializeField]
     private float forceRequiredForFall=10f;
     protected bool isDown;

     #endregion

     #region EngineMethods
     private void Start()
     {
          rb = GetComponent<Rigidbody>();
          animator = GetComponent<Animator>();
          levelManager = LevelManager.Instance;
          if (inputManager == null)
               inputManager = FindObjectOfType<InputManager>();
          positionText = GetComponentInChildren<TextMeshPro>();
          levelManager.OnPlayerFinished += DanceAnimations;
     }
     private void LateUpdate()
     {
          if (levelManager.LevelState == LevelState.Playing)
          {
               UpdatePositionText(positionText);
          }
     }
     private void OnDisable()
     {
          levelManager.OnPlayerFinished -= DanceAnimations;
     }
     #endregion

     #region OtherMethods
     private void DanceAnimations(int finishedPosition)
     {
          //push the character little forward for better camera view 
          transform.position = transform.position + transform.forward * 2f;
          positionText.gameObject.SetActive(false);
          animator.SetFloat("VelX", 0f);
          animator.SetFloat("VelY", 0f);
          if (finishedPosition == 0)
               //Victory Dance !!!!
               animator.SetTrigger("Victory");
          else
               animator.SetTrigger("Defeated");
     }

     public override void ReceiveForce(Vector3 direction, float forceStrength)
     {

          rb.AddForce(direction * forceStrength, ForceMode.Force);
          if (forceStrength>=forceRequiredForFall)
          {
               isDown = true;
               animator.SetBool("GetHit", isDown);
               StartCoroutine(HelperMethods.WaitForAnimationFinish(animator, "Downed", () =>
               {
                    isDown = false;
                    animator.SetBool("GetHit", isDown);
                    rb.velocity = Vector3.zero;
               }, 0f));
          }
     }

     public void Jump(Vector3 direction)
     {
          rb.AddForce(direction, ForceMode.Impulse);
     }
     public override void UpdatePositionText(TextMeshPro textObject)
     {
          positionText.text = levelManager.GetPositionData(this).ToString();
     }
     #endregion
}
