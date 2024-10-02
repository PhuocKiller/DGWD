using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    None,
    Lobby,
    Transition,
    Ingame,

}
public class GameManager : NetworkBehaviour
{
    [Networked(OnChanged =nameof(CurrentStateChanged))]
    private int currentState { get; set; }
    public GameState TypeOfGameState (int value)
    {
        switch(value)
        {
            case 0: return GameState.None; case 1: return GameState.Lobby; case 2: return GameState.Transition;
            case 3:return GameState.Ingame;
            default: return GameState.None;
        }
    }
    // public GameState state => currentState; //chỉ get chứ ko set, lấy giá trị currentState
    public GameState state
    {
        get 
        { if (Object.IsValid)
            {
                return TypeOfGameState(currentState);
            }
            else
            {
                return GameState.None;
            }
        }
        
    }
    private Action<GameState, GameState> oncurrentStateChanged;
    protected static void CurrentStateChanged(Changed<GameManager> changed)
    {
        changed.LoadOld();
        GameState oldState = changed.Behaviour.TypeOfGameState(changed.Behaviour.currentState); ;
        changed.LoadNew();
        GameState newState = changed.Behaviour.TypeOfGameState(changed.Behaviour.currentState);;
        changed.Behaviour.oncurrentStateChanged.Invoke(oldState,newState);
    }
    public override void Spawned()
    {
        base.Spawned();
        currentState = (int)(GameState.Lobby);

    }
    public void SwitchState(GameState state)
    {
        currentState = (int)state;
    }
    public void RegisterOnGameStateChanged(Action<GameState, GameState> listener)
    {
        oncurrentStateChanged += listener;
    }
    public void UnRegisterOnGameStateChanged(Action<GameState, GameState> listener)
    {
        oncurrentStateChanged -= listener;
    }

}
