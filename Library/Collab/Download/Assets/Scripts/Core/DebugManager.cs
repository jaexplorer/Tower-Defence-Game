using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private bool _testLevelMode;
    [SerializeField] private bool _startWave;
    [SerializeField] private LevelData _lastEditedLevelData;

    private long _checksum;

    //PROPERTIES///////////////////////////////////////////////
    public static DebugManager instance
    {
        get;
        private set;
    }

    public bool levelTestMode
    {
        get { return _testLevelMode; }
    }

    public LevelData lastEditedLevelData
    {
        get { return _lastEditedLevelData; }
        set { _lastEditedLevelData = value; }
    }

    public bool startWave
    {
        get { return _startWave; }
    }

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        instance = this;
    }

    static public void ContributeToChecksum(int value)
    {
        instance._checksum += value;
    }

    static public void ResetChecksum()
    {
        instance._checksum = 0;
    }

    static public long GetChecksum()
    {
        return instance._checksum;
    }
}