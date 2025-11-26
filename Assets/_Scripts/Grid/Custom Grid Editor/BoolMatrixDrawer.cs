#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

[CustomPropertyDrawer(typeof(CustomBoolMatrix))]
public class CustomBoolMatrixDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int rows = Mathf.Max(1, property.FindPropertyRelative("rows").intValue);
        // Calcula altura total: t�tulo + inputs + botones + grid + padding
        float baseHeight = EditorGUIUtility.singleLineHeight * 3f; // t�tulo + filas/columnas + botones
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

        // --- Asegurar que la matriz exista y tenga el tama�o correcto ---
        int rows = Mathf.Max(1, rowsProp.intValue);
        int cols = Mathf.Max(1, colsProp.intValue);

        if (matrixProp.arraySize != rows)
        {
            matrixProp.arraySize = rows;
        }

        for (int i = 0; i < rows; i++)
        {
            var row = matrixProp.GetArrayElementAtIndex(i);
            var valuesProp = row.FindPropertyRelative("values");

            if (valuesProp == null)
                continue; // Evita NPE si algo no est� serializado a�n

            if (valuesProp.arraySize != cols)
                valuesProp.arraySize = cols;
        }

        // --- Botones ---
        float buttonWidth = (position.width - 10) / 3f;
        Rect clearRect = new Rect(position.x, y, buttonWidth, lineHeight);
        Rect invertRect = new Rect(position.x + buttonWidth + 5, y, buttonWidth, lineHeight);
        Rect rotateRect = new Rect(position.x + 2 * (buttonWidth + 5), y, buttonWidth, lineHeight);

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

        if (GUI.Button(rotateRect, "Rotar 90º"))
        {
            CustomBoolMatrix matrixObj = GetTargetObjectOfProperty(property) as CustomBoolMatrix;
            if (matrixObj != null)
            {
                matrixObj.Rotate90();
                matrixObj.EnsureSize();
                // marcaremos el ScriptableObject raíz como dirty
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            }

        }


        
        y += lineHeight + 6;

        // --- Dibujar la cuadr�cula ---
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
    
    private static object GetTargetObjectOfProperty(SerializedProperty prop)
{
    if (prop == null) return null;

    // Convert "Array.data[x]" -> "[x]" para simplificar el parsing
    string path = prop.propertyPath.Replace(".Array.data[", "[");
    object obj = prop.serializedObject.targetObject;
    string[] elements = path.Split('.');

    foreach (string element in elements)
    {
        if (element.Contains("["))
        {
            // ejemplo: PlaceableObjectData[3]
            string elementName = element.Substring(0, element.IndexOf("["));
            string indexString = element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", "");
            int index = Convert.ToInt32(indexString);

            object listObj = GetValue(obj, elementName);
            obj = GetIndexedValue(listObj, index);
        }
        else
        {
            obj = GetValue(obj, element);
        }

        if (obj == null) return null;
    }

    return obj;
}

private static object GetValue(object source, string name)
{
    if (source == null) return null;

    Type type = source.GetType();
    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    // 1) Intentar Property (getter)
    PropertyInfo p = type.GetProperty(name, flags);
    if (p != null)
    {
        try { return p.GetValue(source, null); }
        catch { /* ignore and fallthrough */ }
    }

    // 2) Intentar campo con nombre exacto
    FieldInfo f = type.GetField(name, flags);
    if (f != null) return f.GetValue(source);

    // 3) Intentar backing field de auto-property solo si no lo era ya
    if (name.StartsWith("<") && name.EndsWith(">k__BackingField"))
    {
        // El nombre YA es el backing field generado por C#
        FieldInfo bfExisting = type.GetField(name, flags);
        if (bfExisting != null) return bfExisting.GetValue(source);
    }
    else
    {
        // Construir backing field normal
        string backingName = $"<{name}>k__BackingField";
        FieldInfo bf = type.GetField(backingName, flags);
        if (bf != null) return bf.GetValue(source);
    }

    // 4) Si es un IDictionary o tiene un indexer, intentar indexer por nombre (opcional)
    // (no implementado aquí salvo que lo necesites)

    return null;
}

private static object GetIndexedValue(object listObj, int index)
{
    if (listObj == null) return null;

    // Arrays
    Array arr = listObj as Array;
    if (arr != null)
    {
        if (index >= 0 && index < arr.Length) return arr.GetValue(index);
        return null;
    }

    // IList (List<T>, etc.)
    IList il = listObj as IList;
    if (il != null)
    {
        if (index >= 0 && index < il.Count) return il[index];
        return null;
    }

    // IReadOnlyList<T>
    var roType = listObj.GetType().GetInterface("System.Collections.Generic.IReadOnlyList`1");
    if (roType != null)
    {
        var prop = listObj.GetType().GetProperty("Item");
        if (prop != null)
        {
            try { return prop.GetValue(listObj, new object[] { index }); }
            catch { return null; }
        }
    }

    // IEnumerable fallback: recorrer hasta la posición
    var enumerable = listObj as IEnumerable;
    if (enumerable != null)
    {
        int i = 0;
        foreach (var it in enumerable)
        {
            if (i == index) return it;
            i++;
        }
    }

    return null;
}

}


#endif