using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To apply it, pass effect data scriptable object to enemy.effects.Add(data)
// Towers might need effects too, something to think about. Can we unify them?

public class Effect : ManagedObject
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleEffect _particleEffect;
    [SerializeField] private Enemy _enemy;
    private EffectData _data;
    private int _stacks;
    private int _level;
    private WaitForSeconds _interval;
    private WaitForSeconds _lifetime;

    //PUBLIC///////////////////////////////////////////////////
    public virtual void Initiate(EffectData data, int level)
    {
        _data = data; // Assigning data to support multiple levels of efect, or can we just use stacks as levels?
        // _enemy = enemy;
        _enemy.onDeath.AddListener(Recycle); // Removing this effect on enemy death
                                             // If effect is viual, assign enemy as parent

        if (_data.ongoing)
        {
            _interval = new WaitForSeconds(data.interval);
            StartCoroutine(ApplyCoroutine());
        }
        if (_data.temporary)
        {
            StartCoroutine(LifetimeCoroutine());
        }
    }

    //PROTECTED////////////////////////////////////////////////
    protected virtual void AddStacks(int amount)
    {
        // Stacking behaviour. Override if necessary
        // Default just add to stacks, or have options in data for stacking behaviour, for example reset timrt, add stacks, 
    }

    protected virtual void Refresh(int amount)
    {
        //or delete and add a new one instead
    }

    protected virtual void Apply()
    {
        // Actual effect. Override
        // Example: _enemy.slow += _data.amount
        // Or _enemy.effects.slow +=
        // _enemy.effects.armorModifier +=
        // _enemy.Damage()
    }

    protected override void OnRecycle()
    {
        StopAllCoroutines();
        // Remove effects from the enemy. Ovveride
    }

    protected IEnumerator ApplyCoroutine()
    {
        Apply();
        yield return _interval;
    }

    protected IEnumerator LifetimeCoroutine()
    {
        yield return _lifetime;
        Recycle();
    }
}

