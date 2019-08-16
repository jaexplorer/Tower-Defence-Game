using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ToolBrush : LevelEditorTool
{
    // Tool data.
    private List<TileTopData> _tileTopData = new List<TileTopData>(256);
    private List<TileBottomData> _tileBottomData = new List<TileBottomData>(256);
    private List<TileObjectData> _tileObjectData = new List<TileObjectData>(256);
    private List<TowerData> _towerData = new List<TowerData>(256);
    private TileTopData _currentTileTopData;
    private TileObjectData _currentTileObjectData;
    private TileBottomData _currentTileBottomData;
    private TowerData _currentTowerData;
    private Direction _currentDirection = Direction.Forward;
    private int _currentLevel;
    private bool _applyLevel;

    // private TileTopData.TileType _filterTileType;
    private TileTopData.DataTypeFlag _filterCollection;
    private int _brushSize = 0;
    private bool _elevate;
    private bool _removingObject;
    private bool _elevateKeyDown;

    // UI data.
    private Vector2 _palleteScrollPosition = Vector2.zero;

    // Colors.
    private Color _tileDataBuildableColor = new Color(1f, 0.8f, 0f, 1f);

    // Constant data.
    private int BRUSH_SIZE_MAX = 4;

    override public void OnEnable()
    {
        _name = "Brush";
        Undo.undoRedoPerformed += OnUndo;
        RefreshTileData();
    }

    // public override void OnDisable()
    // {
    //     foreach (RenderTexture rt in _previewTixtures)
    //     {
    //         Object.Destroy(rt);
    //     }
    // }

    private void OnUndo()
    {
        string undoName = Undo.GetCurrentGroupName();
        if (undoName.Contains("BrushDraw") || undoName.Contains("BrushEraze") || undoName.Contains("BrushElevate"))
        {
            _levelEditor.level.ReloadInEditor();
            EditorUtility.SetDirty(_levelEditor.level.data);
        }
    }

    public override void DrawHandles()
    {
        DrawBrush(_brushSize, _brushColor);
    }

    public override void DrawGUI()
    {
        // Setting up the UI.
        float windowHeight = _levelEditor.position.height;

        // Filtering tile palette.
        _filterCollection = (TileTopData.DataTypeFlag)EditorGUI.EnumPopup(new Rect(10, 40, 100, 25), _filterCollection, GUI.skin.button);
        // _filterTileType = (TileTopData.TileType)EditorGUI.EnumPopup(new Rect(110, 40, 100, 25), _filterTileType, GUI.skin.button);
        // GUI.contentColor
        GUI.contentColor = Color.white;
        GUI.FocusControl("");
        // _filteredTileData.Clear();
        // for (int i = 0; i < _tileData.Count; i++)
        // {
        //     // if (_tileData[i].HasFlag(_filterCollection) && _tileData[i].CompareTileType(_filterTileType))
        //     // {
        //     _filteredTileData.Add(_tileData[i]);
        //     // }
        // }
        float paletteHeight = (windowHeight - 140 - 70) / 25;

        // bool scrollViewEnabled = _filteredTileData.Count > paletteHeight;
        // if (scrollViewEnabled)
        // {
        //     _palleteScrollPosition = GUI.BeginScrollView(new Rect(10, 70, 220, paletteHeight * 25), _palleteScrollPosition, new Rect(10, 70, 200, _filteredTileData.Count / 8 * 25));
        // }
        //GUI.backgroundColor = new Color(0.7f, 0.7f, 0.7f);

        int buttonYOffset = 70;
        // TileData buttons.
        for (int y = 0, i = 0; y < paletteHeight && y < _tileTopData.Count / 2f; y++)
        {
            for (int x = 0; x < 2 && i < _tileTopData.Count; x++, i++)
            {
                Color color = _tileTopData[i].color;
                string name = _tileTopData[i].name;//.Substring(5, _tileTopData[i].name.Length - 9);
                GUI.backgroundColor = new Color(color.r * 0.6f, color.g * 0.6f, color.b * 0.6f, 1f);
                // GUI.backgroundColor = _tileTopData[i].color;
                if (_currentTileTopData == _tileTopData[i])
                {
                    GUI.backgroundColor = new Color(color.r * 1.5f, color.g * 1.5f, color.b * 1.5f, 1f); ;
                }
                if (GUI.Button(new Rect(10 + 100 * x, buttonYOffset, 100, 25), name))
                {
                    if (_currentTileTopData != _tileTopData[i])
                    {
                        _currentTileTopData = _tileTopData[i];
                        if (_currentTileTopData.hasSocket == true)
                        {
                            _currentTileObjectData = null;
                        }
                        else
                        {
                            _currentTowerData = null;
                        }
                    }
                    else
                    {
                        _currentTileTopData = null;
                    }
                }
            }
            buttonYOffset += 25;
        }
        // OBottomData buttons.
        buttonYOffset += 10;
        for (int y = 0, i = 0; y < paletteHeight && y < _tileBottomData.Count / 2f; y++)
        {
            for (int x = 0; x < 4 && i < _tileBottomData.Count; x++, i++)
            {
                Color color = _tileBottomData[i].color;
                string name = _tileBottomData[i].name;//.Substring(5, _tileBottomData[i].name.Length - 9);
                GUI.backgroundColor = new Color(color.r * 0.6f, color.g * 0.6f, color.b * 0.6f, 1f);
                // GUI.backgroundColor = _tileBottomData[i].color;
                if (_currentTileBottomData == _tileBottomData[i])
                {
                    GUI.backgroundColor = new Color(color.r * 1.5f, color.g * 1.5f, color.b * 1.5f, 1f); ;
                }
                if (GUI.Button(new Rect(10 + 50 * x, buttonYOffset, 50, 25), name))
                {
                    if (_currentTileBottomData != _tileBottomData[i])
                    {
                        _currentTileBottomData = _tileBottomData[i];
                    }
                    else
                    {
                        _currentTileBottomData = null;
                    }
                }
            }
            buttonYOffset += 25;
        }
        // ObjectData buttons.
        buttonYOffset += 10;
        for (int y = 0, i = 0; y < paletteHeight && y < _tileObjectData.Count / 2f; y++)
        {
            for (int x = 0; x < 2 && i < _tileObjectData.Count; x++, i++)
            {
                Color color = _tileObjectData[i].color;
                string name = _tileObjectData[i].name;//.Substring(5, _tileObjectData[i].name.Length - 9);
                GUI.backgroundColor = new Color(color.r * 0.6f, color.g * 0.6f, color.b * 0.6f, 1f);
                // GUI.backgroundColor = _tileObjectData[i].color;
                if (_currentTileObjectData == _tileObjectData[i])
                {
                    GUI.backgroundColor = new Color(color.r * 1.5f, color.g * 1.5f, color.b * 1.5f, 1f); ;
                }
                if (GUI.Button(new Rect(10 + 100 * x, buttonYOffset, 100, 25), name))
                {
                    if (_currentTileObjectData != _tileObjectData[i])
                    {
                        _currentTileObjectData = _tileObjectData[i];
                        _currentTowerData = null;
                    }
                    else
                    {
                        _currentTileObjectData = null;
                    }
                }
            }
            buttonYOffset += 25;
        }

        // TowerData buttons.
        buttonYOffset += 10;
        for (int y = 0, i = 0; y < paletteHeight && y < _towerData.Count / 2f; y++)
        {
            for (int x = 0; x < 2 && i < _towerData.Count; x++, i++)
            {
                Color color = _towerData[i].color;
                string name = _towerData[i].name.Substring(5, _towerData[i].name.Length - 9);
                GUI.backgroundColor = new Color(color.r * 0.6f, color.g * 0.6f, color.b * 0.6f, 1f);
                // GUI.backgroundColor = _towerData[i].color;
                if (_currentTowerData == _towerData[i])
                {
                    GUI.backgroundColor = new Color(color.r * 1.5f, color.g * 1.5f, color.b * 1.5f, 1f); ;
                }
                if (GUI.Button(new Rect(10 + 100 * x, buttonYOffset, 100, 25), name))
                {
                    if (_currentTowerData != _towerData[i])
                    {
                        _currentTowerData = _towerData[i];
                        _currentTileObjectData = null;
                        if (_currentTileTopData != null && !_currentTileTopData.hasSocket)
                        {
                            _currentTileTopData = null;
                        }
                    }
                    else
                    {
                        _currentTowerData = null;
                    }
                }
            }
            buttonYOffset += 25;
        }
        // Tile level buttons.
        buttonYOffset += 10;
        // for (int y = 0, i = 0; y < paletteHeight && y < _towerData.Count / 2f; y++)
        // {
        for (int x = 0, level = -2; x < 5; x++, level++)
        {
            GUI.backgroundColor = Color.grey;
            if (level == _currentLevel && _applyLevel)
            {
                GUI.backgroundColor = Color.white;
            }
            if (GUI.Button(new Rect(10 + 40 * x, buttonYOffset, 40, 25), level.ToString()))
            {
                if (level == _currentLevel)
                {
                    _applyLevel = !_applyLevel;
                }
                else
                {
                    _applyLevel = true;
                }
                _currentLevel = level;
            }
        }

        GUI.backgroundColor = Color.white;
        // if (scrollViewEnabled)
        // {
        //     GUI.EndScrollView();
        // }
    }

    public override void HandleInput(Event e)
    {
        // Drawing.
        TilePosition mouseMapPosition = _levelEditor.GetMouseTilePosition();
        int tileX = mouseMapPosition.x;
        int tileZ = mouseMapPosition.z;
        int xMin = Mathf.Max(tileX - _brushSize, 0);
        int zMin = Mathf.Max(tileZ - _brushSize, 0);
        int xMax = Mathf.Min(tileX + _brushSize, _levelEditor.level.sizeX - 1);
        int zMax = Mathf.Min(tileZ + _brushSize, _levelEditor.level.sizeZ - 1);

        for (int x = xMin; x <= xMax; x++)
        {
            for (int z = zMin; z <= zMax; z++)
            {
                Tile tile = _levelEditor.level.GetTile(x, z);
                // Left mouse button to draw.
                if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
                {
                    if (tile == null)
                    {
                        Undo.RegisterCompleteObjectUndo(_levelEditor.level, "BrushDraw");
                        _levelEditor.level.CreateTile(x, z, _currentTileTopData, _currentTileBottomData, _currentTileObjectData);
                        EditorUtility.SetDirty(_levelEditor.level.data);
                    }
                    else
                    {
                        if (_currentTileTopData != null)
                        {
                            tile.serializedTile.topData = _currentTileTopData;
                            if (!_currentTileTopData.hasSocket)
                            {
                                if (tile.serializedTile.towerData != null)
                                {
                                    tile.serializedTile.towerData = null;
                                }
                            }
                            else if (tile.serializedTile.objectData != null)
                            {
                                tile.serializedTile.objectData = null;
                            }
                        }
                        if (_currentTileObjectData && (tile.serializedTile.topData || !tile.serializedTile.topData.hasSocket))
                        {
                            tile.serializedTile.objectData = _currentTileObjectData;
                        }
                        if (_currentTileBottomData && tile.serializedTile.topData)
                        {
                            tile.serializedTile.bottomData = _currentTileBottomData;
                        }
                        if (_currentTowerData && tile.serializedTile.topData)
                        {
                            tile.serializedTile.elevated = true;
                            tile.serializedTile.towerData = _currentTowerData;
                        }
                        if (_applyLevel)
                        {
                            tile.serializedTile.level = _currentLevel;
                        }

                        Undo.RegisterCompleteObjectUndo(_levelEditor.level, "BrushDraw");
                        _levelEditor.level.GetTile(x, z).ReloadInEditor();
                        EditorUtility.SetDirty(_levelEditor.level.data);
                    }
                }
                if (tile != null)
                {
                    // Right mouse button to eraze.
                    if (e.button == 1)
                    {
                        if (e.type == EventType.MouseDown && (tile.serializedTile.towerData != null || tile.serializedTile.objectData != null))
                        {
                            Undo.RegisterCompleteObjectUndo(_levelEditor.level, "BrushEraze");
                            _removingObject = true;
                            tile.serializedTile.towerData = null;
                            tile.serializedTile.objectData = null;
                            tile.ReloadInEditor();
                            EditorUtility.SetDirty(_levelEditor.level.data);
                        }
                        if (!_removingObject && e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
                        {
                            Undo.RegisterCompleteObjectUndo(_levelEditor.level, "BrushEraze");
                            tile.serializedTile.bottomData = null;
                            tile.serializedTile.topData = null;
                            tile.serializedTile.objectData = null;
                            tile.serializedTile.towerData = null;
                            tile.serializedTile.elevated = false;
                            _levelEditor.level.RemoveTile(x, z);
                            EditorUtility.SetDirty(_levelEditor.level.data);
                        }
                        else if (e.type == EventType.MouseUp)
                        {
                            _removingObject = false;
                        }
                    }
                    // Middle mouse to elevate.
                    else if (e.type == EventType.MouseDown && e.button == 2 && _elevateKeyDown != true && tile.serializedTile.towerData == null)
                    {
                        _elevateKeyDown = true;
                        if (tile.serializedTile.topData != null)
                        {
                            if (tile.serializedTile.elevated)
                            {
                                _elevate = false;
                            }
                            else
                            {
                                _elevate = true;
                            }
                        }
                    }
                    else if (e.type == EventType.MouseUp && e.button == 2)
                    {
                        _elevateKeyDown = false;
                    }
                    if (_elevateKeyDown && tile.serializedTile.elevated != _elevate)
                    {
                        Undo.RegisterCompleteObjectUndo(_levelEditor.level, "BrushElevate");
                        tile.serializedTile.elevated = _elevate;
                        _levelEditor.level.GetTile(x, z).ReloadInEditor();
                        EditorUtility.SetDirty(_levelEditor.level.data);
                    }
                    //Z & X to rotate.
                    if (e.type == EventType.KeyDown && (e.keyCode == KeyCode.Z || e.keyCode == KeyCode.X))
                    {
                        int directionInt = (int)_currentDirection;
                        if (e.keyCode == KeyCode.Z)
                        {
                            directionInt--;
                            if (directionInt < 1)
                            {
                                directionInt = 4;
                            }
                        }
                        if (e.keyCode == KeyCode.X)
                        {
                            directionInt++;
                            if (directionInt > 4)
                            {
                                directionInt = 1;
                            }
                        }
                        _currentDirection = (Direction)directionInt;
                        Undo.RegisterCompleteObjectUndo(_levelEditor.level, "BrushElevate");
                        tile.serializedTile.direction = _currentDirection;
                        _levelEditor.level.GetTile(x, z).ReloadInEditor();
                        EditorUtility.SetDirty(_levelEditor.level.data);
                        e.Use();
                    }
                }
            }
        }

        // Scroll wheel.
        if (e.type == EventType.ScrollWheel)
        {
            if (e.alt)
            {
                // Brush size.
                if (e.delta.y < 0f && _brushSize < BRUSH_SIZE_MAX)
                {
                    _brushSize++;
                }
                else if (e.delta.y > 0f && _brushSize > 0)
                {
                    _brushSize--;
                }
            }
        }


    }

    private void PickRandomBottomObject()
    {

    }

    private void RefreshTileData()
    {
        _tileTopData.Clear();
        // string[] tileTopDataGUIDs = AssetDatabase.FindAssets("t:TileTopData", new string[] { "Assets/Data" });
        // string[] tileTopDataGUIDs = AssetDatabase.FindAssets("t:ScriptableObject", new string[] { "Assets/Data/TileTops" });
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new string[] { "Assets/Data" });
        string[] tileObjectDataGUIDs = AssetDatabase.FindAssets("t:TileBottomData", new string[] { "Assets/Data" });
        string[] tileBottomDataGUIDs = AssetDatabase.FindAssets("t:TileObjectData", new string[] { "Assets/Data" });
        string[] towerDataGUIDs = AssetDatabase.FindAssets("t:TowerData", new string[] { "Assets/Data" });
        foreach (string guid in guids)
        {

            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid));
            if (so is TileTopData)
            {
                TileTopData data = (TileTopData)so;
                if (data != null)
                {
                    _tileTopData.Add(data);
                }
            }
            else if (so is TileBottomData)
            {
                TileBottomData data = (TileBottomData)so;
                if (data != null)
                {
                    _tileBottomData.Add(data);
                }
            }
            else if (so is TileObjectData)
            {
                TileObjectData data = (TileObjectData)so;
                if (data != null)
                {
                    _tileObjectData.Add(data);
                }
            }
            else if (so is TowerData)
            {
                TowerData data = (TowerData)so;
                if (data != null)
                {
                    _towerData.Add(data);
                }
            }
        }
        // foreach (string guid in guids)
        // {
        //     TileTopData data = AssetDatabase.LoadAssetAtPath<TileTopData>(AssetDatabase.GUIDToAssetPath(guid));
        //     if (data != null)
        //     {
        //         _tileTopData.Add(data);
        //     }
        // }
        // foreach (string guid in tileBottomDataGUIDs)
        // {
        //     TileBottomData data = AssetDatabase.LoadAssetAtPath<TileBottomData>(AssetDatabase.GUIDToAssetPath(guid));
        //     if (data != null)
        //     {
        //         _tileBottomData.Add(data);
        //     }
        // }
        // foreach (string guid in tileObjectDataGUIDs)
        // {
        //     TileObjectData data = AssetDatabase.LoadAssetAtPath<TileObjectData>(AssetDatabase.GUIDToAssetPath(guid));
        //     if (data != null)
        //     {
        //         _tileObjectData.Add(data);
        //     }
        // }
        // foreach (string guid in tileObjectDataGUIDs)
        // {
        //     TowerData data = AssetDatabase.LoadAssetAtPath<TowerData>(AssetDatabase.GUIDToAssetPath(guid));
        //     if (data != null)
        //     {
        //         _towerData.Add(data);
        //     }
        // }
    }

    public override void OnLostFocus()
    {
        _elevateKeyDown = false;
    }
}

