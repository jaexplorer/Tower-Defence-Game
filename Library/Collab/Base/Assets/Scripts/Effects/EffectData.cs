using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Data/EffectData", order = 1)]
public class EffectData : ScriptableObject
{
    [SerializeField] private float _lifetime; // lifetime
    [SerializeField] private float _interval; // frames between ticks
    [SerializeField] private float _amount; // effect amout
    [SerializeField] private float _stacksMax; // zero for no stacking?
    [SerializeField] private GameObject _prefab;
    [SerializeField] private bool _ongoing; // if true amount will be applied per tick
    [SerializeField] private bool _amountPerLevel; // if true amount will be applied per tick

    public GameObject prefab { get { return _prefab; } }
    public float interval { get { return _interval; } }
    public float lifetime { get { return _lifetime; } }
    public bool ongoing { get { return _ongoing; } }
}


