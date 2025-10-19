using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuotaUi : MonoBehaviour, QuotaUiInterface
{
    GameFlowManager gameFlowManager;

    TextMeshProUGUI textoQuota;

    TextMeshProUGUI[] texts;

    string cuotaPassText = "___";
    string cuotaText = "___";

    Quota quota;

    public int height = 30;

    [SerializeField]
    //List<Image> imageList = new List<Image>();
    Image imageHerbivore;

    private void Start()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI text in texts)
        {
            if (text.text == "")
            {
                textoQuota = text;
                break;
            }
        }

        SetQuota();
        GameFlowManager.instance.quotaChecker.AddNewUI(this);
    }

    void SetQuota()
    {
        quota = GameFlowManager.instance.quotaChecker.GetQuota();
        int numImage = 0;
        if (quota.Restrictions[Restriction.Herbivore] > 0)
        {
            imageHerbivore.gameObject.SetActive(true);
            imageHerbivore.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, numImage * height);
            numImage++;
        }

        cuotaText = quota.QuotaValue.ToString();

        textoQuota.text = $"{cuotaPassText}/{cuotaText}";
    }

    public void UpdateQuotaPassed(Quota quotaPassed, bool isQuotaPassed)
    {
        this.cuotaPassText = quota.QuotaValue.ToString();
        textoQuota.text = $"{cuotaPassText}/{cuotaText}";
        if (isQuotaPassed)
        {
            textoQuota.color = Color.green;
        }
        else
        {
            textoQuota.color = Color.black;
        }
    }
}
