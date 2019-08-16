using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class GradientMapEditor : EditorWindow
{
    private List<GradientMap> _gradientMaps = new List<GradientMap>(16);
    private GradientMap _currentMap;
    private GradientMap.GradientData _currentGradientData;
    private bool _showMapList;
    private int _currentGradientIndex;
    private int _currentGradientX;
    private int _currentGradientY;
    public Gradient _currentGradient;

    [MenuItem("Window/Gradient Map Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GradientMapEditor));
    }

    private void OnEnable()
    {
        string[] guids = AssetDatabase.FindAssets("t:GradientMap");
        foreach (string guid in guids)
        {
            _gradientMaps.Add(AssetDatabase.LoadAssetAtPath<GradientMap>(AssetDatabase.GUIDToAssetPath(guid)));
        }
        if (_currentMap == null && _gradientMaps.Count > 0)
        {
            _currentMap = _gradientMaps[0];
        }

        for (int x = 0; x < _currentMap.columns; x++)
        {
            for (int y = 0; y < _currentMap.rows; y++)
            {
                UpdateTexture(x, y);
            }
        }

        if (_currentMap.texture == null)
        {
            _currentMap.texture = new Texture2D(512, 512);
            SaveTexture();
        }
    }

    private void OnDisable()
    {
        _gradientMaps.Clear();
    }

    private void OnGUI()
    {
        int verticalOffset = 5;
        if (_showMapList)
        {
            for (int i = 0; i < _gradientMaps.Count; i++)
            {
                if (_gradientMaps[i] == _currentMap)
                {
                    GUI.backgroundColor = Color.grey;
                }
                if (GUI.Button(new Rect(5, verticalOffset, position.width - 10, 25), _gradientMaps[i].name))
                {
                    _currentMap = _gradientMaps[i];
                    _showMapList = false;
                }
                verticalOffset += 25;
                GUI.backgroundColor = Color.white;
            }
        }
        else
        {
            if (GUI.Button(new Rect(5, verticalOffset, position.width - 10, 25), _currentMap.name))
            {
                _showMapList = !_showMapList;
            }
            verticalOffset += 30;

            if (_currentMap)
            {
                // Texture.
                if (_currentMap.texture == null)
                {
                    Texture2D texture = new Texture2D(512, 512);
                    File.WriteAllBytes(Application.dataPath + "/Textures/GradientMaps/" + _currentMap.name + ".png", texture.EncodeToPNG());
                    _currentMap.texture = texture;
                }
                GUI.DrawTexture(new Rect(0, verticalOffset, position.width, position.width), _currentMap.texture);

                // Gradient buttons.
                float gradientHeight = position.width / _currentMap.rows;
                float gradientWidth = position.width / _currentMap.columns;
                GUI.backgroundColor = new Color(1f, 1f, 1f, 0f);
                for (int x = 0; x < _currentMap.columns; x++)
                {
                    for (int y = 0; y < _currentMap.rows; y++)
                    {
                        var gradientData = _currentMap.GetGradientData(x, y);
                        if (gradientData == _currentGradientData)
                        {
                            GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);
                            GUI.Box(new Rect(x * gradientWidth, verticalOffset + y * gradientHeight, 10, gradientHeight), "");
                            GUI.Box(new Rect(x * gradientWidth + gradientWidth - 10, verticalOffset + y * gradientHeight, 10, gradientHeight), "");
                            GUI.backgroundColor = new Color(1f, 1f, 1f, 0f);
                        }
                        if (GUI.Button(new Rect(x * gradientWidth, verticalOffset + y * gradientHeight, gradientWidth, gradientHeight), gradientData.name))
                        {
                            _currentGradientData = gradientData;
                            _currentGradientIndex = _currentMap.GetGradientIndex(x, y);
                            _currentGradient = _currentMap.GetGradientData(x, y).gradient;
                            _currentGradientX = x;
                            _currentGradientY = y;
                            Repaint();
                        }
                    }
                }
                verticalOffset += (int)position.width + 5;

                // Current gradient data. 
                if (_currentGradientData != null)
                {
                    GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);
                    EditorGUI.BeginChangeCheck();
                    SerializedObject so = new SerializedObject(_currentMap);
                    SerializedProperty gradientProperty = so.FindProperty("_gradientData.Array.data[" + _currentGradientIndex + "].gradient");
                    SerializedProperty nameProperty = so.FindProperty("_gradientData.Array.data[" + _currentGradientIndex + "].name");
                    EditorGUI.PropertyField(new Rect(5, verticalOffset, position.width - 10, 25), gradientProperty, new GUIContent("Gradient"), true);
                    verticalOffset += 20;
                    EditorGUI.PropertyField(new Rect(5, verticalOffset, position.width - 10, 25), nameProperty, new GUIContent("Name"), true);
                    verticalOffset += 25;
                    if (EditorGUI.EndChangeCheck())
                    {
                        so.ApplyModifiedProperties();
                        Repaint();
                        UpdateTexture(_currentGradientX, _currentGradientY);
                    }
                }
            }
        }
    }

    private void UpdateTexture(int gradientPositionX, int gradientPositionY)
    {
        Debug.Log("U");
        Gradient gradient = _currentMap.GetGradientData(gradientPositionX, gradientPositionY).gradient;
        int gradientWidth = _currentMap.texture.width / _currentMap.columns;
        int gradientHeight = _currentMap.texture.height / _currentMap.rows;
        int startX = gradientWidth * gradientPositionX;
        int startY = _currentMap.texture.height - gradientHeight * (gradientPositionY + 1);
        int endX = startX + gradientWidth;
        int endY = startY + gradientHeight;
        float progressPerPixel = 1f / gradientWidth;

        for (int x = startX, i = 0; x < endX; x++, i++)
        {
            Color color = gradient.Evaluate(i * progressPerPixel);
            for (int y = startY; y < endY; y++)
            {
                _currentMap.texture.SetPixel(x, y, color);
            }
        }
        _currentMap.texture.Apply();
        SaveTexture();
    }

    private void SaveTexture()
    {
        File.WriteAllBytes(Application.dataPath + "/Textures/GradientMaps/" + _currentMap.name + ".png", _currentMap.texture.EncodeToPNG());
    }
}
