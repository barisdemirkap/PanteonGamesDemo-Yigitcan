using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finishline : MonoBehaviour
{
     #region Actions
     public Action<AbstractCharacter> OnFinishlinePassed;
     #endregion
     #region Engine Methods
     private void OnTriggerEnter(Collider other)
     {
          AbstractCharacter character;
          if (other.TryGetComponent<AbstractCharacter>(out character))
          {
               OnFinishlinePassed?.Invoke(character);
          }
     } 
     #endregion
}
