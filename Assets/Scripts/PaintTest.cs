using Es.InkPainter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintTest : MonoBehaviour
{
		 [SerializeField]
		 private Brush brush;

		 Texture mainTexture;

     private void Start()
     {
					mainTexture = GameObject.FindObjectOfType<InkCanvas>().gameObject.GetComponent<MeshRenderer>().material.mainTexture;
					print(mainTexture.name);
     }
     private void Update()
		 {
					if (Input.GetMouseButton(0))
					{
							 var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
							 RaycastHit hitInfo;
							 if (Physics.Raycast(ray, out hitInfo))
							 {
										var paintObject = hitInfo.transform.GetComponent<InkCanvas>();
										if (paintObject != null)
												 paintObject.Paint(brush, hitInfo);
							 }
					}
		 }
}
