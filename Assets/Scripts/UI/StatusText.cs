using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusText :MonoBehaviour
{
     #region Fields
     private Text statusText;
     private CanvasRenderer canvasRenderer;
     #endregion

     #region Engine Methods
     private void Awake()
     {
          statusText = GetComponent<Text>();
          canvasRenderer = GetComponent<CanvasRenderer>();
     }
     #endregion

     #region Script Methods
     public void ChangeTextDirectly(string message)
     {
          statusText.text = message;
     }
     /// <summary>
     /// Shows status text with fade animation
     /// </summary>
     /// <param name="displayText"></param>
     /// <returns></returns>
     public IEnumerator ChangeStatus(string displayText)
     {
          canvasRenderer.SetAlpha(0);
          statusText.text = displayText;
          statusText.CrossFadeAlpha(1, 1.5f, false);
          yield return new WaitForSeconds(1.51f);
          statusText.CrossFadeAlpha(0, 1.5f, false);
          yield return new WaitForSeconds(1.51f);
          gameObject.SetActive(false);
     } 
     #endregion
}