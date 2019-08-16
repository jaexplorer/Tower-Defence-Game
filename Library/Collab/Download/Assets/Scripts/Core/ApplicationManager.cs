using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Level _level;
    [SerializeField] private GameManager _game;
    [SerializeField] private DebugManager _debugManager;
    // [SerializeField] private TowerMenu _towerMenu;
    // [SerializeField] private Builder _builder;

    //EVENTS///////////////////////////////////////////////////
    private void Start()
    {
        if (Application.isEditor && _debugManager.levelTestMode)
        {
            LoadLevel(_debugManager.lastEditedLevelData);
        }
        else
        {
            LoadLevel(_debugManager.lastEditedLevelData);
        }
    }

    //PUBLIC///////////////////////////////////////////////////
    public void LoadLevel(LevelData levelData)
    {
        _level.Load(levelData);
        //_uiManager.GoToGameScreen();
        _game.OnLevelLoaded();
        //_builder.OnLevelLoaded(_levelManager.currentLevel);
        // _towerMenu.OnLevelLoaded();
        if (Application.isEditor && _debugManager.levelTestMode && _debugManager.startWave)
        {
            _game.StartWave();
        }
    }

    public void LoadLevel(int index)
    {
        _levelManager.LoadLevel(index);
    }

    public void GoToLevelSelectionMenu()
    {
        //_uiManager.GoToLevelMenu();
    }

    //PRIVATE//////////////////////////////////////////////////
    private void OnApplicationFocus()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}