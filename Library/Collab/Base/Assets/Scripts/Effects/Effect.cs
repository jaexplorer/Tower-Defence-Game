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
    private int _lifeTimer; // Lifetime countdown
    private int _tickTimer; // Countdown for next tick
    private int _stacks;
    private int _level;
    private WaitForSeconds _interval;
    private WaitForSeconds _kifetime;

    //PUBLIC///////////////////////////////////////////////////
    public virtual void Initiate(EffectData data, int level)
    {
        if (_data.ongoing)
        {
            _interval = new WaitForSeconds(data.interval);
        }
        _data = data; // Assigning data to support multiple levels of efect, or can we just use stacks as levels?
        // _enemy = enemy;
        _enemy.onDeath.AddListener(Recycle); // Removing this effect on enemy death
        // If effect is viual, assign enemy as parent
        // Reset timers
        // Call Apply()
    }

    //EVENTS///////////////////////////////////////////////////
    protected void Update()
    {
        // Update timers
        // Not going to be called if not set to use Update in Inspector 
        // Apply effect if it is ongoing. Call Apply()
        // This method can be the same for all effects. Don't override
    }

    //PROTECTED////////////////////////////////////////////////
    protected virtual void AddStacks(int amount)
    {
        // Stacking behaviour. Override if necessary
        // Default just add to stacks, or have options in data for stacking behaviour, for example reset timrt, add stacks, 
    }

    protected virtual void Apply()
    {
        // Actual effect. Override
        // Example: _enemy.slow += _data.amount
        // Or _enemy.effects.slow +=
        // _enemy.effects.armorModifier +=
        // _enemy.Damage()
    }

    protected virtual void Remove()
    {
        // Remove effects from the enemy. Ovveride
    }
}

