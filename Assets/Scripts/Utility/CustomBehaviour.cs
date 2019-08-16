using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBehaviour : MonoBehaviour
{
    [EventManager.Ignore] protected virtual void OnLevelLoad() { }
    [EventManager.Ignore] protected virtual void OnLevelClear() { }
    [EventManager.Ignore] protected virtual void OnWaveSave() { }
    [EventManager.Ignore] protected virtual void OnWaveLoad() { }
    [EventManager.Ignore] protected virtual void OnWaveStart() { }
    [EventManager.Ignore] protected virtual void OnTilesChange() { }
    [EventManager.Ignore] protected virtual void OnWaveComplete() { }
    [EventManager.Ignore] protected virtual void OnWaveLost() { }
    [EventManager.Ignore] protected virtual void OnEnemyDeath(Enemy enemy) { }
    [EventManager.Ignore] protected virtual void OnGameStateChange(GameState gameState) { }
}