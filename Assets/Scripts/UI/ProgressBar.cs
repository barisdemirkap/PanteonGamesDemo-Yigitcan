using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProgressBar : MonoBehaviour
{
     private Text progressText;
     private Image progressImage;
    void Start()
    {
          progressImage = GetComponent<Image>();
          progressText = GetComponentInChildren<Text>();
    }

     public void UpdateProgress(float value)
     {
          progressImage.fillAmount = value;
          progressText.text = value * 100 + "%";
     }
}
