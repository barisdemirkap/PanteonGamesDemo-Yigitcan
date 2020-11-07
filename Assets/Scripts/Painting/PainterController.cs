using Es.InkPainter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterController : MonoBehaviour
{
     [SerializeField]
     float speed = 5f;
     [SerializeField]
     float lerpTime = .15f;

     [SerializeField]
     private ParticleSystem spray;

     [SerializeField]
     private Brush brush;

     private MeshFilter meshFilter;

     private int totalTriangles;

     List<int> triangles = new List<int>();

     Vector2 direction = Vector2.zero;

     InkCanvas canvasObject;
     InputManager inputManager;

     private HUDManager hudManager;
     void Start()
     {
          spray.Stop();
          canvasObject = FindObjectOfType<InkCanvas>();
          meshFilter = canvasObject.GetComponent<MeshFilter>();
          inputManager = FindObjectOfType<InputManager>();
          hudManager = FindObjectOfType<HUDManager>();

          totalTriangles = meshFilter.mesh.triangles.Length/3;
     }

     // Update is called once per frame
     void Update()
     {
          Ray ray = new Ray(transform.position, transform.forward);
          RaycastHit hitInfo;
          if (Input.GetMouseButton(0))
          {

               if (!spray.isPlaying)
                    spray.Play();
               if (Physics.Raycast(ray, out hitInfo))
               {
                    if (!triangles.Contains(hitInfo.triangleIndex))
                    {
                         triangles.Add(hitInfo.triangleIndex);
                         hudManager.UpdatePaintingProgress(triangles.Count, totalTriangles);
                    }

                    if (canvasObject != null)
                         canvasObject.Paint(brush, hitInfo);
               }
               direction = inputManager.DeltaPosition;
               transform.localPosition = new Vector3(
               Mathf.Lerp(transform.localPosition.x, Mathf.Clamp(transform.localPosition.x + (speed * direction.x) * Time.deltaTime, -2.5f, 2.5f), lerpTime),
                Mathf.Lerp(transform.localPosition.y, Mathf.Clamp(transform.localPosition.y + (speed * direction.y) * Time.deltaTime, -1f, 5f), lerpTime),
               transform.localPosition.z);
          }

          if (Input.GetMouseButtonUp(0))
          {
               spray.Stop();
               spray.Clear();
          }
     }
}
