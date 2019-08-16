using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class ProfileManager : MonoBehaviour
{
    private string _name;
    private SaveData _saveData = new SaveData();
    private string _directory;

    //PROPERTIES///////////////////////////////////////////////
    public static ProfileManager instance
    {
        get;
        private set;
    }

    public SaveData saveData
    {
        get { return _saveData; }
    }

    //EVENTS///////////////////////////////////////////////////
    private void Awake()
    {
        instance = this;
        _name = "test";
    }

    //PUBLIC///////////////////////////////////////////////////
    public void CreateProfile(string name)
    {
        _name = name;
        _saveData.scores = null;
        _saveData.tileSaveData = null;
        _saveData.currentLevelId = 0;
        _saveData.currentPortalPoints = 0;
        _saveData.currentWaveIndex = 0;
        SaveProfile();
        LoadProfile(name);
    }

    public void DeleteProfile(string name)
    {
        File.Delete(DirectoryFromName(name));
    }

    public void SaveProfile()
    {
        string json = JsonUtility.ToJson(_saveData);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(DirectoryFromName(_name), FileMode.OpenOrCreate);
        File.Copy(DirectoryFromName(_name), BackupDirectoryFromName(_name));
        FileStream backupFile = File.Open(BackupDirectoryFromName(_name), FileMode.OpenOrCreate);
        bf.Serialize(file, json);
        file.Close();
    }

    public void LoadProfile(string name)
    {
        if (File.Exists(_directory))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(DirectoryFromName(name), FileMode.OpenOrCreate);
            string json = (string)bf.Deserialize(file);
            file.Close();
            JsonUtility.FromJsonOverwrite(json, _saveData);
        }
    }

    public void GetProfileList(List<string> _profiles)
    {

    }

    //PRIVATE//////////////////////////////////////////////////
    private string DirectoryFromName(string name)
    {
        return Application.persistentDataPath + "/" + name + ".bin";
    }

    private string BackupDirectoryFromName(string name)
    {
        return Application.persistentDataPath + "/" + name + ".bkp";
    }

    [System.Serializable]
    public class SaveData
    {
        public int[] scores;
        public int currentPortalPoints;
        public int currentLevelId;
        public int currentWaveIndex;
        public int energy;
        public List<Tile.SaveData> tileSaveData = new List<Tile.SaveData>(256);
    }
}