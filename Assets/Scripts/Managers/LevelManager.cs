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
     [SerializeField]
     private Transform characterPaintingPosition;

     [Header("Results Text")]
     [Space]
     [TextArea(5, 10)]
     [SerializeField]
     private string victoryText = "Perfectly Splendid!! \n You Win the Race";
     [TextArea(5, 10)]
     [SerializeField]
     private string defeatText = "Better Luck Next Time";

     public LevelState LevelState { get; private set; }

     //player starting position
     public Vector3 startPoint = Vector3.zero;

     //all active character 
     private List<AbstractCharacter> characters = new List<AbstractCharacter>();

     //Finished characters
     private List<AbstractCharacter> finishedCharacters = new List<AbstractCharacter>();
     #endregion

     #region Events
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
          if (characterPaintingPosition == null)
               characterPaintingPosition = paintStage.transform.Find("PlayerPosition");
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
          if (LevelState == LevelState.Playing)
          {
               //sort character lists
               characters.Sort((x, y) => y.GetDistanceTravelled(startPoint, y.transform).CompareTo(x.GetDistanceTravelled(startPoint, x.transform)));
          }
     }

     private void OnDisable()
     {
          finishline.OnFinishlinePassed -= FinishlinePassed;
     }
     #endregion


     #region Script Methods
     /// <summary>
     /// Returns position in the race
     /// </summary>
     /// <param name="character">Position requesting character</param>
     /// <returns></returns>
     public int GetPositionData(AbstractCharacter character)
     {
          return characters.IndexOf(character) + 1;
     }

     public void SetLevelState(LevelState state)
     {
          LevelState = state;
     }
     /// <summary>
     /// Scene first enter setup
     /// </summary>
     void SetupScene()
     {
          if (player == null)
               player = FindObjectOfType<Player>();
          characters.Add(player);
          if (opponents.Count == 0)
               opponents.AddRange(FindObjectsOfType<Opponent>());

          characters.AddRange(opponents);
          SetLevelState(LevelState.Initialized);
          Debug.Log("Total characters: " + characters.Count);
          Debug.Log("Opponent characters: " + opponents.Count);
     }

     /// <summary>
     /// Finisline passed action from finishline script
     /// </summary>
     /// <param name="obj">Character crossed the finish line</param>
     private void FinishlinePassed(AbstractCharacter obj)
     {
          finishedCharacters.Add(obj);
          if (obj is Player)
          {
               SetLevelState(LevelState.Result);
               //Send player finished signal
               OnPlayerFinished?.Invoke(finishedCharacters.IndexOf(player));

               //coroutine chain for camera transition and results
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
          player.transform.position = characterPaintingPosition.position;
          hudManager.ShowPaintingProgress();
     }
     #endregion
     #region Coroutines
     /// <summary>
     /// Counts down from given number of seconds
     /// </summary>
     /// <param name="countdownFrom">Countdown start number</param>
     /// <returns></returns>
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
     /// <summary>
     /// Camera transition for results state
     /// </summary>
     /// <param name="OnFinished">Callback function</param>
     /// <returns></returns>
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
     /// <summary>
     /// Results state coroutine for celebration or etc.
     /// </summary>
     /// <param name="OnFinished">Callback action</param>
     /// <returns></returns>
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
