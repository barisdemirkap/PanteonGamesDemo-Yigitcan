using Es.InkPainter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterController : MonoBehaviour
{
     #region Fields
     [Header("Mandatory Scripts")]
     [Space]
     [SerializeField]
     InkCanvas canvasObject;
     [SerializeField]
     InputManager inputManager;
     [SerializeField]
     private HUDManager hudManager;

     [Header("Script Properties")]
     [Space]
     [SerializeField]
     float speed = 5f, lerpTime = .15f;
     [SerializeField]
     private ParticleSystem spray;
     [SerializeField]
     private Brush brush;

     private MeshFilter meshFilter;
     private int totalTriangles;
     List<int> triangles = new List<int>();
     Vector2 direction = Vector2.zero;
     #endregion

     #region EngineMethods
     void Start()
     {
          spray.Stop();
          meshFilter = canvasObject.GetComponent<MeshFilter>();

          if (canvasObject == null)
               canvasObject = FindObjectOfType<InkCanvas>();
          if (inputManager != null)
               inputManager = FindObjectOfType<InputManager>();
          if (hudManager == null)
               hudManager = FindObjectOfType<HUDManager>();
          totalTriangles = meshFilter.mesh.triangles.Length / 3;
     }

     void Update()
     {
          if (LevelManager.Instance.LevelState==LevelState.Painting)
          {
               Painting();
          }
     }
     #endregion

     #region Script Methods
     void Painting()
     {
          if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
          {
               SetPainterObjectToTouchPosition();
          }
          
          if (Input.GetMouseButton(0) || Input.touchCount > 0)
          {
               PaintFunction();
          }
          if (Input.GetMouseButtonUp(0) || Input.touchCount < 0)
          {
               //clear particles
               spray.Stop();
               spray.Clear();
          }
     }

     void SetPainterObjectToTouchPosition()
     {
          //set spray can position to mouse position
          float dist = transform.position.z - Camera.main.transform.position.z;
          Vector3 pos = Input.mousePosition;
          pos.z = dist;
          pos = Camera.main.ScreenToWorldPoint(pos);
          pos.y = transform.position.y;
          transform.position = pos;
     }
     void PaintFunction()
     {
          //cast a ray from transform forward and paint hitpoint with brush texture
          Ray ray = new Ray(transform.position, transform.forward);
          RaycastHit hitInfo;
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
     #endregion
}
