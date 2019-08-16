using UnityEngine;

// Contains persistent level editor data.
[CreateAssetMenu(fileName = "LevelEditorLocalData", menuName = "Data/LevelEditorLocalData", order = 1)]
public class LevelEditorLocalData : ScriptableObject
{
    public string lastActiveScene;
    public Vector3 cameraPosition;
    public float cameraOrthographicSize;
}