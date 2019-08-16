using UnityEngine;

public class DebugManager : CustomBehaviour
{
    [SerializeField] private string _lastEditedScene;
    [SerializeField] private bool _inLevelTestMode;
    private long _checksum;

    //PROPERTIES///////////////////////////////////////////////
    private static DebugManager instance { get; set; }

    public string lastEditedScene { get { return _lastEditedScene; } set { _lastEditedScene = value; } }
    public bool inLevelTestMode { get { return _inLevelTestMode; } set { _inLevelTestMode = value; } }

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        instance = this;
    }

    protected override void OnWaveComplete()
    {
        Debug.Log("Wave checksum: " + _checksum);
    }

    //PUBLIC///////////////////////////////////////////////////
    static public void ContributeToChecksum(int value)
    {
        instance._checksum += value;
    }
}