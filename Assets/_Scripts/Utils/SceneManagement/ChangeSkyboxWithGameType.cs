using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using RenderSettings = UnityEngine.RenderSettings;

public class ChangeSkyboxWithGameType : MonoBehaviour
{
    [SerializeField]
    private Light directionalLight;
    [SerializeField]
    private Material hardModeSkybox;
    [SerializeField]
    private Color lightColor;
    [SerializeField]
    private float lightIntensity;
    [SerializeField]
    private List<GameObject> objectosToEnableOnHardMode;
    
    GameFlowManager gameFlowManager =  GameFlowManager.instance;

    private void Awake()
    {
        
        if (gameFlowManager.currentLevel.nombreNivel == "Desafío")
        {
            RenderSettings.skybox = hardModeSkybox;
            directionalLight.color = lightColor;
            directionalLight.intensity = lightIntensity;
            
            foreach (GameObject obj in objectosToEnableOnHardMode)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject obj in objectosToEnableOnHardMode)
            {
                obj.SetActive(false);
            }
        }
    }
}
