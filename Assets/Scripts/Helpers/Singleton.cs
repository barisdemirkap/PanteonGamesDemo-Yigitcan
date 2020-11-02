using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{

     /// <summary>
     /// Singleton only let one instace of Type T per scene.
     /// <para>Note: Singleton instance can be destroyed.</para>
     /// </summary>
     /// <typeparam name="T">T inherits from MonoBehavior</typeparam>
     public class Singleton<T> : MonoBehaviour where T : Singleton<T>
     {
          private static T instance;
          public bool isPersistent = false;
          public static T Instance
          {
               get
               {
                    if (instance == null)
                         instance = FindObjectOfType<T>();
                    if (instance == null)
                         Debug.LogError("Singleton<" + typeof(T) + "> instance has been not found.");
                    return instance;
               }
          }

          protected virtual void Awake()
          {
               if (this.GetType() != typeof(T))
                    DestroySelf();
               if (instance == null)
                    instance = this as T;
               else if (instance != this)
                    DestroySelf();
               if (isPersistent)
                    DontDestroyOnLoad(this);
          }

          protected void OnValidate()
          {
               if (this.GetType() != typeof(T)) //Change to solve the problem
               {
                    Debug.LogError("Singleton<" + typeof(T) + "> has a wrong Type Parameter. " +
                        "Try Singleton<" + this.GetType() + "> instead.");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.delayCall -= DestroySelf;
                    UnityEditor.EditorApplication.delayCall += DestroySelf;
#endif
               }

               if (instance == null)
                    instance = this as T;
               else if (instance != this)
               {
                    Debug.LogError("Singleton<" + this.GetType() + "> already has an instance on scene. Component will be destroyed.");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.delayCall -= DestroySelf;
                    UnityEditor.EditorApplication.delayCall += DestroySelf;
#endif
               }
          }


          private void DestroySelf()
          {
               if (Application.isPlaying)
                    Destroy(this);
               else
                    DestroyImmediate(this);
          }
     }
}