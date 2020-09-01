using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateKeeper
{

    private static GameStateKeeper instance = null;

    public enum GameMode
    {
        Timer,
        Coins
    }

    public enum GameState
    {
        Welcome,
        Scanning,
        Playing,
        Ended
    }

    GameMode gameMode = GameMode.Timer;

    GameState gameState = GameState.Welcome;
    private GameStateKeeper() { }

    public static GameStateKeeper getInstance()
    {
        if (instance == null)
        {
            instance = new GameStateKeeper();
        }

        return instance;
    }

    public void setGameMode(GameMode mode)
    {
        gameMode = mode;
    }

    public GameMode getGameMode()
    {
        return gameMode;
    }

    public void setGameState(GameState state)
    {
        gameState = state;
    }

    public GameState getGameState()
    {
        return gameState;
    }


}
