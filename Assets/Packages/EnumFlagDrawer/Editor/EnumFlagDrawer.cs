using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumFlags))]
public class EnumFlagsDrawer : PropertyDrawer
{
    static bool _foldout;
    // int _memberCount;
    const int _memberHeight = 15;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int buttonsIntValue = 0;
        int enumLength = property.enumNames.Length;
        bool[] buttonPressed = new bool[enumLength];
        // float buttonWidth = (_position.width - EditorGUIUtility.labelWidth) / enumLength;
        float buttonWidth = EditorGUIUtility.labelWidth;

        // EditorGUI.Foldout();
        // _memberCount = enumLength;
        // EditorGUI.LabelField(new Rect(_position.x, _position.y, EditorGUIUtility.labelWidth, _position.height), _label);
        _foldout = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, 20), _foldout, label);
        if (_foldout)
        {
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < enumLength; i++)
            {
                // Check if the button is/was pressed 
                if ((property.intValue & (1 << i)) == 1 << i)
                {
                    buttonPressed[i] = true;
                }
                Rect buttonPos = new Rect(position.x + 20, position.y + 20 + i * _memberHeight, EditorGUIUtility.labelWidth + 20, _memberHeight);
                // buttonPressed[i] = GUI.Toggle(buttonPos, property.enumNames[i], buttonPressed[i], "Button");
                buttonPressed[i] = GUI.Toggle(buttonPos, buttonPressed[i], property.enumNames[i], "Button");

                if (buttonPressed[i])
                    buttonsIntValue += 1 << i;
            }
            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = buttonsIntValue;
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!_foldout)
        {
            return base.GetPropertyHeight(property, label);
        }
        else
        {
            return base.GetPropertyHeight(property, label) + _memberHeight * property.enumNames.Length + 20;
        }
    }
}