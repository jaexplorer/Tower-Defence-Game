using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// builds towers/ghost, keeps track of prices and resources
public class Builder : MonoBehaviour
{
    // [SerializeField] private Pathfinder _pathfinder;
    [SerializeField] private Pointer _pointer;
    [SerializeField] private Hud _hud;
    [SerializeField] private TowerMenu _towerMenu;
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private GameObject _ghostObject;
    [SerializeField] private MeshFilter _ghostMeshFilter;
    [SerializeField] private TowerData[] _allTowerData;

    private List<TowerData> _availableTowerData = new List<TowerData>(16);
    private List<int> _prices = new List<int>(16);
    private List<GameObject> _markers = new List<GameObject>(32);
    private TowerData _currentTowerData;
    private Tower _currentTower;
    private Tile _currentTile;
    private PathState _currentPathfindingResult;
    private int _energy;
    private int _energyRecorded;
    private int _currentTowerIndex;
    private List<Tower> _towers = new List<Tower>(64);
    private List<Tile> _markedTiles = new List<Tile>(16);
    private bool _allowBuilding;

    //PROPERTIES///////////////////////////////////////////////
    // public TowerData[] allTowerData { get { return _allTowerData; } }
    // public int energy { get { return _energy; } set { _energy = value; } }
    // public static Builder instance { get; private set; }

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        Events.onEnemyDeath.AddListener(OnEnemyDeath);
        Events.onGameStateChange.AddListener(OnGameStateChange);
        Events.onLevelLoad.AddListener(OnLevelLoad);
        Events.onWaveSave.AddListener(OnWaveSave);
        Events.onWaveLoad.AddListener(OnWaveLoad);
    }

    private void Update()
    {
        // Keyboard input.
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                SelectTower(i + 1);
            }
        }
        // Mouse input.
        if (UIManager.gameInputEnabled)
        {
            if (_pointer.mouseTile != _currentTile)
            {
                _currentTile = _pointer.mouseTile;
                // _currentPathfindingResult = _pathfinder.TestPath(_currentTile); //TODO: Enable.
                UpdateGhost();
            }
            if (Input.GetMouseButtonDown(1))
            {
                DeselectTower();
            }
            if (_currentTile != null)
            {
                if (_currentTowerData != null)
                {
                    // Building.
                    if (Input.GetMouseButtonDown(0) && _currentTile.hasSocket && _currentTile.tower == null)
                    {
                        if (_currentTile.elevated)
                        {
                            BuildTower();
                        }
                        else if (!GameManager.inWaveMode)
                        {
                            _currentTile.ToggleElevation();
                            // _pathfinder.FindPath();
                            Events.onTileChange.Invoke();
                        }
                    }
                }
                else if (!GameManager.inWaveMode)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        // Elevating.
                        if (_currentTile.elevatable && _currentTile.tower == null)
                        {
                            _currentTile.ToggleElevation();
                            // _pathfinder.FindPath();
                            Events.onTileChange.Invoke();
                        }
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        // Destroying.
                        if (_currentTile.tower != null)
                        {
                            DestroyTower(_currentTile.tower);
                        }
                    }
                }
            }
        }
    }

    public void OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.Building)
        {
            enabled = true;
            _allowBuilding = true;
        }
        else if (gameState == GameState.Wave)
        {
            enabled = true;
            DeselectTower();
            _allowBuilding = false;
        }
        else
        {
            DeselectTower();
            enabled = false;
        }
    }

    public void OnLevelLoad()
    {
        DeselectTower();
        _energy = Level.instance.startingEnergy;
        _hud.SetEnergy(_energy);
        _availableTowerData.Clear();
        _availableTowerData.AddRange(_allTowerData);
        _towerMenu.GenerateButtons(_availableTowerData);
        ResetPrices();
    }

    public void OnWaveLoad()
    {
        _energy = ProfileManager.instance.saveData.energy;
        _hud.SetEnergy(_energy);
    }

    public void OnWaveSave()
    {
        ProfileManager.instance.saveData.energy = _energy;
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        _energy += enemy.data.bounty;
        _hud.SetEnergy(_energy);
    }

    //PUBLIC///////////////////////////////////////////////////
    public void SelectTower(int index)
    {
        if (_currentTowerData == null || _currentTowerData != _availableTowerData[index])
        {
            _currentTowerData = _availableTowerData[index];
            _ghostMeshFilter.sharedMesh = _currentTowerData.mesh;
            _currentTower = _currentTowerData.towerPrefab.GetComponent<Tower>();
            UpdateGhost();
        }
        _currentTowerIndex = index;
    }

    public void DeselectTower()
    {
        if (_currentTowerData != null)
        {
            _ghostObject.SetActive(false);
            _currentTowerData = null;
            UpdateGhost();
        }
    }

    //PRIVATE//////////////////////////////////////////////////
    public void ResetPrices()
    {
        _towers.Clear();
        Level.instance.GetAllTowers(_towers);
        _prices.Clear();
        for (int i = 0; i < _availableTowerData.Count; i++)
        {
            _prices.Add(100);
        }
        for (int i = 0; i < _towers.Count; i++)
        {
            int index = _availableTowerData.IndexOf(_towers[i].data);
            _prices[index] += 10;
        }
        for (int i = 0; i < _availableTowerData.Count; i++)
        {
            _towerMenu.SetPrice(i, _prices[i]);
        }
    }

    private void BuildTower()
    {
        if (_energy >= _prices[_currentTowerIndex])
        {
            Tower newTower = PoolManager.Produce(_currentTowerData.towerPrefab, null, _currentTile.transform.position).GetComponent<Tower>();
            _currentTile.tower = newTower;
            _energy -= _prices[_currentTowerIndex];
            _prices[_currentTowerIndex] += 10;
            _towerMenu.SetPrice(_currentTowerIndex, _prices[_currentTowerIndex]);
            _hud.SetEnergy(_energy);
            _ghostObject.SetActive(false);
        }
    }

    private void DestroyTower(Tower tower)
    {
        _currentTile.tower = null;
        int index = _availableTowerData.IndexOf(tower.data);
        _prices[index] -= 10;
        _energy += _prices[index];
        _towerMenu.SetPrice(index, _prices[index]);
        _hud.SetEnergy(_energy);
        tower.Recycle();
    }

    private void UpdateGhost()
    {
        for (int i = 0; i < _markers.Count; i++)
        {
            PoolManager.Recycle(_markers[i]);
        }
        _markers.Clear();
        _markedTiles.Clear();
        // Debug.Log("U");
        if (_currentTowerData != null)
        {
            // Debug.Log("td");
            _ghostObject.SetActive(false);
        }
        if (_currentTowerData != null && _currentTile != null && _currentTile.hasSocket && _currentTile.tower == null && UIManager.gameInputEnabled)
        {
            // Debug.Log("en");
            _currentTower.GetTiles(_currentTile.position, null, _markedTiles);
            for (int i = 0; i < _markedTiles.Count; i++)
            {
                _markers.Add(_markerPrefab.GetCopy(null, _markedTiles[i].transform.position));
            }
            _ghostObject.SetActive(true);
            _ghostObject.transform.position = _currentTile.transform.position;
            _ghostObject.transform.parent = _currentTile.transform;
        }
    }
}