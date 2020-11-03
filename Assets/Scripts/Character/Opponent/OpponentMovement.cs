using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OpponentMovement : Opponent
{

     public Transform target;
     Vector2 smoothDeltaPosition = Vector2.zero;
     Vector2 velocity = Vector2.zero;

     protected override void Start()
     {
          base.Start();
          navMeshAgent.SetDestination(target.position);
          navMeshAgent.updatePosition = false;
     }

     void Update()
     {
          Vector3 worldDeltaPosition = navMeshAgent.nextPosition - transform.position;

          // Map 'worldDeltaPosition' to local space
          float dx = Vector3.Dot(transform.right, worldDeltaPosition);
          float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
          Vector2 deltaPosition = new Vector2(dx, dy);

          // Low-pass filter the deltaMove
          float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
          smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

          // Update velocity if time advances
          //1e-5f means 1 to power of -5 = 0.00001
          if (Time.deltaTime > 1e-5f)
               velocity = smoothDeltaPosition / Time.deltaTime;
       
          bool shouldMove = velocity.magnitude > 0.5f && navMeshAgent.remainingDistance > navMeshAgent.radius;

          // Update animation parameters
          animator.SetBool("Moving", shouldMove);
          animator.SetFloat("VelX", velocity.x);
          animator.SetFloat("VelY", velocity.y);
     }

     private void OnAnimatorMove()
     {
          // Update position to agent position
          transform.position = navMeshAgent.nextPosition;
     }
}
