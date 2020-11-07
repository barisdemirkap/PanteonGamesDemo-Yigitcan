using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Utility;
/// <summary>
/// Enum for controling game flow.
/// </summary>
public enum LevelState { Initialized, Playing, Paused, Result, Painting, Ended }
public class LevelManager : Singleton<LevelManager>
{
     #region Fields
     [SerializeField]
     private GameObject paintStage;

     public LevelState LevelState { get; private set; }

     public Vector3 startPoint = Vector3.zero;
     public List<AbstractCharacter> Characters { get { return characters; } }

     private List<AbstractCharacter> characters = new List<AbstractCharacter>();

     //Finished characters
     private List<AbstractCharacter> finishedCharacters = new List<AbstractCharacter>();

     public Player player;
     public List<Opponent> opponents = new List<Opponent>();

     private Finishline finishline;

     private HUDManager hudManager;
     #endregion

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
          hudManager = FindObjectOfType<HUDManager>();
          SetupScene();
          //subscribe to finishline passed action
          finishline.OnFinishlinePassed += FinishlinePassed;

     }
     private void Update()
     {
          if (LevelState==LevelState.Initialized && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
          {
               StartCoroutine(Countdown(3));
          }

          if (LevelState==LevelState.Playing)
          {
               hudManager.UpdatePosition(GetPositionData(), characters.Count);
          }
          
     }
     private void OnDisable()
     {
          finishline.OnFinishlinePassed -= FinishlinePassed;
     }
     #endregion


     #region OtherMethods
     int GetPositionData()
     {
          characters.Sort((x, y) => y.GetDistanceTravelled(startPoint, y.transform).CompareTo(x.GetDistanceTravelled(startPoint, x.transform)));
          return characters.IndexOf(player)+1;
     }
     public void SetSceneState(LevelState state)
     {
          //Changes the state
          LevelState = state;
     }
     void SetupScene()
     {
          player = FindObjectOfType<Player>();
          characters.Add(player);

          opponents.AddRange(FindObjectsOfType<Opponent>());
          characters.AddRange(opponents);
          SetSceneState(LevelState.Initialized);
          OnInitalized?.Invoke();
          Debug.Log("Total characters: " + characters.Count);
          Debug.Log("Opponent characters: " + opponents.Count);
     }

     private void FinishlinePassed(AbstractCharacter obj)
     {
          finishedCharacters.Add(obj);
          if (obj is Player)
          {
               SetSceneState(LevelState.Result);
               obj.transform.Translate(obj.transform.forward * 2f);
               OnPlayerFinished?.Invoke();
               StartCoroutine(CameraTransition(() => {
                    SetSceneState(LevelState.Painting);
                    Camera.main.gameObject.SetActive(false);
                    paintStage.SetActive(true);
               }));
          }
     }

     #endregion


     #region Coroutines
     IEnumerator Countdown(int countdownFrom)
     {
          for (int i = countdownFrom; i > 0; i--)
          {
               hudManager.ChangeStatusTextDirect(i.ToString());
               yield return new WaitForSeconds(1f);
          }
          hudManager.ShowStatusText("GO");
          SetSceneState(LevelState.Playing);
          OnFirstTapGiven?.Invoke();
     }
     IEnumerator CameraTransition(Action OnFinished)
     {
          //TODO: Some celebration particles and animations and transition to paint camera
          Camera camera = Camera.main;
          Transform target = player.transform.Find("CameraTarget");
          camera.GetComponent<SmoothFollow>().enabled = false;
          while (Vector3.Distance(camera.transform.position, target.transform.position) > .1f)
          {
               camera.transform.position = Vector3.Lerp(camera.transform.position, target.transform.position, 1f * Time.deltaTime);
               Vector3 targetAngle = new Vector3(
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.x, target.rotation.eulerAngles.x, 1f * Time.deltaTime),
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.y, target.rotation.eulerAngles.y, 1f * Time.deltaTime),
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.z, target.rotation.eulerAngles.z, 1f * Time.deltaTime)
                    );
               camera.transform.eulerAngles = targetAngle;
               yield return null;
          }
          OnFinished?.Invoke();
     } 

     IEnumerator ResultActions(Action OnFinished)
     {
          yield return null;
          OnFinished?.Invoke();
     }
     #endregion
}
