using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DebugManager _debugManager;
    [SerializeField] private UpdateManager _updateManager;
    [SerializeField] private Pathfinder _pathfinder;
    [SerializeField] private Builder _builder;
    [SerializeField] private LevelList _levelList;
    [SerializeField] private Level _level;
    [SerializeField] private Hud _hud;
    [SerializeField] private UIManager _uiManager;

    private bool _inWaveMode;
    private bool _paused;

    //PROPERTIES///////////////////////////////////////////////
    public static GameManager instance
    {
        get;
        private set;
    }

    public static bool inWaveMode
    {
        get { return instance._inWaveMode; }
    }

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (Application.isEditor && _debugManager.inLevelTestMode)
        {
            LoadLevel(_debugManager.lastEditedLevelData);
        }
        else
        {
            LoadLevel(_levelList.levelData[0]); //TEMP
        }
    }

    private void Update()
    {
        if (_inWaveMode)
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
            if (!_uiManager.inPauseMenu)
            {
                _uiManager.GoToPauseMenu();
            }
            else
            {
                _uiManager.GoToGameScreen();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_level.spawner.waveIndex > 0 || _inWaveMode)
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

    //PUBLIC///////////////////////////////////////////////////
    public void LoadLevel(LevelData levelData)
    {
        _level.Load(levelData);
        _inWaveMode = false;
        Events.onGameStateChange.Invoke(GameState.Building);
        Events.onLevelLoad.Invoke();

        // _builder.OnLevelLoaded();
        _pathfinder.OnLevelLoaded();
        _uiManager.GoToGameScreen();
        _hud.OnLevelLoaded();
        DebugManager.ResetChecksum();
        Events.onTileChange.Invoke();
        _pathfinder.FindPath();
        Play();
    }

    public void StartWave()
    {
        if (_pathfinder.pathState == PathState.Clear)
        {
            _inWaveMode = true;
            _hud.GoToWaveMode();
            // Debug.Log("hud");
            _level.OnWaveStart();
            // Debug.Log("start");
            // _builder.OnWaveStarted();
            // Debug.Log("wave");
            SaveWave();
            Play();
        }
    }

    public void LoadWave()
    {
        Events.onWaveLoad.Invoke();
        Events.onLevelClear.Invoke();

        _inWaveMode = false;
        _uiManager.GoToGameScreen();
        _level.OnWaveLoad();
        // _builder.OnWaveLoad();
        _hud.GoToBuildMode();
        Play();
    }

    public void CompleteWave()
    {
        _inWaveMode = false;
        _hud.GoToBuildMode();
        // _level.OnWaveCompleted();
        Debug.Log("Wave checksum: " + DebugManager.GetChecksum());
        DebugManager.ResetChecksum();
        Play();
    }

    public void SaveWave()
    {
        Events.onWaveSave.Invoke();

        _level.OnWaveSave();
        // _builder.OnWaveSave();
    }

    public void OnPortalDepleted()
    {
        Pause();
        _uiManager.GoToDefeatScreen();
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

