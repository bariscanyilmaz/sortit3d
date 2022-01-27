using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { PlayerTurn, Wait, Win, Lose }
public class GameManager : Singleton<GameManager>
{

    State _state;
    Level _currentLevel;

    [SerializeField] int _levelIndex;
    [SerializeField] Level[] _levels;

    public State State => _state;

    public void SetState(State state) => _state = state;

    void Start()
    {
        SetState(State.Wait);
        LoadLevel(_levelIndex);
        SetState(State.PlayerTurn);
    }

    void LoadLevel(int levelIndex)
    {
        _currentLevel = Instantiate<Level>(_levels[levelIndex % _levels.Length]);
    }

    public bool CheckWin()
    {
        return _currentLevel.CheckWin();
    }

    public void LoadNextLevel()
    {
        SetState(State.Wait);
        Destroy(_currentLevel.gameObject);
        _levelIndex++;
        LoadLevel(_levelIndex);
        SetState(State.PlayerTurn);
    }

}
