#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using System;
[Serializable]
public struct QuotaInfo
{
    [SerializeField]
    public int totalQuota; 

}
[CreateAssetMenu(fileName = "Nivel", menuName = "ScriptableObjects/NivelSO", order = 1)]
public class NivelSO : ScriptableObject
{
    public string nombreNivel;
    [Space]
    public bool useDefaultBoatPhaseScene = true;
    public bool useDefaultOrganizationPhaseScene = true;
    public bool useDefaultIslands = false;
#if UNITY_EDITOR
    [HideIf("useDefaultIslands")]
    public List<SceneAsset> islands;
    [HideIf("useDefaultOrganizationPhaseScene")]
    public SceneAsset OrganizationPhaseScene; //por si son diferentes en cada uno
    [HideIf("useDefaultBoatPhaseScene")]
    public SceneAsset BoatPhaseScene;
#endif
    [HideInInspector]
    public int[] islandIndexes;
    [HideInInspector]
    public int organizationPhaseSceneIndex;
    [HideInInspector]
    public int boatPhaseSceneIndex;
    public int numberOfIslands = 0;
    [Space]
    public QuotaInfo quotaInfo;

#if UNITY_EDITOR
    private void SyncLists()
    {
        List<int> temp = new List<int>();
        foreach (var scene in islands) //pasar las cosas de la lista del editor a la de verdad
        {

            if(scene == null) continue;
            string scenePath = AssetDatabase.GetAssetPath(scene);
            int buildIndex = ForceGetIndexOf(scenePath);
            
            temp.Add(buildIndex);
            EditorUtility.SetDirty(this);


        }
        
        organizationPhaseSceneIndex = ForceGetIndexOf(AssetDatabase.GetAssetPath(OrganizationPhaseScene));
        boatPhaseSceneIndex = ForceGetIndexOf(AssetDatabase.GetAssetPath(BoatPhaseScene));
        islandIndexes = temp.ToArray();
        if(!useDefaultIslands){ //si no usa las por defecto, la lista son las que tiene

            numberOfIslands = temp.Count;
            
        }
        

      

    }
    public int ForceGetIndexOf(string scenePath){
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
    private void OnValidate()
    {
        SyncLists();
    }
#endif
}
