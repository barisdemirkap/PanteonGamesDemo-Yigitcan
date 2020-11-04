using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState { Idle, Moving,Down,Ended}
public abstract class AIStateMachine
{

     protected Opponent opponent;

     public abstract void Update();

     public virtual void OnStateEnter() { }
     public virtual void OnStateExit() { }

     protected AIStateMachine(Opponent opponent)
     {
          this.opponent = opponent;
     }
}
