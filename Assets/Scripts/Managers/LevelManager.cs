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
public enum LevelState { Initialized, Countdown, Playing, Paused, Result, Painting, Ended }
public class LevelManager : Singleton<LevelManager>
{
     #region Fields
     [Header("Mandatory Scripts")]
     [Space]

     [SerializeField]
     private Finishline finishline;
     [SerializeField]
     private HUDManager hudManager;

     [Header("Participants")]
     [Space]
     [SerializeField]
     public Player player;
     [SerializeField]
     public List<Opponent> opponents = new List<Opponent>();

     [Header("Painting Stage")]
     [Space]
     [SerializeField]
     private GameObject paintStage;
     [SerializeField]
     
     private float cameraTransitionSpeed = 5f;

     [Header("Results Text")]
     [Space]
     [TextArea(5,10)]
     [SerializeField]
     private string victoryText = "Perfectly Splendid!! \n You Win the Race";
     [TextArea(5, 10)]
     [SerializeField]
     private string defeatText = "Better Luck Next Time";

     public LevelState LevelState { get; private set; }

     public Vector3 startPoint = Vector3.zero;
     //all active character 
     private List<AbstractCharacter> characters = new List<AbstractCharacter>();

     //Finished characters
     private List<AbstractCharacter> finishedCharacters = new List<AbstractCharacter>();
     #endregion

     #region Events
     public Action OnInitalized;
     public Action OnFirstTapGiven;
     public Action<int> OnPlayerFinished;
     #endregion


     #region Engine Methods
     protected override void Awake()
     {
          base.Awake();
          if (finishline == null)
               finishline = FindObjectOfType<Finishline>();
          if (hudManager == null)
               hudManager = FindObjectOfType<HUDManager>();
          SetupScene();
          //subscribe to finishline passed action
          finishline.OnFinishlinePassed += FinishlinePassed;

     }
     private void Update()
     {
          if (LevelState == LevelState.Initialized && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
          {
               StartCoroutine(Countdown(3));
          }
     }
     private void OnDisable()
     {
          finishline.OnFinishlinePassed -= FinishlinePassed;
     }
     #endregion


     #region Script Methods
     public int GetPositionData(AbstractCharacter character)
     {
          characters.Sort((x, y) => y.GetDistanceTravelled(startPoint, y.transform).CompareTo(x.GetDistanceTravelled(startPoint, x.transform)));
          return characters.IndexOf(character) + 1;
     }
     public void SetLevelState(LevelState state)
     {
          //Changes the state
          LevelState = state;
     }
     void SetupScene()
     {
          if (player == null)
               player = FindObjectOfType<Player>();
          characters.Add(player);
          if (opponents.Count == 0)
               opponents.AddRange(FindObjectsOfType<Opponent>());

          characters.AddRange(opponents);
          SetLevelState(LevelState.Initialized);
          OnInitalized?.Invoke();
          Debug.Log("Total characters: " + characters.Count);
          Debug.Log("Opponent characters: " + opponents.Count);
     }

     private void FinishlinePassed(AbstractCharacter obj)
     {
          finishedCharacters.Add(obj);
          if (obj is Player)
          {
               SetLevelState(LevelState.Result);
               OnPlayerFinished?.Invoke(finishedCharacters.IndexOf(player));
               StartCoroutine(CameraTransition(() =>
               {
                    StartCoroutine(ResultActions(() =>
                    {
                         TransitionToPaintingStage();
                    }));
               }));
          }
     }

     void TransitionToPaintingStage()
     {
          SetLevelState(LevelState.Painting);
          Camera.main.gameObject.SetActive(false);
          paintStage.SetActive(true);
          hudManager.ShowPaintingProgress();
     }

     #endregion


     #region Coroutines
     IEnumerator Countdown(int countdownFrom)
     {
          SetLevelState(LevelState.Countdown);
          for (int i = countdownFrom; i > 0; i--)
          {
               hudManager.ChangeStatusTextDirect(i.ToString());
               yield return new WaitForSeconds(1f);
          }
          hudManager.ShowStatusText("GO");
          SetLevelState(LevelState.Playing);
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
               camera.transform.position = Vector3.Lerp(camera.transform.position, target.transform.position, cameraTransitionSpeed * Time.deltaTime);
               Vector3 targetAngle = new Vector3(
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.x, target.rotation.eulerAngles.x, cameraTransitionSpeed * Time.deltaTime),
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.y, target.rotation.eulerAngles.y, cameraTransitionSpeed * Time.deltaTime),
                    Mathf.LerpAngle(camera.transform.rotation.eulerAngles.z, target.rotation.eulerAngles.z, cameraTransitionSpeed * Time.deltaTime)
                    );
               camera.transform.eulerAngles = targetAngle;
               yield return null;
          }
          OnFinished?.Invoke();
     }

     IEnumerator ResultActions(Action OnFinished)
     {
          if (finishedCharacters[0] == player)
               hudManager.ShowStatusText(victoryText, 3f);
          else
               hudManager.ShowStatusText(defeatText, 3f);

          yield return new WaitForSeconds(3f);
          OnFinished?.Invoke();
     }
     #endregion
}
