using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    int _speed;
    int _armor;
    int _health;
    Effects _effects;

    public int Speed { get { return _speed; } set { _speed = value; } }
    public int Armor { get { return _armor; } set { _armor = value; } }
    public int Health { get { return _health; } set { _health = value; } }
    public Effects Effects { get { return _effects; } } 

    // NOT IMPLIMENTED YET // WILL SHOW ALL EFFECTS

}
