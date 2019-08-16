using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// Base class, for objects that need simulation in the game, objects that are part of the game state
public class ManagedObject : CustomBehaviour, IUpdatable, IPoolItem
{
    [SerializeField] private bool _onManagedUpdate;
    [SerializeField] private UnityEvent _onRecycle;
    private int _recycleTimer;
    private bool _pendindgRecycle;

    protected bool pendingRecycle { get { return _pendindgRecycle; } }
    public UnityEvent onRecycle { get { return _onRecycle; } }
    public int updatePriority { get { return 0; } }

    void IPoolItem.OnInstantiate()
    {
        OnInstantiate();
    }

    void IPoolItem.OnProduce()
    {
        EventManager.Subscribe(this);
        _pendindgRecycle = false;
        _recycleTimer = 0;
        if (_onManagedUpdate)
        {
            UpdateManager.AddItem(this);
        }
        OnProduce();
    }

    void IPoolItem.OnRecycle()
    {
        EventManager.Unsubscribe(this);
        if (_onManagedUpdate)
        {
            UpdateManager.RemoveItem(this);
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

    // protected void SetUpdateOrder(int order)
    // {
    //     if (_onManagedUpdate)
    //     {
    //         // UpdateManager.SetUpdateOrder(this, order);
    //     }
    // }

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