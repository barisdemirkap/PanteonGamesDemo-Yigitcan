using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProgressBar : MonoBehaviour
{
     #region Fields
     private Text progressText;
     private Image progressImage;
     #endregion
     #region Engine Methods
     void Start()
     {
          progressImage = GetComponent<Image>();
          progressText = GetComponentInChildren<Text>();
     }
     #endregion
     #region Script Methods

     public void UpdateProgress(float value)
     {
          progressImage.fillAmount = value;
          progressText.text = value * 100 + "%";
     } 
     #endregion
}
