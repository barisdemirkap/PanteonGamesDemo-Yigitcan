using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
     /// <summary>
     /// This coroutine waits for animation state finish,  then invoke callback method.
     /// </summary>
     /// <param name="animator">Animator holding the animation state</param>
     /// <param name="stateName">Name of the state to be checked </param>
     /// <param name="callBack">Callback name to be called at the end of the process</param>
     /// <param name="extraTime">Extra time before callback function called</param>
     /// <param name="layer">State layer index in animator.</param>
     /// <returns></returns>
     public static IEnumerator WaitForAnimationFinish(Animator animator, string stateName, Action callBack, float extraTime = .1f, int layer = 0)
     {
          while (!animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
          {
               yield return null;
          }
          while (animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
          {
               if (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime > 1f)
               {
                    yield return new WaitForSeconds(extraTime);
                    callBack?.Invoke();
               }
               yield return null;
          }

     }
}
