using UnityEngine;

// Contains persistent level editor data.
[CreateAssetMenu(fileName = "LevelEditorData", menuName = "Data/LevelEditorData", order = 1)]
public class LevelEditorData : ScriptableObject
{
    public LevelData lastActiveLevel;
    public Vector3 cameraPosition;
    public float cameraOrthographicSize;
}