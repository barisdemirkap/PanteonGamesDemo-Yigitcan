using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finishline : MonoBehaviour
{
     public Action<AbstractCharacter> OnFinishlinePassed;
     private void OnTriggerEnter(Collider other)
     {
          AbstractCharacter character;
          if (other.TryGetComponent<AbstractCharacter>(out character))
          {
               OnFinishlinePassed?.Invoke(character);
          }
     }
}
