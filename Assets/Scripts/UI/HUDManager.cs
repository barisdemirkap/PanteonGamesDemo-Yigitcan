using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
     #region Fields
     [Header("UI Components")]
     [Space]
     [SerializeField]
     private Image paintProgress;
     [SerializeField]
     private StatusText statusText;
     #endregion
     #region EngineMethods
     private void Start()
     {
          //if statusText is null try to find from scene
          if (statusText == null)
               statusText = FindObjectOfType<StatusText>();
     }
     #endregion
     #region ScriptMethods
     /// <summary>
     /// This method will update painting progress bar visual in UI
     /// </summary>
     /// <param name="paintArea">Painted area amount</param>
     /// <param name="totalArea">Total area amount</param>
     public void UpdatePaintingProgress(int paintArea, int totalArea)
     {
          paintProgress.fillAmount = ((float)paintArea / totalArea);
     }
     /// <summary>
     /// Changes status text without fade animation.
     /// </summary>
     /// <param name="message">Message to show</param>
     public void ChangeStatusTextDirect(string message)
     {
          statusText.gameObject.SetActive(true);
          statusText.ChangeTextDirectly(message);
     }

     /// <summary>
     /// Hide status text object if necessary
     /// </summary>
     public void HideStatusText() => statusText.gameObject.SetActive(false);
     /// <summary>
     /// Show status text with animation
     /// </summary>
     /// <param name="message">Status text message</param>
     public void ShowStatusText(string message)
     {
          statusText.gameObject.SetActive(true);
          StartCoroutine(statusText.ChangeStatus(message));
     }

     /// <summary>
     /// Reset scene for test
     /// </summary>
     public void ResetScene()=> SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
     #endregion
}
