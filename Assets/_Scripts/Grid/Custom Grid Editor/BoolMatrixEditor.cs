using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestTamañosSO))]
public class BoolMatrixEditor : Editor
{
    // private SerializedProperty matricesProp;
    // 
    // private void OnEnable()
    // {
    //     matricesProp = serializedObject.FindProperty("matrices");
    // }
    // 
    // public override void OnInspectorGUI()
    // {
    //     serializedObject.Update();
    //     TestTamañosSO collection = (TestTamañosSO)target;
    // 
    //     EditorGUILayout.LabelField("Matrices booleanas", EditorStyles.boldLabel);
    //     EditorGUILayout.Space();
    // 
    //     // Botón para agregar nueva matriz
    //     if (GUILayout.Button("Agregar nueva matriz"))
    //     {
    //         collection.ListOfMatrix.Add(new CustomBoolMatrix());
    //     }
    // 
    //     // Dibujar cada matriz
    //     for (int i = 0; i < collection.ListOfMatrix.Count; i++)
    //     {
    //         var matrix = collection.ListOfMatrix[i];
    //         EditorGUILayout.BeginVertical("box");
    //         EditorGUILayout.LabelField($"Matriz {i}", EditorStyles.boldLabel);
    // 
    //         matrix.rows = EditorGUILayout.IntField("Filas", matrix.rows);
    //         matrix.columns = EditorGUILayout.IntField("Columnas", matrix.columns);
    //         matrix.EnsureSize();
    // 
    //         EditorGUILayout.Space(5);
    //         for (int r = 0; r < matrix.rows; r++)
    //         {
    //             GUILayout.BeginHorizontal();
    //             for (int c = 0; c < matrix.columns; c++)
    //             {
    //                 matrix.matrix[r].values[c] = GUILayout.Toggle(
    //                     matrix.matrix[r].values[c],
    //                     GUIContent.none,
    //                     GUILayout.Width(20),
    //                     GUILayout.Height(20)
    //                 );
    //             }
    //             GUILayout.EndHorizontal();
    //         }
    // 
    //         EditorGUILayout.Space(5);
    //         if (GUILayout.Button("Eliminar esta matriz"))
    //         {
    //             collection.ListOfMatrix.RemoveAt(i);
    //             break; // salir del bucle porque la lista cambió
    //         }
    // 
    //         EditorGUILayout.EndVertical();
    //         EditorGUILayout.Space(5);
    //     }
    // 
    //     if (GUI.changed)
    //     {
    //         EditorUtility.SetDirty(collection);
    //     }
    // 
    //     serializedObject.ApplyModifiedProperties();
    // }
}
