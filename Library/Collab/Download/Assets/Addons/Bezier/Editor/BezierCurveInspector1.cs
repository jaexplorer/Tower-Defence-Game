using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [CustomPropertyDrawer(typeof(BezierCurve))]
// public class ScaledCurveDrawer : PropertyDrawer
// {
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         EditorGUI.PropertyField(position, property, label, true);
//         if (GUILayout.Button("Precalculate"))
//         {
//             (property.serializedObject as BezierCurve).CalculateNormalizedPoints();
//         }
//     }

//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         return EditorGUI.GetPropertyHeight(property) + 30;
//     }
// }
