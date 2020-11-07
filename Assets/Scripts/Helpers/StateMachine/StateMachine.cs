using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
     public Dictionary<int, State<T>> StateCache { get; protected set; }

     public State<T> currentState;

     public StateMachine()
     {
          StateCache = new Dictionary<int, State<T>>();
     }
     public void AddToCache(int key, State<T> state)
     {
          if (StateCache.ContainsKey(key))
               return;
          StateCache.Add(key, state);
     }
     public void RemoveFromCache(int key)
     {
          if (StateCache.ContainsKey(key))
               StateCache.Remove(key);

     }
     public void SetState(State<T> newState)
     {
          currentState?.OnStateExit();

          currentState = newState;

          currentState?.OnStateEnter();
     }
}
