using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Base class, for objects that need simulation in the game, objects that are part of the game state
/// </summary>

public class ManagedObject : MonoBehaviour, IUpdatable, IPoolItem
{
    [SerializeField] private bool _onManagedUpdate;
    private int _recycleTimer;
    private bool _recycleOnLevelReset = true;
    private bool _pendindgRecycle;

    protected bool pendingRecycle
    {
        get { return _pendindgRecycle; }
    }

    void IPoolItem.OnInstantiate()
    {
        OnInstantiate();
    }

    void IPoolItem.OnProduce()
    {
        _pendindgRecycle = false;
        _recycleTimer = 0;
        if (_onManagedUpdate)
        {
            UpdateManager.AddItem(this);
        }
        if (_recycleOnLevelReset)
        {
            Events.onLevelClear.AddListener(Recycle);
        }
        OnProduce();
    }

    void IPoolItem.OnRecycle()
    {
        if (_onManagedUpdate)
        {
            UpdateManager.RemoveItem(this);
        }
        if (_recycleOnLevelReset)
        {
            Events.onLevelClear.RemoveListener(Recycle);
        }
        OnRecycle();
    }

    void IUpdatable.ManagedUpdate()
    {
        ManagedUpdate();
        if (_pendindgRecycle)
        {
            if (_recycleTimer == 0)
            {
                Recycle();
            }
            _recycleTimer--;
        }
    }

    protected virtual void OnInstantiate()
    {

    }

    protected virtual void OnProduce()
    {

    }

    protected virtual void OnRecycle()
    {

    }

    protected virtual void ManagedUpdate()
    {

    }

    protected void SetUpdateOrder(int order)
    {
        if (_onManagedUpdate)
        {
            UpdateManager.SetUpdateOrder(this, order);
        }
    }

    public void Recycle()
    {
        PoolManager.Recycle(gameObject);
    }

    public void Recycle(int delay)
    {
        if (_onManagedUpdate)
        {
            _pendindgRecycle = true;
            _recycleTimer = delay;
        }
    }
}