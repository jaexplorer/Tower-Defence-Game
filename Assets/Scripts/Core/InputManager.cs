using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// Provides input and manages key settings.
public class InputManager : MonoBehaviour
{
    [SerializeField] private List<Action> _serializedActions;
    private static List<Action> _actions = new List<Action>(32);
    private static bool _pointerOverUI;

    //PROPERTIES///////////////////////////////////////////////
    public static bool pointerOverUI { get { return _pointerOverUI; } }
    public static Action up { get { return _actions[(int)ActionCode.Up]; } }
    public static Action down { get { return _actions[(int)ActionCode.Down]; } }
    public static Action left { get { return _actions[(int)ActionCode.Left]; } }
    public static Action right { get { return _actions[(int)ActionCode.Right]; } }
    public static Action submit { get { return _actions[(int)ActionCode.Submit]; } }
    public static Action cancel { get { return _actions[(int)ActionCode.Cancel]; } }

    //EVENTS///////////////////////////////////////////////////
    public void Awake()
    {
        // Check if any action codes are out of range.
        if (_serializedActions.Count < System.Enum.GetNames(typeof(ActionCode)).Length)
        {
            Debug.Log("Not all action codes are implemented");
        }
        // Initialize action list.
        var hashSet = new HashSet<ActionCode>();
        _actions.AddRange(_serializedActions);
        foreach (IInernalAction a in _serializedActions)
        {
            if (hashSet.Contains(a.code))
            {
                Debug.LogError("Duplicate ActionCode found: " + a.code + " " + a.name);
            }
            _actions[(int)a.code] = (Action)a;
            hashSet.Add(a.code);
        }
    }

    //TODO: Gotta make sure that it runs before update in any script that is using this input.
    public void Update()
    {
        // Update actions.
        foreach (IInernalAction a in _actions)
        {
            a.started = Input.GetKeyDown(a.key1) || Input.GetKeyDown(a.key2) || Input.GetKeyDown(a.button);
            a.active = Input.GetKey(a.key1) || Input.GetKey(a.key2) || Input.GetKey(a.button);
            a.ended = Input.GetKeyUp(a.key1) || Input.GetKeyUp(a.key2) || Input.GetKeyUp(a.button);
        }

        // Check if mouse is over UI.
        _pointerOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    //TYPES////////////////////////////////////////////////////
    [System.Serializable]
    public class Action : IInernalAction
    {
        [SerializeField] private string _name = "Action";
        [SerializeField] private ActionCode _code;
        [SerializeField] private KeyCode _key1;
        [SerializeField] private KeyCode _key2;
        [SerializeField] private KeyCode _button;
        private bool _started;
        private bool _active;
        private bool _ended;

        public bool started { get { return _started; } }
        public bool ended { get { return _ended; } }

        string IInernalAction.name { get { return _name; } }
        ActionCode IInernalAction.code { get { return _code; } }
        bool IInernalAction.started { set { _started = value; } }
        bool IInernalAction.active { set { _active = value; } }
        bool IInernalAction.ended { set { _ended = value; } }
        KeyCode IInernalAction.key1 { get { return _key1; } set { _key1 = value; } }
        KeyCode IInernalAction.key2 { get { return _key2; } set { _key2 = value; } }
        KeyCode IInernalAction.button { get { return _button; } set { _button = value; } }

        public static implicit operator bool(Action a)
        {
            return a._active;
        }
    }

    private interface IInernalAction
    {
        string name { get; }
        ActionCode code { get; }
        bool started { set; }
        bool active { set; }
        bool ended { set; }
        KeyCode key1 { get; set; }
        KeyCode key2 { get; set; }
        KeyCode button { get; set; }
    }

    public enum ActionCode
    {
        Up,
        Down,
        Left,
        Right,
        Submit,
        Cancel,
    }
}