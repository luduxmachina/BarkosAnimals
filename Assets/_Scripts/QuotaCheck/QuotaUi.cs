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

    Quota quota;

    public int height = 30;

    [SerializeField]
    List<Image> imageList = new List<Image>();

    private void Start()
    {
        textoQuota = GetComponent<TextMeshProUGUI>();
        SetQuota();
        quota = new Quota(1);
    }

    void SetQuota()
    {
        //quota = gameFlowManager.currentQuotaInfo;
        int numImage = 0;
        foreach (Restriction restriction in Enum.GetValues(typeof(Restriction)))
        {
            if (quota.Restrictions[restriction] > 0)
            {
                //images[restriction].GetComponent<GameObject>().SetActive(true);
                //images[restriction].GetComponent<RectTransform>().anchoredPosition -= new Vector2 (0, numImage * height);
                //numImage++;
            }
        }        

    }
}
