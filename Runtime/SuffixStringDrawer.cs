using UnityEngine;
using UnityEditor;
using System;

namespace Agava.SuffixBigIntegerUtils
{
    public class SuffixStringAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SuffixStringAttribute))]
    public class SuffixStringAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var startColor = GUI.color;

            try
            {
                BigIntegerExtention.ToBigIntegerFromSuffixFormat(property.stringValue);
            }
            catch (Exception)
            {
                GUI.color = Color.red;
            }

            EditorGUI.PropertyField(position, property, label);
            GUI.color = startColor;
        }
    }
#endif
}