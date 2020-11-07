namespace AI.States
{
     public class DownedState : State<Opponent>
     {
          public DownedState(Opponent context) : base(context)
          {
          }
          public override void OnStateEnter()
          {
               context.Agent.isStopped = true;
               context.Animator.SetBool("GetHit", true);
               context.StartCoroutine(HelperMethods.WaitForAnimationFinish(context.Animator, "Downed",
                    () =>
                    {
                         context.CheckCacheAndChangeState(AIState.Moving);
                    }));

          }
          public override void Update() { }
          public override void OnStateExit()
          {
               context.Agent.enabled = true;
               context.Agent.Warp(context.startPosition);
               context.Animator.SetBool("GetHit", false);
               context.CalculateDestination();

          }
     }
}