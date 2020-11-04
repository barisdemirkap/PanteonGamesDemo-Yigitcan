using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Concerete opponent class
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Opponent : AbstractCharacter
{
     #region Variables
     public Animator Animator { get; private set; }
     public Rigidbody Rb { get; private set; }
     public NavMeshAgent Agent { get; private set; }

     public Transform target;

     float horizontalMax = 5f, verticalMax = 3f;

     [HideInInspector]
     public Vector3 startPosition;

     private Dictionary<int, AIStateMachine> states = new Dictionary<int, AIStateMachine>();

     protected AIStateMachine currentState;

     public SceneManager SceneManager { get; private set; }

     #endregion

     protected virtual void Start()
     {
          SceneManager = SceneManager.Instance;
          startPosition = transform.position;
          Agent = GetComponent<NavMeshAgent>();
          Rb = GetComponent<Rigidbody>();
          Animator = GetComponent<Animator>();

          Agent.updatePosition = false;
          //set state to Idle
          CheckCacheAndChangeState(AIState.Idle);

     }
     private void Update()
     {
          currentState?.Update();
     }
     private void OnAnimatorMove()
     {
          transform.position = Agent.nextPosition;
     }
     #region OtherMethods
     public override void ReceiveForce(Vector3 direction, float forceStrength)
     {
          //print(direction);
          //Rb.AddForce(direction.normalized * forceStrength, ForceMode.Impulse);
          CheckCacheAndChangeState(AIState.Down);
     }
     public void CalculateDestination()
     {
          Vector3 targetPosition = target.position;
          targetPosition.z += 2f;
          Agent.SetDestination(new Vector3(targetPosition.x + (Random.Range(-horizontalMax, horizontalMax)), targetPosition.y, targetPosition.z + (Random.Range(0, verticalMax))));
     }
     public void CheckForFalling()
     {
          //TODO:check if they are falling, transform check doesnt work
     }
     public void SetState(AIStateMachine newState)
     {
          Debug.LogError(name + "  State Changed : " + newState.GetType());
          currentState?.OnStateExit();

          currentState = newState;

          currentState?.OnStateEnter();
     }
     public void CheckCacheAndChangeState(AIState state)
     {
          if (states.TryGetValue((int)state, out AIStateMachine value))
               SetState(value);
          else
          {
               switch (state)
               {
                    case AIState.Idle:
                         SetState(new IdleState(this));
                         break;
                    case AIState.Moving:
                         SetState(new MovingState(this));
                         break;
                    case AIState.Down:
                         SetState(new DownedState(this));
                         break;
                    case AIState.Ended:
                         SetState(new EndedState(this));
                         break;
               }
          }      
     }
     public void CacheStates()
     {
          states.Add((int)AIState.Idle, new IdleState(this));
          states.Add((int)AIState.Moving, new MovingState(this));
          states.Add((int)AIState.Down, new DownedState(this));
          states.Add((int)AIState.Ended, new EndedState(this));
     }
     #endregion
}
