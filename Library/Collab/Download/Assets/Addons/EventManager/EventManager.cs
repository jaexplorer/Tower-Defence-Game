using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _onLevelReset;
    [SerializeField] private UnityEvent _update;
    [SerializeField] private UnityEvent _onTileChange;

    private static EventManager _instance;

    //PROPERTIES///////////////////////////////////////////////
    // public static EnemyEvent onEnemyDeath
    // {
    //     get { return _instance._onEnemyDeath; }
    // }

    // public static EnemyEvent onEnemySpawn
    // {
    //     get { return _instance._onEnemySpawn; }
    // }

    // public static EnemyEvent onEnemyReachPortal
    // {
    //     get { return _instance._onEnemyReachPortal; }
    // }

    public static UnityEvent update
    {
        get { return _instance._update; }
    }

    public static UnityEvent onLevelClear
    {
        get { return _instance._onLevelReset; }
    }

    public static UnityEvent onLevelChange
    {
        get { return _instance._onTileChange; }
    }

    //EVENTS///////////////////////////////////////////////////
    public void Awake()
    {
        _instance = this;
    }
}

[System.Serializable]
public class EnemyEvent : UnityEvent<Enemy> { }