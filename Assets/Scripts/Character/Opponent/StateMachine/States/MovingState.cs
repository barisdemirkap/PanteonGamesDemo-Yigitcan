using UnityEngine;

public class MovingState : AIStateMachine
{
     public MovingState(Opponent opponent) : base(opponent)
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
          opponent.Agent.isStopped = false;
     }
     public override void Update()
     {
          Animations();
          opponent.CheckForFalling();
     }

     void Animations()
     {
          worldDeltaPosition= opponent.Agent.nextPosition - opponent.transform.position;

          // Map 'worldDeltaPosition' to local space
          float dx = Vector3.Dot(opponent.transform.right, worldDeltaPosition);
          float dy = Vector3.Dot(opponent.transform.forward, worldDeltaPosition);
          deltaPosition = new Vector2(dx, dy);

          // Low-pass filter the deltaMove
          float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
          smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

          // Update velocity if time advances
          //1e-5f means 1 to power of -5 = 0.00001
          if (Time.deltaTime > 1e-5f)
               velocity = smoothDeltaPosition / Time.deltaTime;

          shouldMove = velocity.magnitude > 0.5f && opponent.Agent.remainingDistance > opponent.Agent.radius;

          // Update animation parameters
          opponent.Animator.SetBool("Moving", shouldMove);
          opponent.Animator.SetFloat("VelX", velocity.x);
          opponent.Animator.SetFloat("VelY", velocity.y);
     }
}
