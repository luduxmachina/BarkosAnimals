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
    [HideInInspector]
    public int numberOfIslands = 0;
#if UNITY_EDITOR
    public void SyncData()
    {
        numberOfIslands = 0;
        foreach (var island in islands)
        {
            if(island == null) return; //si hay una nulo, los siguietnes serán nulos, y si no se ignora por inutiles
            numberOfIslands++;
            island.SyncData();
        }
    }
#endif

}
[Serializable]
public struct PreviewInfo
{
    public Sprite previewImage;
    public Sprite[] animalesDeLaIsla;
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
        islandSceneIndex = SceneAssetGetIndex.ForceGetIndexOf(AssetDatabase.GetAssetPath(IslandScene));
            EditorUtility.SetDirty(this);


    }


#endif

}
