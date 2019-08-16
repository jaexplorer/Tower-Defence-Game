// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using System.IO;
// using System.Runtime.Serialization.Formatters.Binary;

// public class SerializationManager : MonoBehaviour
// {
//     [SerializeField]
//     private string _saveFileName;

//     private void Awake()
//     {

//     }

//     public void Serialize<T>(T serializaleObject)
//     {
//         string json = JsonUtility.ToJson(serializaleObject);
//         BinaryFormatter bf = new BinaryFormatter();
//         FileStream file = File.Open(Application.persistentDataPath + _saveFileName, FileMode.OpenOrCreate);
//         bf.Serialize(file, json);
//         file.Close();
//     }
// }
