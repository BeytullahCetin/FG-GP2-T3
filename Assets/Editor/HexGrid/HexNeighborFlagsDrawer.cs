using UnityEngine;
using UnityEditor;
using System;

namespace FG_GP2_T3
{
    [CustomPropertyDrawer(typeof(HexRoadSet))]
    public class HexRoadsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty _roadsArray = property.FindPropertyRelative("_roads");

            if (_roadsArray == null)
            {
                EditorGUI.LabelField(position, label.text, "Missing _roads field");
                return;
            }

            if (_roadsArray.arraySize != 6)
                _roadsArray.arraySize = 6;

            label = EditorGUI.BeginProperty(position, label, property);
            Rect _contentPosition = EditorGUI.PrefixLabel(position, label);

            float _spacing = 2f;
            float _fieldWidth = (_contentPosition.width / 6f) - _spacing;
            
            string[] _directionNames = Enum.GetNames(typeof(HexDirection));

            Rect _currentRect = new Rect(_contentPosition.x, _contentPosition.y, _fieldWidth, _contentPosition.height);

            for (int i = 0; i < 6; i++)
            {
                SerializedProperty _element = _roadsArray.GetArrayElementAtIndex(i);

                float _textWidth = 20f; 
                Rect _labelRect = new Rect(_currentRect.x, _currentRect.y, _textWidth, _currentRect.height);
                Rect _checkRect = new Rect(_currentRect.x + _textWidth, _currentRect.y, _currentRect.width - _textWidth, _currentRect.height);

                GUIStyle _style = new GUIStyle(EditorStyles.miniLabel);
                _style.alignment = TextAnchor.MiddleRight;
                EditorGUI.LabelField(_labelRect, _directionNames[i], _style);

                _element.boolValue = EditorGUI.Toggle(_checkRect, GUIContent.none, _element.boolValue);
                
                _currentRect.x += _fieldWidth + _spacing;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;
    }
}