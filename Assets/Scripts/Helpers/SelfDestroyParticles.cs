
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyParticles : MonoBehaviour
{
     [SerializeField]
     private float delayTime = 1f;

     private void Start()
     {
          //destroy object with a delay after particle completed
          Destroy(gameObject, GetComponent<ParticleSystem>().main.duration+delayTime);
     }
}
