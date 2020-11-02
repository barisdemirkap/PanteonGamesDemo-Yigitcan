using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentMovement : MonoBehaviour
{
     NavMeshAgent navMeshAgent;

     public Transform target;
    void Start()
    {
          navMeshAgent = GetComponent<NavMeshAgent>();
          navMeshAgent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
