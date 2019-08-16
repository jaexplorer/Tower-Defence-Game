using UnityEngine;
using UnityEditor;

public class ToolSize : LevelEditorTool
{
    override public void OnEnable()
    {
        _name = "Size";
    }

    public override void DrawGUI()
    {
        if (GUI.Button(new Rect(10, 130, 100, 25), "Bottom Left -"))
        {
            Resize(-1, 0, 0, 0);
        }
        if (GUI.Button(new Rect(10, 100, 100, 25), "Bottom Left +"))
        {
            Resize(1, 0, 0, 0);
        }
        if (GUI.Button(new Rect(110, 130, 100, 25), "Bottom Right -"))
        {
            Resize(0, 0, 0, -1);
        }
        if (GUI.Button(new Rect(110, 100, 100, 25), "Bottom Right +"))
        {
            Resize(0, 0, 0, 1);
        }
        if (GUI.Button(new Rect(10, 70, 100, 25), "Top Left -"))
        {
            Resize(0, 0, -1, 0);
        }
        if (GUI.Button(new Rect(10, 40, 100, 25), "Top Left  +"))
        {
            Resize(0, 0, 1, 0);
        }
        if (GUI.Button(new Rect(110, 70, 100, 25), "Top Right -"))
        {
            Resize(0, -1, 0, 0);
        }
        if (GUI.Button(new Rect(110, 40, 100, 25), "Top Right +"))
        {
            Resize(0, 1, 0, 0);
        }
    }

    private void Resize(int xMin, int xMax, int zMin, int zMax)
    {
        Undo.RecordObject(_levelEditor.level, "Resize");
        _levelEditor.level.MoveBorders(xMin, xMax, zMin, zMax);
        _levelEditor.level.ReloadInEditor();
        EditorUtility.SetDirty(_levelEditor.level.data);
    }
}