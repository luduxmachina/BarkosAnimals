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
    public bool useDefaultSelectionPhaseScene = true;
    public bool useDefaultIslands = false;
    public bool useDefaultBoatPhaseScene = true;
    public bool useDefaultOrganizationPhaseScene = true;
    public bool useDefaultQuotaScene = true;

    [SerializeField, CustomLabel("", true), HideIf("useDefaultIslands")]
    public List<Archipelago> archipelagos;
#if UNITY_EDITOR
[SerializeField, HideIf("useDefaultSelectionPhaseScene")]
    private SceneAsset SelectionPhaseScene; //por si son diferentes en cada uno
    [SerializeField, HideIf("useDefaultOrganizationPhaseScene")]
    private SceneAsset OrganizationPhaseScene; //por si son diferentes en cada uno
    [SerializeField, HideIf("useDefaultBoatPhaseScene")]
    private SceneAsset BoatPhaseScene;
    [SerializeField, HideIf("useDefaultBoatPhaseScene")]
    private SceneAsset QuotaScene;
#endif
    [HideInInspector]
    public int selectionPhaseSceneIndex;
    [HideInInspector]
    public int organizationPhaseSceneIndex;
    [HideInInspector]
    public int boatPhaseSceneIndex;
    [HideInInspector]
    public int quotaSceneIndex;
    public int numberOfArchipelagos = 0;
    [Space]
    public QuotaInfo quotaInfo;

#if UNITY_EDITOR
    private void SyncData()
    {
        foreach (var archipelago in archipelagos) //pasar las cosas de la lista del editor a la de verdad
        {

            if(archipelago == null) continue;
            archipelago.SyncData();

            EditorUtility.SetDirty(this);


        }
        selectionPhaseSceneIndex = ForceGetIndexOf(AssetDatabase.GetAssetPath(SelectionPhaseScene));
        organizationPhaseSceneIndex = ForceGetIndexOf(AssetDatabase.GetAssetPath(OrganizationPhaseScene));
        boatPhaseSceneIndex = ForceGetIndexOf(AssetDatabase.GetAssetPath(BoatPhaseScene));
        quotaSceneIndex = ForceGetIndexOf(AssetDatabase.GetAssetPath(QuotaScene));
        if(!useDefaultIslands){

            numberOfArchipelagos = archipelagos.Count;
            
        }
     
        

      

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
    public void OnValidate()
    {
        SyncData();
    }
#endif
}
#if UNITY_EDITOR
public class BuildPreprocessor : IPreprocessBuildWithReport //espero que esto funcione, total, es para que no de errores de tener las listas de los niveles desactualizados
{
    // Prioridad del callback (cuanto menor, antes se ejecuta)
    public int callbackOrder => 0;

    public  void OnPreprocessBuild(BuildReport report)
    {
        // Busca todos los ScriptableObjects del tipo MyScriptableClass en el proyecto
        string[] guids = AssetDatabase.FindAssets("t:MyScriptableClass");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            NivelSO obj = AssetDatabase.LoadAssetAtPath<NivelSO>(path);

            if (obj == null)
                continue;

            // Llama a la función que quieras ejecutar en cada uno
            obj.OnValidate();

            // Marca el objeto como modificado
            EditorUtility.SetDirty(obj);
        }

        // Guarda todos los cambios
        AssetDatabase.SaveAssets();
        Debug.Log("Todos los MyScriptableClass han sido actualizados con MyFunc().");
    }
}
[InitializeOnLoad]
public static class ScriptReloadHandler
{
    static ScriptReloadHandler()
    { // Busca todos los ScriptableObjects del tipo MyScriptableClass en el proyecto
        string[] guids = AssetDatabase.FindAssets("t:" + nameof(NivelSO));

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            NivelSO obj = AssetDatabase.LoadAssetAtPath<NivelSO>(path);

            if (obj == null)
                continue;

            // Llama a la función que quieras ejecutar en cada uno
            obj.OnValidate();

            // Marca el objeto como modificado
            EditorUtility.SetDirty(obj);
        }

        // Guarda todos los cambios
        AssetDatabase.SaveAssets();
       // Debug.Log("Todos los MyScriptableClass han sido actualizados con MyFunc().");
    }
}
#endif