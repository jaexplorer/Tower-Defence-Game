using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelData> _levelDataList;
    [SerializeField] private Level _level;

    //PROPERTIES///////////////////////////////////////////////
    // public static LevelManager instance
    // {
    //     get;
    //     private set;
    // }

    public List<LevelData> levelDataList
    {
        get { return _levelDataList; }
        set { _levelDataList = value; }
    }

    //EVENTS///////////////////////////////////////////////////
    // private void Awake()
    // {
    //     instance = this;
    // }

    //PUBLIC///////////////////////////////////////////////////
    public void LoadLevel(int index)
    {
        _level.Load(_levelDataList[index]);
    }
}

