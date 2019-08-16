using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    // [SerializeField] private LevelList _levelList;
    [SerializeField] private GameObject _buttonOriginal;
    [SerializeField] private Transform _buttonContainer;

    private void Start()
    {
        // for (int i = 0; i < _levelList.levelData.Count; i++)
        // {
        //     LevelData level = _levelList.levelData[i];
        //     LevelButton button = (Instantiate(_buttonOriginal, _buttonContainer) as GameObject).GetComponent<LevelButton>();
        //     button.Set(new Vector2(0f, -30 * i), level.name, i, this);
        // }
        // Destroy(_buttonOriginal);
    }

    public void OnLevelButtonClick(int index)
    {
        _gameManager.LoadLevel(_levelList.levelData[index]);
    }
}
