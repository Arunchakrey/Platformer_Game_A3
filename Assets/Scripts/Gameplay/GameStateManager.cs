using System;
using UnityEngine;

public enum GameState { Playing, Paused, GameOver }

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager I { get; private set; }
    public GameState State { get; private set; } = GameState.Playing;
    public event Action<GameState> OnChanged;

    void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
        Time.timeScale = 1f;
    }

    public void Set(GameState s)
    {
        if (State == s) return;
        State = s;
        Time.timeScale = (s == GameState.GameOver || s == GameState.Paused) ? 0f : 1f;
        OnChanged?.Invoke(State);
        Debug.Log($"[GSM] State -> {State}");
    }
}