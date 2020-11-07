using UnityEngine;

namespace AI.States
{
     public class MovingState : State<Opponent>
     {

          public MovingState(Opponent context) : base(context)
          {
          }
          #region Variables

          Vector2 smoothDeltaPosition = Vector2.zero;
          Vector2 velocity = Vector2.zero;
          Vector3 worldDeltaPosition = Vector3.zero;
          Vector2 deltaPosition = Vector2.zero;

          bool shouldMove;


          #endregion

          public override void OnStateEnter()
          {
               context.Agent.isStopped = false;
          }
          public override void Update()
          {
               Animations();
               context.CheckForFalling();
               CheckReachedDestination();

          }

          void Animations()
          {
               worldDeltaPosition = context.Agent.nextPosition - context.transform.position;

               // Map 'worldDeltaPosition' to local space
               float dx = Vector3.Dot(context.transform.right, worldDeltaPosition);
               float dy = Vector3.Dot(context.transform.forward, worldDeltaPosition);
               deltaPosition = new Vector2(dx, dy);

               // Low-pass filter the deltaMove
               float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
               smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

               // Update velocity if time advances
               //1e-5f means 1 to power of -5 = 0.00001
               if (Time.deltaTime > 1e-5f)
                    velocity = smoothDeltaPosition / Time.deltaTime;

               shouldMove = velocity.magnitude > 0.5f && context.Agent.remainingDistance > context.Agent.radius;

               // Update animation parameters
               context.Animator.SetBool("Moving", shouldMove);
               context.Animator.SetFloat("VelX", velocity.x);
               context.Animator.SetFloat("VelY", velocity.y);
          }

          void CheckReachedDestination()
          {
               if (!context.Agent.pathPending)
               {
                    if (context.Agent.remainingDistance <= context.Agent.stoppingDistance)
                    {
                         if (!context.Agent.hasPath || context.Agent.velocity.sqrMagnitude == 0f)
                         {
                              context.CheckCacheAndChangeState(AIState.Ended);
                         }
                    }
               }
          }
     }  
}
