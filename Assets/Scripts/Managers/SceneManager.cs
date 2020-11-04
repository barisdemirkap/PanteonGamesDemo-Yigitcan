using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Utility;

public class SceneManager : Singleton<SceneManager>
{
     public GameState GameState { get; private set; }

     public Vector3 startPoint = Vector3.zero;
     public List<AbstractCharacter> Characters { get { return characters; } }

     private List<AbstractCharacter> characters = new List<AbstractCharacter>();

     private List<AbstractCharacter> finishedPlayers = new List<AbstractCharacter>();


     public Player player;
     public List<Opponent> opponents = new List<Opponent>();

     private Finishline finishline;

     [SerializeField]
     private Camera paintCamera;
     [SerializeField]
     private GameObject paintingWall;


     #region Events
     public Action OnInitalized;
     public Action OnFirstTapGiven;
     public Action OnPlayerFinished;
     #endregion


     #region Engine Methods
     protected override void Awake()
     {
          base.Awake();
          finishline = FindObjectOfType<Finishline>();
          SetupScene();
          SetGameState(GameState.Initialized);
          OnInitalized?.Invoke();
     }

     private void Update()
     {
          if (GameState != GameState.Playing && GameState == GameState.Initialized)
          {
               if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
               {
                    SetGameState(GameState.Playing);
                    OnFirstTapGiven?.Invoke();
               }
          }
     }
     private void OnDisable()
     {
          finishline.OnFinishlinePassed -= FinishlinePassed;
     }
     #endregion

     void GetPositionData()
     {
          characters.Sort((x, y) => y.GetDistanceTravelled(startPoint, y.transform).CompareTo(x.GetDistanceTravelled(startPoint, x.transform)));
     }
     public void SetGameState(GameState state)
     {
          //Changes the state
          GameState = state;
     }

     void SetupScene()
     {
          //subscribe to finishline passed action
          finishline.OnFinishlinePassed += FinishlinePassed;

          player = FindObjectOfType<Player>();
          characters.Add(player);

          opponents.AddRange(FindObjectsOfType<Opponent>());
          characters.AddRange(opponents);

          Debug.Log("Total characters: " + characters.Count);
          Debug.Log("Opponent characters: " + opponents.Count);
     }

     private void FinishlinePassed(AbstractCharacter obj)
     {
          finishedPlayers.Add(obj);
          if (obj is Player)
          {
               PaintingState();
          }
     }
     private void PaintingState()
     {
          SetGameState(GameState.Painting);
          OnPlayerFinished?.Invoke();
          StartCoroutine(TransitionToPaintingState());
     }

     IEnumerator TransitionToPaintingState()
     {
          //TODO: Some celebration particles and animations and transition to paint camera
          Camera camera = Camera.main;
          camera.GetComponent<SmoothFollow>().enabled = false;
          Transform target = player.transform.Find("CameraTarget");

          while (Vector3.Distance(camera.transform.position, target.transform.position) > .1f)
          {
               camera.transform.position = Vector3.Lerp(camera.transform.position, target.transform.position, 1f * Time.deltaTime);
               Vector3 targetAngle = new Vector3(
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.x, target.rotation.eulerAngles.x, 1f * Time.deltaTime),
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.y, target.rotation.eulerAngles.y, 1f * Time.deltaTime),
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.z, target.rotation.eulerAngles.z, 1f * Time.deltaTime)
                    );
               camera.transform.eulerAngles = targetAngle;
               print("moving");
               yield return null;
          }

          paintingWall.SetActive(true);
          paintCamera.gameObject.SetActive(true);
          camera.gameObject.SetActive(false);
          print("completed");
     }
}
