using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Builder : CustomBehaviour
{
    [SerializeField] private Pointer _pointer;
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private GameObject _ghostObject;
    [SerializeField] private MeshFilter _ghostMeshFilter;
    [SerializeField] private TowerData[] _allTowerData;
    [SerializeField] private TowerMenu _towerMenu;

    private List<TowerData> _availableTowerData = new List<TowerData>(16);
    private List<Tower> _towers = new List<Tower>(64);
    private List<Tile> _markedTiles = new List<Tile>(16);
    private List<int> _prices = new List<int>(16);
    private List<GameObject> _markers = new List<GameObject>(32);
    private TowerData _currentTowerData;
    private Tower _currentTower;
    private Tile _currentTile;
    private PathState _currentPathState;
    private int _energy;
    private int _energyRecorded;
    private int _currentTowerIndex;
    private bool _buildingAllowed;

    //EVENTS///////////////////////////////////////////////////
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
        if (!InputManager.pointerOverUI)
        {
            if (_pointer.mouseTile != _currentTile)
            {
                _currentTile = _pointer.mouseTile;
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
                        else if (_buildingAllowed)
                        {
                            _currentTile.ToggleElevation();
                            EventManager.onTilesChange.Invoke();
                        }
                    }
                }
                else if (_buildingAllowed)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        // Elevating.
                        if (_currentTile.elevatable && _currentTile.tower == null)
                        {
                            _currentTile.ToggleElevation();
                            EventManager.onTilesChange.Invoke();
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

    protected override void OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.Building)
        {
            enabled = true;
            _buildingAllowed = true;
        }
        else if (gameState == GameState.Wave)
        {
            enabled = true;
            DeselectTower();
            _buildingAllowed = false;
        }
        else
        {
            DeselectTower();
            enabled = false;
        }
    }

    protected override void OnLevelLoad()
    {
        DeselectTower();
        _energy = Level.instance.startingEnergy;
        _availableTowerData.Clear();
        _availableTowerData.AddRange(_allTowerData);
        _towerMenu.GenerateButtons(_availableTowerData);
        ResetPrices();
    }

    protected override void OnWaveLoad()
    {
        _energy = ProfileManager.instance.saveData.energy;
    }

    protected override void OnWaveSave()
    {
        ProfileManager.instance.saveData.energy = _energy;
    }

    protected override void OnEnemyDeath(Enemy enemy)
    {
        _energy += enemy.data.bounty;
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
    private void ResetPrices()
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
        if (_currentTowerData != null)
        {
            _ghostObject.SetActive(false);
        }
        if (_currentTowerData != null && _currentTile != null && _currentTile.hasSocket && _currentTile.tower == null && !InputManager.pointerOverUI)
        {
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