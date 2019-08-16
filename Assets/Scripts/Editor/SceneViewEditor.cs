using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class SceneViewEditor : EditorWindow
{
    static SceneViewEditor()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneview)
    {
        Handles.BeginGUI();
        if (GUILayout.Button("Press Me"))
            Handles.EndGUI();

        // Load MainScene if it's not loaded.
        if (!Application.isPlaying)
        {
            if (!EditorSceneManager.GetSceneByName("Main").IsValid())
            {
                EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/Main.unity", OpenSceneMode.Additive);
            }
        }
    }
}
