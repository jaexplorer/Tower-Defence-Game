using System.Collections.Generic;
using UnityEngine;
// public delegate void UpdateManagerCallback();

public class UpdateManager : MonoBehaviour
{
    [SerializeField] private int _framesPerSecond;

    private static List<IUpdatable> _items = new List<IUpdatable>(256);
    private static List<IUpdatable> _itemsToRemove = new List<IUpdatable>(16);
    private static List<IUpdatable> _itemsToAdd = new List<IUpdatable>(32);
    private static UpdateManager _instance;

    //PROPERTIES///////////////////////////////////////////////
    public static int framesPerSecond { get { return _instance._framesPerSecond; } }

    //EVENTS///////////////////////////////////////////////////
    public void Awake()
    {
        if (_instance != null)
        {
            Debug.Log("DOUBLE INSTANCE OF UPDATE MANAGER SINGLETON");
        }
        _instance = this;
        // framesPerSecond = (int)(1 / Time.fixedDeltaTime);
        // Debug.Log(framesPerSecond);
    }

    //PUBLIC///////////////////////////////////////////////////
    public void FixedUpdate()
    {
        AddAndRemove();
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].ManagedUpdate();
        }
    }

    public static void AddItem(IUpdatable item)
    {
        _itemsToAdd.Add(item);
    }

    public static void RemoveItem(IUpdatable item)
    {
        _itemsToRemove.Add(item);
    }

    public static void Clear()
    {

    }

    public static void SetUpdateOrder(IUpdatable item, int index)
    {
        AddAndRemove();
        if (index < _items.Count && index >= 0)
        {
            int currentIndex = _items.IndexOf(item);
            if (currentIndex != -1)
            {
                IUpdatable swapItem = _items[index];
                _items[currentIndex] = swapItem;
                _items[index] = item;
            }
            else
            {
                Debug.LogError("Unable to set an update order, the item doesn't exist in the list.");
            }
        }
        else
        {
            Debug.LogError("Update index out of range. " + "(" + index + ")");
        }
    }

    public static int GetCount()
    {
        return _items.Count;
    }

    public static bool Contains(IUpdatable item)
    {
        return _items.Contains(item);
    }

    private static void AddAndRemove()
    {
        if (_itemsToRemove.Count > 0)
        {
            // Debug.Log(_itemsToRemove.Count);
            for (int i = 0; i < _itemsToRemove.Count; i++)
            {
                if (!_items.Remove(_itemsToRemove[i]))
                {
                    if (!_itemsToAdd.Remove(_itemsToRemove[i]))
                    {
                        Debug.LogError("Item can't be removed. Not found.");
                    }
                }
            }
            _itemsToRemove.Clear();
        }
        if (_itemsToAdd.Count > 0)
        {
            // Debug.Log(_itemsToAdd.Count);
            _items.AddRange(_itemsToAdd);
            _itemsToAdd.Clear();
        }
    }
}