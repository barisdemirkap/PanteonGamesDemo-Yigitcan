using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
     public GameState GameState { get { return gameState; } }
     private GameState gameState;

     public List<AbstractCharacter> Characters { get { return characters; } }
     private List<AbstractCharacter> characters = new List<AbstractCharacter>();

     public Player player;
     public List<Opponent> opponents = new List<Opponent>();

     private void Update()
     {
          if (gameState!=GameState.Playing)
          {
               if (Input.GetMouseButtonDown(0)||Input.touchCount>0)
               {
                    SetGameState(GameState.Playing);
               }
          }
     }
     public void SetGameState(GameState state)
     {
          //Changes the state
          gameState = state;

     }
}
