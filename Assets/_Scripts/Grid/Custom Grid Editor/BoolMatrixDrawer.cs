using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomBoolMatrix))]
public class CustomBoolMatrixDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int rows = Mathf.Max(1, property.FindPropertyRelative("rows").intValue);
        // Calcula altura total: título + inputs + botones + grid + padding
        float baseHeight = EditorGUIUtility.singleLineHeight * 3f; // título + filas/columnas + botones
        float gridHeight = rows * 22f + 10f;
        return baseHeight + gridHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty rowsProp = property.FindPropertyRelative("rows");
        SerializedProperty colsProp = property.FindPropertyRelative("columns");
        SerializedProperty matrixProp = property.FindPropertyRelative("matrix");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float y = position.y;

        // --- Etiqueta principal ---
        EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), label, EditorStyles.boldLabel);
        y += lineHeight + 2;

        // --- Campos de filas y columnas ---
        Rect rowRect = new Rect(position.x, y, position.width / 2 - 4, lineHeight);
        Rect colRect = new Rect(position.x + position.width / 2 + 2, y, position.width / 2 - 4, lineHeight);
        EditorGUI.PropertyField(rowRect, rowsProp, new GUIContent("Filas"));
        EditorGUI.PropertyField(colRect, colsProp, new GUIContent("Columnas"));
        y += lineHeight + 4;

        int rows = Mathf.Max(1, rowsProp.intValue);
        int cols = Mathf.Max(1, colsProp.intValue);

        // --- Botones ---
        float buttonWidth = (position.width - 10) / 3f;
        Rect clearRect = new Rect(position.x, y, buttonWidth, lineHeight);
        Rect invertRect = new Rect(position.x + buttonWidth + 5, y, buttonWidth, lineHeight);
        Rect randomRect = new Rect(position.x + 2 * (buttonWidth + 5), y, buttonWidth, lineHeight);

        if (GUI.Button(clearRect, "Limpiar"))
        {
            for (int i = 0; i < matrixProp.arraySize; i++)
            {
                var row = matrixProp.GetArrayElementAtIndex(i).FindPropertyRelative("values");
                for (int j = 0; j < row.arraySize; j++)
                    row.GetArrayElementAtIndex(j).boolValue = false;
            }
        }

        if (GUI.Button(invertRect, "Invertir"))
        {
            for (int i = 0; i < matrixProp.arraySize; i++)
            {
                var row = matrixProp.GetArrayElementAtIndex(i).FindPropertyRelative("values");
                for (int j = 0; j < row.arraySize; j++)
                    row.GetArrayElementAtIndex(j).boolValue = !row.GetArrayElementAtIndex(j).boolValue;
            }
        }

        if (GUI.Button(randomRect, "Aleatorio"))
        {
            for (int i = 0; i < matrixProp.arraySize; i++)
            {
                var row = matrixProp.GetArrayElementAtIndex(i).FindPropertyRelative("values");
                for (int j = 0; j < row.arraySize; j++)
                    row.GetArrayElementAtIndex(j).boolValue = Random.value > 0.5f;
            }
        }

        y += lineHeight + 6;

        // --- Dibujar la cuadrícula ---
        float toggleSize = 20f;
        float totalGridWidth = cols * (toggleSize + 2);
        float startX = position.x + (position.width - totalGridWidth) * 0.5f; // centrado horizontalmente

        for (int i = 0; i < rows; i++)
        {
            var rowProp = matrixProp.GetArrayElementAtIndex(i).FindPropertyRelative("values");
            for (int j = 0; j < cols; j++)
            {
                Rect toggleRect = new Rect(
                    startX + j * (toggleSize + 2),
                    y + i * (toggleSize + 2),
                    toggleSize,
                    toggleSize
                );
                SerializedProperty cell = rowProp.GetArrayElementAtIndex(j);
                cell.boolValue = EditorGUI.Toggle(toggleRect, cell.boolValue);
            }
        }

        EditorGUI.EndProperty();
    }
}
