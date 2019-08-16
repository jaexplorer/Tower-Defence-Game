using UnityEngine;
using System.Collections.Generic;

public class RayTower : Tower
{
    //PRIVATE//////////////////////////////////////////////////
    protected override void Shoot()
    {
        ProduceProjectile().Launch(GetLastTarget(), this);
    }
}