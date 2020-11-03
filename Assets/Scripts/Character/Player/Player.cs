using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

     protected SceneManager sceneManager;
     protected InputManager inputManager;

     protected bool isDown;
     #endregion


     #region EngineMethods
     private void Start()
     {
          rb = GetComponent<Rigidbody>();
          animator = GetComponent<Animator>();
          sceneManager = SceneManager.Instance;
          inputManager = FindObjectOfType<InputManager>();
     }
     #endregion

     #region OtherMethods
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
