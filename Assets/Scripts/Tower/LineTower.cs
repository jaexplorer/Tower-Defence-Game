using UnityEngine;
using System.Collections.Generic;

public class LineTower : Tower
{
    [SerializeField]
    private LineTowerTrigger[] _triggers;

    //PUBLIC///////////////////////////////////////////////////
    public override void GetTiles(TilePosition tilePosition, List<Tile> pathTiles, List<Tile> validTiles = null, List<Tile> blockedTiles = null)
    {
        for (int i = 0; i < _triggers.Length; i++)
        {
            _triggers[i].GetTiles(tilePosition, pathTiles, validTiles, blockedTiles);
        }
    }

    //EVENTS///////////////////////////////////////////////////
    protected override void OnWaveStart()
    {
        base.OnWaveStart();
        for (int i = 0; i < _triggers.Length; i++)
        {
            _triggers[i].OnWaveStarted();
        }
    }

    //PROTECTED////////////////////////////////////////////////
    protected override void MarkTiles()
    {
        TilePosition mapPosition = Level.instance.GetTile(_transform.position).position;
        for (int i = 0; i < _triggers.Length; i++)
        {
            _triggers[i].MarkTiles();
        }
    }

    protected override void Shoot()
    {
        int maxTargetCount = 0;
        LineTowerTrigger chosenTrigger = null;
        for (int i = 0; i < _triggers.Length; i++)
        {
            if (_triggers[i].enemies.Count > maxTargetCount)
            {
                maxTargetCount = _triggers[i].enemies.Count;
                chosenTrigger = _triggers[i];
            }
        }
        // _audioSource.Play();
        AudioSource.PlayClipAtPoint(_audioSource.clip, _transform.position);
        ((LineProjectile)ProduceProjectile()).Launch(chosenTrigger);
        Cooldown();
    }

    // protected override void OnProduce()
    // {
    //     base.OnProduce();
    //     EventManager.onLevelChange.AddListener(MarkTiles);
    // }
}











































// private int _enemyCount;

// private void Start()
// {
//     _tile = Level.current.GetTile(_transform.position);
//     // _cooldownDelay = new WaitForSeconds(_data.cooldown);
//     OnPathChanged();
// }

//PUBLIC///////////////////////////////////////////////////
// public override void OnPathChanged()
// {
//     for (int i = 0; i < _triggers.Length; i++)
//     {
//         LineTowerTrigger trigger = _triggers[i];
//         int range = _data.range;
//         for (int r = 0; r < _data.range; r++)
//         {
//             Tile tile = Level.current.GetTile(_tile.mapPosition.x + trigger.rangeMultiplierX * (r + 1), _tile.mapPosition.z + trigger.rangeMultiplierZ * (r + 1));
//             if (tile == null || !tile.walkable)
//             {
//                 range = r;
//                 break;
//             }
//         }
//         trigger.SetRange(range);
//     }
// }

// public override LineTowerTrigger GetLineTowerTrigger()
// {
//     LineTowerTrigger targetTrigger = _triggers[0];
//     int highestEnemyCount = 0;
//     for (int i = 0; i < _triggers.Length; i++)
//     {
//         if (highestEnemyCount < _triggers[i].enemyCount)
//         {
//             highestEnemyCount = _triggers[i].enemyCount;
//             targetTrigger = _triggers[i];
//         }
//     }
//     return targetTrigger;
// }
