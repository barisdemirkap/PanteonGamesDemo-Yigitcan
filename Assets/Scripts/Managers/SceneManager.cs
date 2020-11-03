using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
     public GameState GameState { get; private set; }

     public List<AbstractCharacter> Characters { get { return characters; } }

     private List<AbstractCharacter> characters = new List<AbstractCharacter>();

     private List<AbstractCharacter> finishedPlayers = new List<AbstractCharacter>();


     public Player player;
     public List<Opponent> opponents = new List<Opponent>();

     private Finishline finishline;

     #region Engine Method
     protected override void Awake()
     {
          base.Awake();
          finishline = FindObjectOfType<Finishline>();
          SetupScene();
          SetGameState(GameState.Initialized);
     }

     private void Update()
     {
          if (GameState != GameState.Playing)
          {
               if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
               {
                    SetGameState(GameState.Playing);
               }
          }
     }
     private void OnDisable()
     {
          finishline.OnFinishlinePassed -= FinishlinePassed;
     } 
     #endregion

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
          
          Debug.Log("Total characters: "+characters.Count);
          Debug.Log("Opponent characters: "+opponents.Count);
     }

     private void FinishlinePassed(AbstractCharacter obj)
     {
          finishedPlayers.Add(obj);
     }
}
