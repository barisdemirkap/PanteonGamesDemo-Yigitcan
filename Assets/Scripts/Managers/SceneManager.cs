using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
     public GameState GameState { get { return gameState; } }

     private GameState gameState;


     private void Start()
     {
          SetGameState(GameState.Initialized);
     }
     void SetGameState(GameState newState)
     {
          gameState = newState;
     }
}
