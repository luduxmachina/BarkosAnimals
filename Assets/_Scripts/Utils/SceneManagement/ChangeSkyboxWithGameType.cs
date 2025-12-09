using System;
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
    
    GameFlowManager gameFlowManager =  GameFlowManager.instance;

    private void Awake()
    {
        if (gameFlowManager.currentLevel.nombreNivel == "Desafío")
        {
            RenderSettings.skybox = hardModeSkybox;
            directionalLight.color = lightColor;
            directionalLight.intensity = lightIntensity;
        }
    }
}
