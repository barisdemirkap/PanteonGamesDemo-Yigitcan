using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
     public class IdleState : State<Opponent>
     {
          public IdleState(Opponent context) : base(context)
          {
          }

          public override void OnStateEnter()
          {
               context.LevelManager.OnFirstTapGiven += TransitionToMovingState;
               context.Animator.SetBool("Moving", false);
               context.Agent.isStopped = true;
               context.CalculateDestination();
          }
          public override void Update() { }
          public override void OnStateExit()
          {
               context.LevelManager.OnFirstTapGiven -= TransitionToMovingState;
          }
          void TransitionToMovingState()
          {
               context.CheckCacheAndChangeState(AIState.Moving);
          }
     }  
}

