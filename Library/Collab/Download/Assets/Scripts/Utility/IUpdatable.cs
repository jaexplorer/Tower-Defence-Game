using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable
{
    // bool active
    // {
    //     get;
    //     set;
    // }
    void ManagedUpdate();
    // void OnSceneClear();
}

public interface IResettable
{
    // bool active
    // {
    //     get;
    //     set;
    // }
    void Reset();
    // void OnSceneClear();
}