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
     private StatusText statusText;
     [SerializeField]
     private ProgressBar progressBar;
     [SerializeField]
     private Button nextLevelButton;
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
     /// Show painting progress UI components
     /// </summary>
     public void ShowPaintingProgress() => progressBar.gameObject.SetActive(true);
     /// <summary>
     /// This method will update painting progress bar visual in UI
     /// </summary>
     /// <param name="paintArea">Painted area amount</param>
     /// <param name="totalArea">Total area amount</param>
     public void UpdatePaintingProgress(int paintArea, int totalArea)
     {
          var progress = ((float)paintArea / totalArea);
          if (progress>.75f)
          {
               nextLevelButton.gameObject.SetActive(true);
          }
          progressBar.UpdateProgress(progress);
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
     /// Show status text with animation stated displayTime
     /// </summary>
     /// <param name="message">Status text message</param>
     /// <param name="displayTime">Text display time before fading</param>
     public void ShowStatusText(string message, float displayTime)
     {
          statusText.gameObject.SetActive(true);
          StartCoroutine(statusText.ChangeStatus(message,displayTime));
     }

     /// <summary>
     /// Reset scene for test
     /// </summary>
     public void ResetScene()=> SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
     #endregion
}
