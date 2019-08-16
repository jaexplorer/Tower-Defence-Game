using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;

public class TestBench
{
    // private static Stopwatch sw = new Stopwatch();

    [MenuItem("TestBench/DictionaryTest")]
    static void TestArrays()
    {
        // List<GameObject> objectList = new List<GameObject>(100000);
        // List<Dummy> dummyList = new List<Dummy>(100000);
        // List<int> intList = new List<int>(100000);
        // int[] intArray = new int[500];
        // Dictionary<GameObject, int> objectDictionary = new Dictionary<GameObject, int>(100000);
        // Dictionary<Dummy, int> dummyDictionary = new Dictionary<Dummy, int>(100000);
        // Dictionary<int, int> intDictionary = new Dictionary<int, int>(100500);
        // Dictionary<int, int> intDictionary2 = new Dictionary<int, int>(101000);
        // Dictionary<int, int> intDictionary3 = new Dictionary<int, int>(1010000);

        // for (int i = 0; i < 100; i++)
        // {
        //     GameObject go = new GameObject();
        //     objectList.Add(go);
        //     objectDictionary.Add(go, i);
        //     // intArray[i] = i;
        //     intList.Add(i);
        //     intDictionary.Add(go.GetInstanceID(), i);
        //     intDictionary2.Add(go.GetInstanceID(), i);
        //     intDictionary3.Add(go.GetInstanceID(), i);
        // }

        // sw.Start();
        // for (int i = 0; i < objectList.Count; i++)
        // {
        //     int val;
        //     objectDictionary.TryGetValue(objectList[i], out val);
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // for (int i = 0; i < objectList.Count; i++)
        // {
        //     int val;
        //     intDictionary.TryGetValue(objectList[i].GetInstanceID(), out val);
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // for (int i = 0; i < objectList.Count; i++)
        // {
        //     int val;
        //     intDictionary.TryGetValue(i, out val);
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // for (int i = 0; i < intList.Count; i++)
        // {
        //     int val;
        //     val = intList[i];
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // for (int i = 0; i < intArray.Length; i++)
        // {
        //     int val;
        //     val = intArray[i];
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // intList.Remove(40);
        // intList.Add(50);
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();



        // GameObject o = new GameObject();
        // sw.Start();
        // objectDictionary.Add(o, 1234);
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // intDictionary3.Add(o.GetInstanceID(), 1234);
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // GameObject.Instantiate(o);
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();
        // sw.Start();
        // GameObject.Instantiate(o);
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();


        // GameObject go2 = new GameObject();
        // GameObject.DestroyImmediate(go2);
        // sw.Start();
        // for (int i = 0; i < intArray.Length; i++)
        // {
        //     int val;
        //     val = intArray[intArray[i]];
        //     // val = intArray[i];
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();


        // sw.Start();
        // for (int i = 0; i < 500; i++)
        // {
        //     intDictionary.Add(535135235 + i, 1234);
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // for (int i = 0; i < 500; i++)
        // {
        //     intDictionary2.Add(535135235 + i, 1234);
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // sw.Start();
        // for (int i = 0; i < 500; i++)
        // {
        //     intDictionary3.Add(535135235 + i, 1234);
        // }
        // sw.Stop();
        // UnityEngine.Debug.Log(sw.Elapsed);
        // sw.Reset();

        // for (int i = 0; i < objectList.Count; i++)
        // {
        //     GameObject.DestroyImmediate(objectList[i]);
        // }
    }

}


// public class ManagedArray<T>
// {
//     private T[] _items;
//     private int[] _indexes;

//     public ManagedArray(int capacity)
//     {

//     }

//     private class ItemContainer
//     {

//     }
// }


