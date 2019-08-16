using UnityEngine.Events;

public class EnemyEvent : UnityEvent<Enemy> { }

// using System;
// using System.Reflection;
// using System.Collections.Generic;

// public class Events : MonoBehaviour
// {
//     // // New event system.
//     // private static Dictionary<Type, TypeInfo> _typeInfoDictionary = new Dictionary<Type, TypeInfo>(100);
//     // private static List<Event> _events = new List<Event>(16);
//     // private static Event<int> _onIntEventTest = new Event<int>("OnIntEventTest");
//     // private static Event _onEventTest = new Event("OnEventTest");

//     // private void Awake()
//     // {
//     //     _events.Add(_onEventTest);
//     //     _events.Add(_onIntEventTest);

//     //     // Test
//     //     ESTest test = new ESTest();
//     //     Subscribe(test);
//     //     _onEventTest.Invoke();
//     //     _onIntEventTest.Invoke(6418276);
//     //     Unsubscribe(test);
//     //     _onEventTest.Invoke();
//     // }

//     // public static void Subscribe(object obj)
//     // {
//     //     TypeInfo typeInfo = null;
//     //     Type type = obj.GetType();
//     //     int count = 0;
//     //     if (_typeInfoDictionary.TryGetValue(type, out typeInfo))
//     //     {
//     //         foreach (IEventInternal e in typeInfo._supportedEvents)
//     //         {
//     //             count++;
//     //             e.Subscribe(obj);
//     //         }
//     //         Debug.Log("Subscribed to " + count + " events. Using cached TypeInfo.");
//     //     }
//     //     else
//     //     {
//     //         List<Event> supportedEvents = new List<Event>(16);
//     //         foreach (Event e in _events)
//     //         {
//     //             if (((IEventInternal)e).Subscribe(obj))
//     //             {
//     //                 count++;
//     //                 supportedEvents.Add(e);
//     //             }
//     //         }
//     //         if (supportedEvents.Count > 0)
//     //         {
//     //             _typeInfoDictionary.Add(type, new TypeInfo(type, supportedEvents));
//     //         }
//     //         Debug.Log("Subscribed to " + count + " events. With new TypeInfo.");
//     //     }
//     // }

//     // public static void Unsubscribe(object obj)
//     // {
//     //     TypeInfo typeInfo = null;
//     //     Type type = obj.GetType();
//     //     int count = 0;
//     //     if (_typeInfoDictionary.TryGetValue(type, out typeInfo))
//     //     {
//     //         foreach (IEventInternal e in typeInfo._supportedEvents)
//     //         {
//     //             count++;
//     //             e.Unsubscribe(obj);
//     //         }
//     //         Debug.Log("Unsubscribed from " + count + " events.");
//     //     }
//     // }

//     // private class TypeInfo
//     // {
//     //     public Type _type;
//     //     public List<Event> _supportedEvents;
//     //     public List<MethodInfo> _methodInfo;

//     //     public TypeInfo(Type type, List<Event> events)
//     //     {
//     //         _type = type;
//     //         _supportedEvents = events;
//     //     }
//     // }

//     // public class Event : IEventInternal
//     // {
//     //     protected delegate void MyDelegate();
//     //     protected MyDelegate _delegate;
//     //     protected Dictionary<Type, MethodInfo> _methodInfoDictionary = new Dictionary<Type, MethodInfo>(100);
//     //     protected Dictionary<object, Delegate> _delegateDictionary = new Dictionary<object, Delegate>(100);
//     //     protected string _name;

//     //     public Event(string eventName)
//     //     {
//     //         _name = eventName;
//     //     }

//     //     public void Invoke()
//     //     {
//     //         if (_delegate != null)
//     //         {
//     //             _delegate.Invoke();
//     //         }
//     //     }

//     //     bool IEventInternal.Subscribe(object obj)
//     //     {
//     //         return Subscribe(obj);
//     //     }

//     //     protected virtual bool Subscribe(object obj)
//     //     {
//     //         MethodInfo methodInfo = null;
//     //         if (!_methodInfoDictionary.TryGetValue(obj.GetType(), out methodInfo))
//     //         {
//     //             methodInfo = obj.GetType().GetMethod(_name);
//     //             if (methodInfo != null)
//     //             {
//     //                 _methodInfoDictionary.Add(obj.GetType(), methodInfo);
//     //             }
//     //             else
//     //             {
//     //                 return false;
//     //             }
//     //         }
//     //         Delegate newDelegate = Delegate.CreateDelegate(typeof(MyDelegate), obj, methodInfo);
//     //         if (newDelegate != null)
//     //         {
//     //             _delegate += (MyDelegate)newDelegate;
//     //             _delegateDictionary.Add(obj, newDelegate);
//     //             return true;
//     //         }
//     //         return false;
//     //     }

//     //     void IEventInternal.Unsubscribe(object obj)
//     //     {
//     //         Unsubscribe(obj);
//     //     }

//     //     protected virtual void Unsubscribe(object obj)
//     //     {
//     //         Delegate theDelegate = null;
//     //         if (_delegateDictionary.TryGetValue(obj, out theDelegate))
//     //         {
//     //             _delegate -= (MyDelegate)theDelegate;
//     //         }
//     //         else
//     //         {
//     //             Debug.LogError("Delegate not found.");
//     //         }
//     //     }
//     // }

//     // public class Event<T> : Event
//     // {
//     //     private new delegate void MyDelegate(T parameter);
//     //     private new MyDelegate _delegate;

//     //     public Event(string eventName) : base(eventName)
//     //     {

//     //     }

//     //     public void Invoke(T parameter)
//     //     {
//     //         _delegate.Invoke(parameter);
//     //     }

//     //     protected override bool Subscribe(object obj)
//     //     {
//     //         MethodInfo methodInfo = null;
//     //         if (!_methodInfoDictionary.TryGetValue(obj.GetType(), out methodInfo))
//     //         {
//     //             methodInfo = obj.GetType().GetMethod(_name);
//     //             if (methodInfo != null)
//     //             {
//     //                 _methodInfoDictionary.Add(obj.GetType(), methodInfo);
//     //             }
//     //             else
//     //             {
//     //                 return false;
//     //             }
//     //         }
//     //         // Debug.Log(methodInfo.GetParameters().Length);
//     //         // Debug.Log(this.GetType());
//     //         Delegate newDelegate = Delegate.CreateDelegate(typeof(MyDelegate), obj, methodInfo);
//     //         if (newDelegate != null)
//     //         {
//     //             _delegate += (MyDelegate)newDelegate;
//     //             _delegateDictionary.Add(obj, newDelegate);
//     //             return true;
//     //         }
//     //         return false;
//     //     }

//     //     protected override void Unsubscribe(object obj)
//     //     {
//     //         Delegate theDelegate = null;
//     //         if (_delegateDictionary.TryGetValue(obj, out theDelegate))
//     //         {
//     //             _delegate -= (MyDelegate)theDelegate;
//     //         }
//     //         else
//     //         {
//     //             Debug.LogError("Delegate not found.");
//     //         }
//     //     }
//     // }

//     // private interface IEventInternal
//     // {
//     //     bool Subscribe(object obj);
//     //     void Unsubscribe(object obj);
//     // }

//     // public class ESTest
//     // {
//     //     public void OnEventTest()
//     //     {
//     //         Debug.Log("OnEventTest triggered");
//     //     }

//     //     public void OnIntEventTest(int integer)
//     //     {
//     //         Debug.Log("OnIntEventTest triggered with parameter: " + integer);
//     //     }
//     // }

//     // Legacy.
//     // private static GameStateEvent _onGameStateChange = new GameStateEvent();
//     // private static UnityEvent _onLevelReset = new UnityEvent();
//     // private static UnityEvent _onTileChange = new UnityEvent();
//     // private static UnityEvent _onLevelLoad = new UnityEvent();
//     // private static UnityEvent _onWaveStart = new UnityEvent();
//     // private static UnityEvent _onWaveSave = new UnityEvent();
//     // private static UnityEvent _onWaveLoad = new UnityEvent();
//     // private static UnityEvent _onPortalDepleted = new UnityEvent();
//     // private static UnityEvent _onAllEnemiesDestroyed = new UnityEvent();
//     // private static EnemyEvent _onEnemyDeath = new EnemyEvent();

//     //PROPERTIES///////////////////////////////////////////////
//     // public static GameStateEvent onGameStateChange { get { return _onGameStateChange; } }
//     // public static UnityEvent onLevelLoad { get { return _onLevelLoad; } }
//     // public static UnityEvent onLevelClear { get { return _onLevelReset; } }
//     // public static UnityEvent onTileChange { get { return _onTileChange; } }
//     // public static UnityEvent onWaveStart { get { return _onWaveStart; } }
//     // public static UnityEvent onWaveSave { get { return _onWaveSave; } }
//     // public static UnityEvent onWaveLoad { get { return _onWaveLoad; } }
//     // public static UnityEvent onPortalDepleted { get { return _onPortalDepleted; } }
//     // public static UnityEvent onAllEnemiesDestroyed { get { return _onAllEnemiesDestroyed; } }
//     // public static EnemyEvent onEnemyDeath { get { return _onEnemyDeath; } }
// }

