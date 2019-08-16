using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Data/EffectData", order = 1)]
public class EffectData : ScriptableObject
{
    [SerializeField] private int _lifetime; // lifetime
    [SerializeField] private int _interval; // frames between ticks
    [SerializeField] private int _amount; // effect amout
    [SerializeField] private int _stacksMax; // zero for no stacking?
    [SerializeField] private GameObject _prefab;
    [SerializeField] private bool _ongoing; // if true amount will be applied per tick

    public GameObject prefab { get { return _prefab; } }
}


