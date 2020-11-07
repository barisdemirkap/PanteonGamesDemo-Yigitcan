using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusText :MonoBehaviour
{
     private Text statusText;

     private void Awake()
     {
          statusText = GetComponent<Text>();
     }

     public void ChangeTextDirectly(string message)
     {
          statusText.text = message;
     }
     public IEnumerator ChangeStatus(string displayText)
     {
          statusText.text = displayText;
          statusText.CrossFadeAlpha(1, 1.5f, false);
          yield return new WaitForSeconds(1.51f);
          statusText.CrossFadeAlpha(0, 1.5f, false);
          yield return new WaitForSeconds(1.51f);
          gameObject.SetActive(false);
     }
}