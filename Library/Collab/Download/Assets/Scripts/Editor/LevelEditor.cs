using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// Editor window for the map, and level settings editing.
public class LevelEditor : EditorWindow
{
    // Objects.
    private Level _level;
    private LevelManager _levelManager;
    private DebugManager _debugManager;
    private Camera _camera;
    private Transform _cameraTransform;
    private Transform _editorObjectsTransform;

    // Editor.
    private bool _initiated;
    private bool _showGrid = true;
    private bool _hideUI;
    private bool _skipUpdate;
    private bool _showLevelList;
    // private int _levelIndex;
    private TilePosition _mouseMapPosition;
    private Tile _mouseTile;
    private List<LevelEditorTool> _tools = new List<LevelEditorTool>(4);
    private LevelEditorTool _currentTool;
    private LevelEditorData _persistentData;

    // Camera.
    private float _cameraMoveSpeed = 0.5f;
    private float _cameraZoomSpeed = 1.4f;
    private float _cameraSizeMax = 100f;
    private float _cameraSizeMin = 2f;
    private bool _moveCameraForward;
    private bool _moveCameraBackward;
    private bool _moveCameraLeft;
    private bool _moveCameraRight;

    // Colors.
    private Color _UIhighlightColor = new Color(1f, 0.8f, 0f);
    private Color _gridColor = new Color(0.2f, 0.2f, 0.2f);

    // Levels
    private List<LevelData> _levelDataList;

    // Paths
    private string levelDataPath = "Assets/Data/Levels";
    private string levelEditorDataPath = "Assets/Data/Editor";

    private CompoundMesh _mesh;

    public static LevelEditor instance
    {
        get;
        private set;
    }

    public Level level
    {
        get { return _level; }
    }

    [MenuItem("Window/Level Editor")]
    static void Init()
    {
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        window.Show();
    }

    private void OnEnable()
    {
        Reset();
        // Debug.Log("reset");
    }

    private void OnDisable()
    {
        if (_editorObjectsTransform != null)
        {
            DestroyImmediate(_editorObjectsTransform.gameObject);
        }
        if (IsActive())
        {
            _currentTool.OnDisable();
        }
    }

    private void OnGUI()
    {
        if (IsActive())
        {
            // Performing full update.
            UpdateMousePosition();
            UpdateMouseTile();
            RenderMap();
            DrawHandles();
            DrawGUI();
            if (Event.current.type != EventType.Layout && Event.current.type != EventType.Repaint)
            {
                HandleInput();
            }
        }
        else
        {
            GUI.Label(new Rect(10, 10, 800, 25), "Level editor failed to initiate.");
        }
    }

    private void Update()
    {
        if (IsActive())
        {
            if (!_skipUpdate && focusedWindow == this)
            {
                // Moving camera.
                if (_moveCameraForward) _cameraTransform.Translate(0f, 0f, _cameraMoveSpeed);
                if (_moveCameraBackward) _cameraTransform.Translate(0f, 0f, -_cameraMoveSpeed);
                if (_moveCameraLeft) _cameraTransform.Translate(-_cameraMoveSpeed, 0f, 0f);
                if (_moveCameraRight) _cameraTransform.Translate(_cameraMoveSpeed, 0f, 0f);

                // Keeping camera in bounds.
                Vector3 cameraPosition = _cameraTransform.position;
                float camX = cameraPosition.x;
                float camZ = cameraPosition.z;
                if (camX < 0) camX = 0;
                if (camZ < 0) camZ = 0;
                if (camX > _level.sizeX) camX = _level.sizeX;
                if (camZ > _level.sizeZ) camZ = _level.sizeZ;
                _cameraTransform.position = new Vector3(camX, 2f, camZ);
                _persistentData.cameraPosition = _cameraTransform.position;

                // Forcing window to repaint.
                Repaint();
            }
            _skipUpdate = !_skipUpdate;
        }
    }

    private void OnFocus()
    {
        Reset();
        if (IsActive())
        {
            // Forwarding the event to the tool.
            _currentTool.OnFocus();
        }
    }

    private void OnLostFocus()
    {
        if (IsActive())
        {
            // Stopping the camera movement.
            _moveCameraForward = false;
            _moveCameraBackward = false;
            _moveCameraLeft = false;
            _moveCameraRight = false;

            // Forwarding the event to the tool.
            _currentTool.OnLostFocus();
        }
    }

    //LEVEL////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void LoadLevel(LevelData levelData)
    {
        _level.LoadInEditor(levelData);
        _persistentData.lastActiveLevel = levelData;
        _debugManager.lastEditedLevelData = levelData;
        // Finding index of current level.
        // for (int i = 0; i < _levelDataList.Count; i++)
        // {
        //     if (_level == _levelDataList[i])
        //     {
        //         _levelIndex = i;
        //     }
        // }
    }

    private void Reset()
    {
        if (focusedWindow == this)
        {
            instance = this;

            // Searching for objects.
            _level = GameObject.FindObjectOfType<Level>();
            _levelManager = GameObject.FindObjectOfType<LevelManager>();
            _debugManager = GameObject.FindObjectOfType<DebugManager>();
            _persistentData = AssetDatabase.LoadAssetAtPath<LevelEditorData>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:LevelEditorData", new string[] { "Assets/Data/Editor" })[0]));

            // Updating Levels.
            _levelDataList = _levelManager.levelDataList;

            // Selecting last active level or a first from the list.
            if (_persistentData != null)
            {
                if (_levelDataList.Count > 0)
                {
                    if (_persistentData.lastActiveLevel == null)
                    {
                        _persistentData.lastActiveLevel = _levelDataList[0];
                    }
                    LoadLevel(_persistentData.lastActiveLevel);
                }
                else
                {
                    Debug.LogError("LevelData not found!");
                }
            }
            else
            {
                Debug.LogError("EditorData not found!");
            }

            // Creating object container.
            GameObject editorObjectsContainer = GameObject.Find(Names.levelEditorObjects) as GameObject;
            if (editorObjectsContainer != null)
            {
                DestroyImmediate(editorObjectsContainer);
            }
            _editorObjectsTransform = new GameObject(Names.levelEditorObjects).transform;
            _editorObjectsTransform.hideFlags = HideFlags.DontSaveInBuild;// | HideFlags.HideInHierarchy;

            // Creating camera.
            GameObject cameraGameObject = new GameObject("EditorCamera");
            cameraGameObject.layer = LayerMask.NameToLayer("Editor");
            _camera = cameraGameObject.AddComponent<Camera>();
            _camera.orthographic = true;
            _camera.nearClipPlane = -10;
            _camera.farClipPlane = 15;
            _camera.clearFlags = CameraClearFlags.Color;
            _camera.targetTexture = new RenderTexture((int)position.width, (int)position.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            // _camera.targetTexture.antiAliasing = 8;
            _camera.backgroundColor = new Color32(55, 144, 155, 255);
            _cameraTransform = _camera.transform;
            _cameraTransform.SetParent(_editorObjectsTransform);
            _cameraTransform.rotation = Quaternion.Euler(45f, 45f, 0f);
            _cameraTransform.position = _persistentData.cameraPosition;
            _camera.orthographicSize = _persistentData.cameraOrthographicSize;

            // Creating compound mesh.
            // CompoundMesh mesh = editorObjectsContainer.AddComponent<CompoundMesh>();

            // Initializing tools.
            LevelEditorTool.Initiate(this);
            _tools.Clear();
            _tools.Add(new ToolBrush());
            _tools.Add(new ToolSize());
            _tools.Add(new ToolSelect());
            _tools.Add(new ToolSpawn());
            _currentTool = _tools[0];
            foreach (LevelEditorTool tool in _tools)
            {
                tool.OnEnable();
            }

        }
    }

    private void UpdateLevelList()
    {
        // _levelList.Clear();
        // // _levelList.AddRange(_levelManager.GetComponentsInChildren<Level>());
        // // var levelDataPaths = AssetDatabase.FindAssets("t:LevelData");
        // // AssetDatabase.GetLevel
        // // _levelList.AddRange();
        // // Filtering out disabled levels.
        // for (int i = 0; i < _levelList.Count; i++)
        // {
        //     if (_levelList[i].gameObject.active == false)
        //     {
        //         _levelList.RemoveAt(i);
        //     }
        // }

        // _levelManager.serializedLevels = _levelList.ToArray();
    }

    //CUSTOM METHODS///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private bool IsActive()
    {
        if (_level != null && _editorObjectsTransform != null && _camera != null && _currentTool != null && !Application.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RenderMap()
    {
        // Resizing camera texture if window size was changed.
        if (_camera.targetTexture == null || (int)position.width != (int)_camera.targetTexture.width || (int)position.height != (int)_camera.targetTexture.height)
        {
            _camera.targetTexture = new RenderTexture((int)position.width, (int)position.height, 24);
        }

        //Rendering camera and displaying the texture.
        _camera.Render();
        GUI.DrawTexture(new Rect(0, 0, position.width, position.height), _camera.targetTexture, ScaleMode.ScaleAndCrop);

        //Handles.DrawCamera(new Rect(0, 0, 1, 1), _camera);//Buggy still (no depth).
    }

    private void DrawHandles()
    {
        // Drawing handles in screen space.
        Handles.BeginGUI();

        // Drawing grid.
        Handles.color = _gridColor;
        if (_showGrid)
        {
            for (int x = 1; x < _level.sizeX; x++)
            {
                Vector3 p1 = WorldToScreenPoint(new Vector3(x - 0.5f, 0f, -0.5f));
                Vector3 p2 = WorldToScreenPoint(new Vector3(x - 0.5f, 0f, _level.sizeZ - 0.5f));
                Handles.DrawLine(p1, p2);
            }
            for (int y = 1; y < _level.sizeZ; y++)
            {
                Vector3 p1 = WorldToScreenPoint(new Vector3(-0.5f, 0f, y - 0.5f));
                Vector3 p2 = WorldToScreenPoint(new Vector3(_level.sizeX - 0.5f, 0f, y - 0.5f));
                Handles.DrawLine(p1, p2);
            }

            // Drawing grid border.
            Vector3 v1 = WorldToScreenPoint(new Vector3(-0.5f, 0f, -0.5f));
            Vector3 v2 = WorldToScreenPoint(new Vector3(-0.5f, 0f, _level.sizeZ - 0.5f));
            Vector3 v3 = WorldToScreenPoint(new Vector3(_level.sizeX - 0.5f, 0f, _level.sizeZ - 0.5f));
            Vector3 v4 = WorldToScreenPoint(new Vector3(_level.sizeX - 0.5f, 0f, -0.5f));
            Handles.DrawPolyLine(new Vector3[5] { v1, v2, v3, v4, v1 });
            Handles.color = Color.white;
        }

        // Drawing tool's handles.
        _currentTool.DrawHandles();

        Handles.EndGUI();
    }

    private void DrawGUI()
    {
        if (!_hideUI)
        {
            // Tool buttons.
            for (int i = 0; i < _tools.Count; i++)
            {
                if (_currentTool == _tools[i])
                {
                    GUI.backgroundColor = _UIhighlightColor;
                }
                if (GUI.Button(new Rect(10 + 50 * i, 10, 50, 25), _tools[i].GetName()))
                {
                    _currentTool = _tools[i];
                    _tools[i].OnActivate();
                }
                GUI.backgroundColor = Color.white;
            }

            // Level button.
            //GUI.Label(new Rect(position.width - 210, 10, 200, 25), _level.levelName);
            if (GUI.Button(new Rect(position.width - 210, 10, 200, 25), _level.name))
            {
                if (!_showLevelList)
                {
                    _showLevelList = true;
                }
                else
                {
                    _showLevelList = false;
                }
            }

            // Level list buttons.
            if (_showLevelList)
            {
                for (int i = 0; i < _levelDataList.Count; i++)
                {
                    if (_levelDataList[i] == _level)
                    {
                        GUI.backgroundColor = _UIhighlightColor;
                    }
                    if (GUI.Button(new Rect(position.width - 210, 20 + 25 * (i + 1), 200, 25), _levelDataList[i].name))
                    {
                        LoadLevel(_levelDataList[i]);
                    }
                    GUI.backgroundColor = Color.white;
                }
            }

            // Drawing UI of the current tool.
            _currentTool.DrawGUI();
        }
    }

    private void HandleInput()
    {
        Event e = Event.current;

        // Camera movement.
        if (e.keyCode == KeyCode.W)
        {
            if (e.type == EventType.KeyDown)
            {
                _moveCameraForward = true;
            }
            else
            {
                _moveCameraForward = false;
            }
        }
        if (e.keyCode == KeyCode.S)
        {
            if (e.type == EventType.KeyDown)
            {
                _moveCameraBackward = true;
            }
            else
            {
                _moveCameraBackward = false;
            }
        }
        if (e.keyCode == KeyCode.A)
        {
            if (e.type == EventType.KeyDown)
            {
                _moveCameraLeft = true;
            }
            else
            {
                _moveCameraLeft = false;
            }
        }
        if (e.keyCode == KeyCode.D)
        {
            if (e.type == EventType.KeyDown)
            {
                _moveCameraRight = true;
            }
            else
            {
                _moveCameraRight = false;
            }
        }

        // Handle scroll wheel input.
        if (e.type == EventType.ScrollWheel)
        {
            if (!e.alt && !e.control)
            {
                // Camera zoom.
                if (e.delta.y > 0f && _camera.orthographicSize < _cameraSizeMax)
                {
                    _camera.orthographicSize *= _cameraZoomSpeed;
                }
                else if (e.delta.y < 0f && _camera.orthographicSize > _cameraSizeMin)
                {
                    _camera.orthographicSize /= _cameraZoomSpeed;
                }
                _persistentData.cameraOrthographicSize = _camera.orthographicSize;
            }
        }

        // Toggle grid.
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.G)
        {
            _showGrid = !_showGrid;
        }

        // Toggle UI.
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab)
        {
            _hideUI = !_hideUI;
        }

        // Reset the editor.
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
        {
            Reset();
        }

        // Allow tools to recieve input.
        _currentTool.HandleInput(e);

        // Disabling Unity's standart input, except chosen key strokes.
        // if (!(e.keyCode == KeyCode.Space && e.shift) && (e.type == EventType.KeyDown || e.type == EventType.KeyUp))
        // {
        // 	e.Use();
        // }
        // else
        // {
        // 	OnEnable();
        // }
    }

    private void UpdateMousePosition()
    {
        Vector3 v = Event.current.mousePosition;
        Plane raycastPlane = new Plane(Vector3.up, 0f);
        Ray ray = _camera.ScreenPointToRay(new Vector2(v.x, position.height - v.y));
        float distance;
        raycastPlane.Raycast(ray, out distance);
        _mouseMapPosition = new TilePosition(ray.GetPoint(distance));
    }

    private void UpdateMouseTile()
    {
        _mouseTile = _level.GetTile(_mouseMapPosition);
    }

    public Vector2 WorldToScreenPoint(Vector3 worldPoint)
    {
        Vector2 v = _camera.WorldToScreenPoint(worldPoint);
        return new Vector2(v.x, position.height - v.y);
    }

    //GET//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public TilePosition GetMouseTilePosition()
    {
        return _mouseMapPosition;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public bool GetHideUI()
    {
        return _hideUI;
    }

    public Tile GetMouseTile()
    {
        return _mouseTile;
    }
}