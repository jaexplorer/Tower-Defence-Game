using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _defeatmenu;
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _healthBars;

    private GameObject _currentMenuObject;
    private float _gameTimeScale;
    private bool _inPauseMenu;
    private bool _onGameScreen;
    private bool _gameInputEnabled;
    // private bool _inDefeatMenu;

    public bool inPauseMenu
    {
        get { return _inPauseMenu; }
    }

    public bool onGameScreen
    {
        get { return _onGameScreen; }
    }

    public static bool gameInputEnabled
    {
        get { return instance._gameInputEnabled; }
    }

    public static UIManager instance
    {
        get;
        private set;
    }

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        instance = this;
        DeactivateAll();
    }

    private void Update()
    {
        _gameInputEnabled = !EventSystem.current.IsPointerOverGameObject() && _onGameScreen;
    }

    //PUBLIC///////////////////////////////////////////////////
    public void GoToMainMenu()
    {

    }

    public void GoToGameScreen()
    {
        Unpause();
        ActivateMenu(_hud);
        _onGameScreen = true;
    }

    public void GoToPauseMenu()
    {
        Pause();
        ActivateMenu(_pauseMenu);
    }

    public void GoToDefeatScreen()
    {
        ActivateMenu(_defeatmenu);
    }

    private void DeactivateCurrentMenu()
    {
        if (_currentMenuObject)
        {
            _currentMenuObject.SetActive(false);
        }
    }

    //PRIVATE//////////////////////////////////////////////////
    private void ActivateMenu(GameObject menu)
    {
        DeactivateCurrentMenu();
        _currentMenuObject = menu;
        _onGameScreen = false;
        menu.SetActive(true);
    }

    private void Pause()
    {
        _gameTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        _inPauseMenu = true;
    }

    private void Unpause()
    {
        if (_inPauseMenu)
        {
            Time.timeScale = _gameTimeScale;
            _inPauseMenu = false;
        }
    }

    private void DeactivateAll()
    {
        _mainMenu.SetActive(false);
        _hud.SetActive(false);
        _pauseMenu.SetActive(false);
        _defeatmenu.SetActive(false);
        // _lossScreen.SetActive(false);
        // _victoryScreen.SetActive(false);
        // _settingsMenu.SetActive(false);
        // _healthBars.SetActive(false);
    }
}
