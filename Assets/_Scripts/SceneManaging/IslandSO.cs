#if UNITY_EDITOR
using UnityEditor;

using UnityEditor.Build.Reporting;
using UnityEditor.Build;
#endif
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
[Serializable]
public class Archipelago
{
    [CustomLabel("")]
    public IslandSO[] islands= new IslandSO[3];
#if UNITY_EDITOR
    public void SyncData()
    {
        foreach (var island in islands)
        {
            if(island == null) continue;
            island.SyncData();
        }
    }
#endif

}
[Serializable]
public struct PreviewInfo
{
    public Sprite previewImage;
}
[CreateAssetMenu(fileName = "Island", menuName = "ScriptableObjects/IslandSO", order = 1)]
public class IslandSO : ScriptableObject
{
#if UNITY_EDITOR
[SerializeField]
    private SceneAsset IslandScene; //por si son diferentes en cada uno
    
#endif
    [HideInInspector]
    public int islandSceneIndex;
    public string islandName;
    public PreviewInfo previewInfo;
#if UNITY_EDITOR

    public void SyncData()
    {
        islandSceneIndex = ForceGetIndexOf(AssetDatabase.GetAssetPath(IslandScene));
            EditorUtility.SetDirty(this);


    }

    public int ForceGetIndexOf(string scenePath)
    {
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        if (buildIndex < 0)  //si la escena no esta en la lista se añade
        {

            var newScene = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;

            var updatedScenes = new EditorBuildSettingsScene[existingScenes.Length + 1];
            existingScenes.CopyTo(updatedScenes, 0);
            updatedScenes[existingScenes.Length] = newScene;
            EditorBuildSettings.scenes = updatedScenes;
            buildIndex = existingScenes.Length; // El nuevo índice será el último
        }
        return buildIndex;
    }

#endif

}
