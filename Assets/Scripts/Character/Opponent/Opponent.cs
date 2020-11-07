using AI.States;
using System.Collections;
using System.Collections.Generic;
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

          public Transform target;

          float horizontalMax = 5f, verticalMax = 3f;

          [HideInInspector]
          public Vector3 startPosition;

          private StateMachine<Opponent> stateMachine = new StateMachine<Opponent>();

          public LevelManager LevelManager { get; private set; }

          #endregion

          protected virtual void Start()
          {
               LevelManager = LevelManager.Instance;
               startPosition = transform.position;
               Agent = GetComponent<NavMeshAgent>();
               Rb = GetComponent<Rigidbody>();
               Animator = GetComponent<Animator>();
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
               //Agent.enabled = false;
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
                              stateMachine.SetState(new MovingState(this));
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
               stateMachine.AddToCache((int)AIState.Moving, new MovingState(this));
               stateMachine.AddToCache((int)AIState.Down, new DownedState(this));
               stateMachine.AddToCache((int)AIState.Ended, new EndedState(this));
          }
          #endregion
     }