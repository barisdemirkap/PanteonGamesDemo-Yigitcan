namespace AI.States
{

     public class EndedState : State<Opponent>
     {
          public EndedState(Opponent context) : base(context)
          {
          }
          public override void OnStateEnter()
          {
               context.Animator.SetBool("Moving", false);
               context.Agent.isStopped = true;
               //TODO:play ended animation here
          }
          public override void Update() { }
     } 
}