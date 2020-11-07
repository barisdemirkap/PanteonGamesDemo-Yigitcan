using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
     [Header("UI Components")]
     [Space]

     
     [SerializeField]
     private Text positionText;
     [SerializeField]
     private Image paintProgress;
     [SerializeField]
     private StatusText statusText;

     public void UpdatePosition(int position,int total)
     {
          positionText.text = position + "/" + total;
     }
     public void UpdatePaintingProgress(int paintArea,int totalArea)
     {
          paintProgress.fillAmount =((float)paintArea / totalArea);
     }
     public void ChangeStatusTextDirect(string message)
     {
          statusText.gameObject.SetActive(true);
          statusText.ChangeTextDirectly(message);
     }
     public void HideStatusText() => statusText.gameObject.SetActive(false);
     public void ShowStatusText(string message)
     {
          statusText.gameObject.SetActive(true);
          StartCoroutine(statusText.ChangeStatus(message));
     }

     public void ResetScene()
     {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
     }
}
