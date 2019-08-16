using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier
{
    private SpeedModifier _speedMod = new SpeedModifier();
    private ArmorModifier _armorMod = new ArmorModifier();
    private HealthModifier _healthMod = new HealthModifier();
    private AttackModifier _attackMod = new AttackModifier();
    private RangeModifier _rangeMod = new RangeModifier();
    private ReloadModifier _reloadMod = new ReloadModifier();

    //PUBLIC///////////////////////////////////////////////////
    public SpeedModifier speedMod { get { return _speedMod; } }
    public ArmorModifier armorMod { get { return _armorMod; } }
    public HealthModifier healthMod { get { return _healthMod; } }
    public AttackModifier attackMod { get { return _attackMod; } }
    public RangeModifier rangeMod { get { return _rangeMod; } }
    public ReloadModifier reloadMod { get { return _reloadMod; } }

    //ENEMY//////////////////////////////////////////////////////////////
    public class SpeedModifier // For Enemies
    {
    }
    public class ArmorModifier // For Enemies
    {
    }
    public class HealthModifier // For Enemies
    {
    }

    //TOWER//////////////////////////////////////////////////////////////
    public class AttackModifier // For Towers
    {
    }
    public class RangeModifier // For Towers
    {
    }
    public class ReloadModifier // For Towers
    {
    }

}
