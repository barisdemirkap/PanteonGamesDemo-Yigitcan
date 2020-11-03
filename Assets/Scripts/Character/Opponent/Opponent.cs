using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Concerete player class
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Opponent : AbstractCharacter
{
     #region Variables
     protected Animator animator;
     protected Rigidbody rb;
     protected NavMeshAgent navMeshAgent;
     protected bool isDown;
     #endregion



     protected virtual void Start()
     {
          navMeshAgent = GetComponent<NavMeshAgent>();
          rb = GetComponent<Rigidbody>();
          animator = GetComponent<Animator>();
     }


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
