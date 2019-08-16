using UnityEngine;
using UnityEditor;

// Contains persistent level editor data.
[CreateAssetMenu(fileName = "LevelEditorData", menuName = "Data/LevelEditorData", order = 1)]
public class LevelEditorData : ScriptableObject
{
    public SceneAsset scene;
    public float cameraOrthographicSize;
}