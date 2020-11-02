using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyParticles : MonoBehaviour
{
     private void Start()
     {
          Destroy(gameObject, GetComponent<ParticleSystem>().main.duration+1f);
     }
}
