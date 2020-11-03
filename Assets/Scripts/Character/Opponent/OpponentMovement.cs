using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class OpponentMovement : Opponent
{

     public Transform target;

     Vector2 smoothDeltaPosition = Vector2.zero;
     Vector2 velocity = Vector2.zero;

     float horizontalMax = 5f, verticalMax = 3f;

     protected override void Start()
     {
          base.Start();
          navMeshAgent.updatePosition = false;
          CalculateDestination();
     }

     void Update()
     {
          Animations();
          if (SceneManager.Instance.GameState!=GameState.Playing)
          {
               navMeshAgent.isStopped = true;
          }
          else
          {
               navMeshAgent.isStopped = false;
          }
     }
     private void OnAnimatorMove()
     {
          // Update position to agent position
          transform.position = navMeshAgent.nextPosition;
     }
     void CalculateDestination()
     {
          Vector3 targetPosition = target.position;
          targetPosition.z += 2f;
          navMeshAgent.SetDestination(new Vector3(targetPosition.x + (Random.Range(-horizontalMax, horizontalMax)), targetPosition.y, targetPosition.z + (Random.Range(0,verticalMax))));
     }
     void Animations()
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

}
