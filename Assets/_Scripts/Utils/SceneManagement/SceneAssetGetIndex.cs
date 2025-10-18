#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAssetGetIndex 
{
    public static int ForceGetIndexOf(string scenePath)
    {
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        if (buildIndex < 0)  //si la escena no esta en la lista se a�ade
        {

            var newScene = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;

            var updatedScenes = new EditorBuildSettingsScene[existingScenes.Length + 1];
            existingScenes.CopyTo(updatedScenes, 0);
            updatedScenes[existingScenes.Length] = newScene;
            EditorBuildSettings.scenes = updatedScenes;
            buildIndex = existingScenes.Length; // El nuevo �ndice ser� el �ltimo
        }
        return buildIndex;
    }
}
#endif