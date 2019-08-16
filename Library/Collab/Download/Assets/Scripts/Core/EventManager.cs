using UnityEngine;
using UnityEngine.Events;

public class Events : MonoBehaviour
{
    private static GameStateEvent _onGameStateChange = new GameStateEvent();
    private static UnityEvent _onLevelReset = new UnityEvent();
    private static UnityEvent _onTileChange = new UnityEvent();
    private static UnityEvent _onLevelLoad = new UnityEvent();
    private static UnityEvent _onWaveSave = new UnityEvent();
    private static UnityEvent _onWaveLoad = new UnityEvent();
    private static EnemyEvent _onEnemyDeath = new EnemyEvent();
    // private static IntEvent _onPortalPointsChange = new IntEvent();

    //PROPERTIES///////////////////////////////////////////////
    public static GameStateEvent onGameStateChange { get { return _onGameStateChange; } }
    public static UnityEvent onLevelLoad { get { return _onLevelLoad; } }
    public static UnityEvent onLevelClear { get { return _onLevelReset; } }
    public static UnityEvent onTileChange { get { return _onTileChange; } }
    public static UnityEvent onWaveSave { get { return _onWaveSave; } }
    public static UnityEvent onWaveLoad { get { return _onWaveLoad; } }
    public static EnemyEvent onEnemyDeath { get { return _onEnemyDeath; } }
    // public static IntEvent onEnergyChange { get { return _onEnergyChange; } }
    // public static IntEvent onPortalPointsChange { get { return _onEnergyChange; } }
}

// [System.Serializable]
public class EnemyEvent : UnityEvent<Enemy> { }
public class GameStateEvent : UnityEvent<GameState> { }
public class IntEvent : UnityEvent<GameState> { }

// [System.Serializable]