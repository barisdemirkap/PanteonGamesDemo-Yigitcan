using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AIStateMachine
{
     public IdleState(Opponent opponent) : base(opponent)
     {
     }

     public override void OnStateEnter()
     {
          opponent.SceneManager.OnFirstTapGiven += TransitionToMovingState;
          opponent.Animator.SetBool("Moving", false);
          opponent.Agent.isStopped = true;
          opponent.CalculateDestination();
     }
     public override void Update() { }
     public override void OnStateExit()
     {
          opponent.SceneManager.OnFirstTapGiven -= TransitionToMovingState;
     }
     void TransitionToMovingState()
     {
          opponent.CheckCacheAndChangeState(AIState.Moving);
     }
}
