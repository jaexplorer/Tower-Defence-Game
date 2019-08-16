// MIT License

// Copyright (c) 2017 Vladislav Kostin

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _subscribedOnAwake;

    private static Dictionary<Type, TypeInfo> _typeInfoDictionary = new Dictionary<Type, TypeInfo>(100);
    private static List<Event> _events = new List<Event>(16);

    public static readonly Event onLevelLoad = new Event("OnLevelLoad");
    public static readonly Event onLevelClear = new Event("OnLevelClear");
    public static readonly Event onWaveSave = new Event("OnWaveSave");
    public static readonly Event onWaveLoad = new Event("OnWaveLoad");
    public static readonly Event onWaveStart = new Event("OnWaveStart");
    public static readonly Event onWaveComplete = new Event("OnWaveComplete");
    public static readonly Event onWaveLost = new Event("OnWaveLost");
    public static readonly Event onBuildingStart = new Event("OnBuildingStart");
    public static readonly Event onTilesChange = new Event("OnTilesChange");
    public static readonly Event<Enemy> onEnemyDeath = new Event<Enemy>("OnEnemyDeath");
    public static readonly Event<GameState> onGameStateChange = new Event<GameState>("OnGameStateChange");

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        _events.Add(onLevelLoad);
        _events.Add(onLevelClear);
        _events.Add(onWaveSave);
        _events.Add(onWaveLoad);
        _events.Add(onWaveStart);
        _events.Add(onWaveComplete);
        _events.Add(onWaveLost);
        _events.Add(onBuildingStart);
        _events.Add(onTilesChange);
        _events.Add(onEnemyDeath);
        _events.Add(onGameStateChange);

        foreach (var obj in _subscribedOnAwake)
        {
            Subscribe(obj);
        }
    }

    //PUBLIC///////////////////////////////////////////////////
    public static void Subscribe(object obj)
    {
        TypeInfo typeInfo = null;
        Type type = obj.GetType();
        int count = 0;
        if (_typeInfoDictionary.TryGetValue(type, out typeInfo))
        {
            foreach (IEventInternal e in typeInfo._supportedEvents)
            {
                count++;
                e.Subscribe(obj);
            }
        }
        else
        {
            List<Event> supportedEvents = new List<Event>(16);
            foreach (Event e in _events)
            {
                if (((IEventInternal)e).Subscribe(obj))
                {
                    count++;
                    supportedEvents.Add(e);
                }
            }
            if (supportedEvents.Count > 0)
            {
                _typeInfoDictionary.Add(type, new TypeInfo(type, supportedEvents));
            }
        }
    }

    public static void Unsubscribe(object obj)
    {
        TypeInfo typeInfo = null;
        Type type = obj.GetType();
        int count = 0;
        if (_typeInfoDictionary.TryGetValue(type, out typeInfo))
        {
            foreach (IEventInternal e in typeInfo._supportedEvents)
            {
                count++;
                e.Unsubscribe(obj);
            }
        }
    }

    //TYPES////////////////////////////////////////////////////
    private class TypeInfo
    {
        public Type _type;
        public List<Event> _supportedEvents;

        public TypeInfo(Type type, List<Event> events)
        {
            _type = type;
            _supportedEvents = events;
        }
    }

    public class Event : IEventInternal
    {
        protected delegate void MyDelegate();
        protected Dictionary<Type, MethodInfo> _methodInfoDictionary = new Dictionary<Type, MethodInfo>(100);
        protected Dictionary<object, Delegate> _delegateDictionary = new Dictionary<object, Delegate>(100);
        protected string _name;
        protected MyDelegate _delegate;

        public Event(string eventName)
        {
            _name = eventName;
        }

        public void Invoke()
        {
            if (_delegate != null)
            {
                _delegate.Invoke();
            }
        }

        bool IEventInternal.Subscribe(object obj)
        {
            return Subscribe(obj);
        }

        protected virtual bool Subscribe(object obj)
        {
            MethodInfo methodInfo = null;
            if (!_methodInfoDictionary.TryGetValue(obj.GetType(), out methodInfo))
            {
                methodInfo = obj.GetType().GetMethod(_name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                if (methodInfo != null)
                {
                    if (methodInfo.GetCustomAttributes(typeof(Ignore), false).Length > 0)
                    {
                        return false;
                    }
                    _methodInfoDictionary.Add(obj.GetType(), methodInfo);
                }
                else
                {
                    return false;
                }
            }
            Delegate newDelegate = Delegate.CreateDelegate(typeof(MyDelegate), obj, methodInfo);
            if (newDelegate != null)
            {
                _delegate += (MyDelegate)newDelegate;
                _delegateDictionary.Add(obj, newDelegate);
                return true;
            }
            return false;
        }

        void IEventInternal.Unsubscribe(object obj)
        {
            Unsubscribe(obj);
        }

        protected virtual void Unsubscribe(object obj)
        {
            Delegate theDelegate = null;
            if (_delegateDictionary.TryGetValue(obj, out theDelegate))
            {
                _delegate -= (MyDelegate)theDelegate;
            }
            else
            {
                Debug.LogError("Delegate not found.");
            }
        }
    }

    public class Event<T> : Event
    {
        private new delegate void MyDelegate(T parameter);
        private new MyDelegate _delegate;

        public Event(string eventName) : base(eventName) { }

        public void Invoke(T parameter)
        {
            if (_delegate != null)
            {
                _delegate.Invoke(parameter);
            }
        }

        protected override bool Subscribe(object obj)
        {
            MethodInfo methodInfo = null;
            if (!_methodInfoDictionary.TryGetValue(obj.GetType(), out methodInfo))
            {
                methodInfo = obj.GetType().GetMethod(_name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                if (methodInfo != null)
                {
                    if (methodInfo.GetCustomAttributes(typeof(Ignore), false).Length > 0)
                    {
                        return false;
                    }
                    _methodInfoDictionary.Add(obj.GetType(), methodInfo);
                }
                else
                {
                    return false;
                }
            }
            Delegate newDelegate = Delegate.CreateDelegate(typeof(MyDelegate), obj, methodInfo);
            if (newDelegate != null)
            {
                _delegate += (MyDelegate)newDelegate;
                _delegateDictionary.Add(obj, newDelegate);
                return true;
            }
            return false;
        }

        protected override void Unsubscribe(object obj)
        {
            Delegate theDelegate = null;
            if (_delegateDictionary.TryGetValue(obj, out theDelegate))
            {
                _delegate -= (MyDelegate)theDelegate;
            }
            else
            {
                Debug.LogError("Delegate not found.");
            }
        }
    }

    private interface IEventInternal
    {
        bool Subscribe(object obj);
        void Unsubscribe(object obj);
    }

    public class Ignore : Attribute
    {

    }
}
