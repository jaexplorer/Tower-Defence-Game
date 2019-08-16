using UnityEngine;

[CreateAssetMenu(fileName = "EditorData", menuName = "Data/EditorData", order = 1)]
public class EditorData : ScriptableObject
{
    [SerializeField] private string _lastSelectedScene;

    public string lastSelectedLevel { get { return _lastSelectedScene; } set { _lastSelectedScene = value; } }
}
