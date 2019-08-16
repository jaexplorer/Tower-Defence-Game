using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public List<Effect> effects = new List<Effect>(); // Should be private
    List<EffectCode> effectTypes = new List<EffectCode>();
    EnemyStats stats;

    public Effects(Enemy enemy)
    {
        stats = enemy.Stats;
    }

    public void AddEffect(Effect effect)
    {
        if (effectTypes.Contains(effect.Code)) // Checks if it is an effect
        {
            foreach (Effect e in effects)
            {
                if (e.Code != effect.Code) // Checks whether we have it or not yet
                {
                    effects.Add(effect);
                    return;
                }
                else
                {
                    // NOT IMPLIMENTED YET // INCREASE EXPIRY TIME
                }
            }
        }
        else { Debug.LogError("Bug Found: Unknown effect attempted to be added"); }
    }

    public void RemoveEffect(Effect effect)
    {
        foreach (Effect e in effects)
        {
            if (e.Code == effect.Code)
            {
                effects.Remove(effect);
                return;
            }
            else { Debug.LogError("Bug Found: Unknown effect attempted to be removed"); }
        }
    }

    private void Update()
    {
        foreach (Effect e in effects)
        {
            Apply(e);
        }
    }

    private void Apply(Effect e)
    {
        e.EntryEffect();
        {
            switch (e.Code)
            {
                case EffectCode.Bleed:
                    {
                        break;
                    }
                case EffectCode.Burn:
                    {
                        break;
                    }
                case EffectCode.Freeze:
                    {
                        break;
                    }
                case EffectCode.Heal:
                    {
                        break;
                    }
                case EffectCode.Might:
                    {
                        break;
                    }
                case EffectCode.Slow:
                    {
                        break;
                    }
                case EffectCode.Poison:
                    {
                        break;
                    }
                case EffectCode.Stun:
                    {
                        break;
                    }
            }
        }
        e.TickEffect();
        {
            switch (e.Code)
            {
                case EffectCode.Bleed:
                    {
                        break;
                    }
                case EffectCode.Burn:
                    {
                        break;
                    }
                case EffectCode.Freeze:
                    {
                        break;
                    }
                case EffectCode.Heal:
                    {
                        break;
                    }
                case EffectCode.Might:
                    {
                        break;
                    }
                case EffectCode.Slow:
                    {
                        break;
                    }
                case EffectCode.Poison:
                    {
                        break;
                    }
                case EffectCode.Stun:
                    {
                        break;
                    }
            }
        }
        e.PersistantEffect();
        {
            switch (e.Code)
            {
                case EffectCode.Bleed:
                    {
                        break;
                    }
                case EffectCode.Burn:
                    {
                        break;
                    }
                case EffectCode.Freeze:
                    {
                        break;
                    }
                case EffectCode.Heal:
                    {
                        break;
                    }
                case EffectCode.Might:
                    {
                        break;
                    }
                case EffectCode.Slow:
                    {
                        break;
                    }
                case EffectCode.Poison:
                    {
                        break;
                    }
                case EffectCode.Stun:
                    {
                        break;
                    }
            }
        }
        e.LeaveEffect();
        {
            switch (e.Code)
            {
                case EffectCode.Bleed:
                    {
                        break;
                    }
                case EffectCode.Burn:
                    {
                        break;
                    }
                case EffectCode.Freeze:
                    {
                        break;
                    }
                case EffectCode.Heal:
                    {
                        break;
                    }
                case EffectCode.Might:
                    {
                        break;
                    }
                case EffectCode.Slow:
                    {
                        break;
                    }
                case EffectCode.Poison:
                    {
                        break;
                    }
                case EffectCode.Stun:
                    {
                        break;
                    }
            }
        }
    }

    public class Effect : InternalEffect
    {
        public delegate void EffectApplicator();

        string _name;
        EffectCode _code;
        bool _isBenefical;
        bool _removable;
        bool _expirable;

        public string Name { get { return _name; } }
        public EffectCode Code { get { return _code; } }
        public bool IsBenefical { get { return _isBenefical; } }
        public bool Removable { get { return _removable; } }
        public bool Expirable { get { return _expirable; } }

        public EffectApplicator EntryEffect; // Applied ONCE when the effect comes into play
        public EffectApplicator TickEffect; // Applied whenever the effect 'ticks', e.g BURN
        public EffectApplicator PersistantEffect; // Applied constantly until removed/expired
        public EffectApplicator LeaveEffect; // Applied ONCE when effect leaves
    }

    private interface InternalEffect
    {
        string Name { get; }
        EffectCode Code { get; }
        bool IsBenefical { get; }
        bool Removable { get; }
        bool Expirable { get; }
    }

    public enum EffectCode
    {
        Freeze,
        Bleed,
        Burn,
        Stun,
        Heal,
        Poison,
        Slow,
        Might,
    }
}
