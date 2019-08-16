using UnityEngine;
using System.Collections.Generic;

public class GameManager : CustomBehaviour
{
    [SerializeField] private DebugManager _debugManager;
    [SerializeField] private Level _level;
    // [SerializeField] private LevelList _levelList;

    private GameState _gameState;
    private bool _paused;

    //EVENTS///////////////////////////////////////////////////
    private void Start()
    {
        // if (Application.isEditor && _debugManager.inLevelTestMode)
        // {
        //     LoadLevel(_debugManager.lastEditedScene);
        // }
        // else
        // {
        //     LoadLevel(_levelList.levelData[0]); //TODO: Load properly
        // }
    }

    private void Update()
    {
        if (_gameState == GameState.Wave)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_paused) Play();
                else Pause();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                Time.timeScale = 100f;
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                Play();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                FastForward();
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                Play();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartWave();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && (Time.timeScale == 1 || Time.timeScale == 0))
        {
            // Pause
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_level.spawner.waveIndex > 0 || _gameState == GameState.Wave)
            {
                LoadWave();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            LoadWave();
            StartWave();
        }
    }

    protected override void OnWaveLost()
    {
        Pause();
    }

    protected override void OnGameStateChange(GameState gameState)
    {
        _gameState = gameState;
    }

    //PUBLIC///////////////////////////////////////////////////
    public void LoadLevel()
    {
        // _level.Load(levelData);
        EventManager.onGameStateChange.Invoke(GameState.Building);
        EventManager.onLevelLoad.Invoke();
        EventManager.onTilesChange.Invoke();
        Play();
    }

    public void StartWave()
    {
        EventManager.onGameStateChange.Invoke(GameState.Wave);
        EventManager.onWaveStart.Invoke();
        SaveWave();
        Play();
    }

    public void LoadWave()
    {
        EventManager.onWaveLoad.Invoke();
        EventManager.onLevelClear.Invoke();
        Play();
    }

    public void CompleteWave()
    {
        EventManager.onGameStateChange.Invoke(GameState.Building);
        EventManager.onWaveComplete.Invoke();
        Play();
    }

    public void SaveWave()
    {
        EventManager.onWaveSave.Invoke();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        _paused = true;
    }

    public void Play()
    {
        Time.timeScale = 1f;
        _paused = false;
    }

    public void FastForward()
    {
        Time.timeScale = 4f;
        _paused = false;
    }
}