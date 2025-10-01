
#if UNITY_EDITOR

using UnityEditor;
#endif
using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute { }
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
internal class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Desactiva la edición
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true; // Vuelve a activar para otros campos
    }
}
#endif