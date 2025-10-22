using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomBoolMatrix))]
public class BoolMatrixDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int rows = Mathf.Max(1, property.FindPropertyRelative("rows").intValue);
        return 60 + rows * 22;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var rowsProp = property.FindPropertyRelative("rows");
        var columnsProp = property.FindPropertyRelative("columns");
        var matrixProp = property.FindPropertyRelative("matrix");

        Rect rect = position;
        rect.height = EditorGUIUtility.singleLineHeight;

        // Etiqueta principal
        EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);
        rect.y += rect.height + 2;

        EditorGUI.indentLevel++;
        rowsProp.intValue = Mathf.Max(1, EditorGUI.IntField(new Rect(rect.x, rect.y, rect.width / 2 - 2, rect.height), "Filas", rowsProp.intValue));
        columnsProp.intValue = Mathf.Max(1, EditorGUI.IntField(new Rect(rect.x + rect.width / 2 + 2, rect.y, rect.width / 2 - 2, rect.height), "Columnas", columnsProp.intValue));
        rect.y += rect.height + 4;

        // Asegurar la estructura
        EnsureMatrixSize(property);

        // Dibujar la cuadrícula
        for (int i = 0; i < rowsProp.intValue; i++)
        {
            var rowProp = matrixProp.GetArrayElementAtIndex(i).FindPropertyRelative("values");

            GUILayout.BeginHorizontal();
            float toggleSize = 20;
            float totalWidth = columnsProp.intValue * toggleSize + (columnsProp.intValue - 1) * 2;
            float startX = rect.x + (rect.width - totalWidth) / 2;

            for (int j = 0; j < columnsProp.intValue; j++)
            {
                var cellProp = rowProp.GetArrayElementAtIndex(j);
                Rect toggleRect = new Rect(startX + j * (toggleSize + 2), rect.y, toggleSize, toggleSize);
                cellProp.boolValue = EditorGUI.Toggle(toggleRect, cellProp.boolValue);
            }

            rect.y += toggleSize + 2;
            GUILayout.EndHorizontal();
        }

        EditorGUI.EndProperty();
    }

    private void EnsureMatrixSize(SerializedProperty property)
    {
        var rowsProp = property.FindPropertyRelative("rows");
        var colsProp = property.FindPropertyRelative("columns");
        var matrixProp = property.FindPropertyRelative("matrix");

        int rows = Mathf.Max(1, rowsProp.intValue);
        int cols = Mathf.Max(1, colsProp.intValue);

        if (matrixProp.arraySize != rows)
            matrixProp.arraySize = rows;

        for (int i = 0; i < rows; i++)
        {
            var rowProp = matrixProp.GetArrayElementAtIndex(i).FindPropertyRelative("values");
            if (rowProp.arraySize != cols)
                rowProp.arraySize = cols;
        }
    }
}
