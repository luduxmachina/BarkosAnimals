using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuotaUi : MonoBehaviour
{
    GameFlowManager gameFlowManager;

    TextMeshProUGUI textoQuota;

    string texto = "___0/___0";

    Quota quota;

    public int height = 30;

    [SerializeField]
    //List<Image> imageList = new List<Image>();
    Image imageHerbivore;

    private void Start()
    {
        textoQuota = GetComponent<TextMeshProUGUI>();
        SetQuota();
    }

    void SetQuota()
    {
        //quota = GameFlowManager.instance;
        int numImage = 0;
        if (quota.Restrictions[Restriction.Herbivore] > 0)
        {
            imageHerbivore.GetComponent<GameObject>().SetActive(true);
            imageHerbivore.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, numImage * height);
            numImage++;
        }

        texto = $"___0/{quota.QuotaValue}";

    }


}
