using AI.States;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum AIState { Idle, Moving, Down, Ended }
/// <summary>
/// Concerete opponent class
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Opponent : AbstractCharacter
{
     #region Fields
     public Animator Animator { get; private set; }
     public Rigidbody Rb { get; private set; }
     public NavMeshAgent Agent { get; private set; }
     public LevelManager LevelManager { get; private set; }

     [HideInInspector]
     public Vector3 startPosition;

     //Destination calculate variables
     float horizontalMax = 5f, verticalMax = 3f;
     [SerializeField]
     private Finishline target;
     [SerializeField]
     private TextMeshPro positionText;

     //Finite State Machine
     private StateMachine<Opponent> stateMachine = new StateMachine<Opponent>();


     #endregion

     protected virtual void Start()
     {
          startPosition = transform.position;
          LevelManager = LevelManager.Instance;
          Agent = GetComponent<NavMeshAgent>();
          Rb = GetComponent<Rigidbody>();
          Animator = GetComponent<Animator>();
          if (target==null)
               target = FindObjectOfType<Finishline>();

          Agent.updatePosition = false;
          //cache states for better memory control
          CacheStates();
          //set state to Idle
          CheckCacheAndChangeState(AIState.Idle);
     }
     private void Update()
     {
          stateMachine.currentState?.Update();
     }
     private void OnAnimatorMove()
     {
          transform.position = Agent.nextPosition;
     }

     #region OtherMethods
     public override void ReceiveForce(Vector3 direction, float forceStrength)
     {
          CheckCacheAndChangeState(AIState.Down);
     }
     public override void UpdatePositionText(TextMeshPro textObject)
     {
          textObject.text = LevelManager.GetPositionData(this).ToString();
     }
     public void CalculateDestination()
     {
          Vector3 targetPosition = target.gameObject.transform.position;
          targetPosition.z += 2f;
          Agent.SetDestination(new Vector3(targetPosition.x + (Random.Range(-horizontalMax, horizontalMax)), targetPosition.y, targetPosition.z + (Random.Range(0, verticalMax))));
     }
     public void CheckForFalling()
     {
          if (!Agent.isOnNavMesh)
          {
               Agent.Warp(startPosition);
               CalculateDestination();
               CheckCacheAndChangeState(AIState.Moving);
          }
     }
     public void CheckCacheAndChangeState(AIState state)
     {
          if (stateMachine.StateCache.TryGetValue((int)state, out State<Opponent> value))
               stateMachine.SetState(value);
          else
          {
               switch (state)
               {
                    case AIState.Idle:
                         stateMachine.SetState(new IdleState(this));
                         stateMachine.AddToCache((int)AIState.Idle, stateMachine.currentState);
                         break;
                    case AIState.Moving:
                         stateMachine.SetState(new MovingState(this,ref positionText));
                         stateMachine.AddToCache((int)AIState.Moving, stateMachine.currentState);
                         break;
                    case AIState.Down:
                         stateMachine.SetState(new DownedState(this));
                         stateMachine.AddToCache((int)AIState.Down, stateMachine.currentState);
                         break;
                    case AIState.Ended:
                         stateMachine.SetState(new EndedState(this));
                         stateMachine.AddToCache((int)AIState.Ended, stateMachine.currentState);
                         break;
               }
          }
     }
     public void CacheStates()
     {
          stateMachine.AddToCache((int)AIState.Idle, new IdleState(this));
          stateMachine.AddToCache((int)AIState.Moving, new MovingState(this,ref positionText));
          stateMachine.AddToCache((int)AIState.Down, new DownedState(this));
          stateMachine.AddToCache((int)AIState.Ended, new EndedState(this));
     }

     #endregion
}