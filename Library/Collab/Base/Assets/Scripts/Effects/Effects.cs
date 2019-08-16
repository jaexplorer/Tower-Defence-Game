using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private List<Effect> _effects = new List<Effect>(); // needs getter
    private int slow; // getter, setter, if used
    private int armorModifier;
    private int damageModifier;

    public void Add(EffectData effectData)
    {
        // Check stacking, find same effects and increase it's stacks. 
        // If efect is not present, add it
        Effect effect = effectData.prefab.GetCopy(_enemy.transform).GetComponent<Effect>();
        // effect.Initiate(effectData);
        // Add to list
    }

    public void Remove(Effect effect)
    {
        // Called by effect probably 
        // Just remove from the list
    }
}