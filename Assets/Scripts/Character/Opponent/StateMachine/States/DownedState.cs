public class DownedState : AIStateMachine
{
     public DownedState(Opponent opponent) : base(opponent)
     {
     }
     public override void OnStateEnter()
     {
          opponent.Agent.Warp(opponent.startPosition);
          opponent.Agent.isStopped = true;
          opponent.Animator.SetBool("GetHit", true);
          opponent.StartCoroutine(HelperMethods.WaitForAnimationFinish(opponent.Animator,"Downed",
               () =>
               {
                    opponent.CheckCacheAndChangeState(AIState.Moving);
               })); 
          
     }
     public override void Update()
     {
          opponent.CheckForFalling();
     }
     public override void OnStateExit()
     {
          opponent.Agent.enabled = true;
          opponent.Animator.SetBool("GetHit", false);
          opponent.CalculateDestination();

     }
}
