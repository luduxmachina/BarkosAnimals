
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[Serializable]
public struct RestrictionUI
{
    public Restriction restriction;
    public TextMeshProUGUI textPass;
    public TextMeshProUGUI textNeeded;
    public Image image;
    public bool isPassed;
}
public class QuotaUi : MonoBehaviour, QuotaUiInterface, ISerializationCallbackReceiver
{
    GameFlowManager gameFlowManager;

    [SerializeField]
    TextMeshProUGUI textoQuota;

    TextMeshProUGUI[] texts;

    string cuotaPassText = "___";
    string cuotaText = "___";

    Quota quota;

    public int height = 30;

    [SerializeField]
    List<RestrictionUI> restrictionUIs = new List<RestrictionUI>();

    Dictionary<Restriction, int> restictions = new Dictionary<Restriction, int>();

    private void Start()
    {
        //TextMeshProUGUI[] textos = GetComponentsInChildren<TextMeshProUGUI>(true);
        //int i = 0;
        //foreach (TextMeshProUGUI text in texts)
        //{
        //    if (text.text == ""&& i == 1)
        //    {
        //        textoQuota = text;
        //    }
        //    else
        //    {
        //        texts[i-2] = text;
        //    }
        //
        //        i++;
        //}

        SetQuota();
        GameFlowManager.instance.quotaChecker.AddNewUI(this);
    }

    void SetQuota()
    {
        quota = GameFlowManager.instance.quotaChecker.GetQuota();
        int numImage = 0;
        //if (quota.Restrictions[Restriction.Herbivore] > 0)
        //{
        //    imageHerbivore.gameObject.SetActive(true);
        //    imageHerbivore.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, numImage * height);
        //    numImage++;
        //}

        cuotaText = quota.QuotaValue.ToString();

        textoQuota.text = $"{cuotaPassText}/{cuotaText}";
        restictions.Clear();
        restictions = quota.Restrictions;

        foreach (var restrictionUI in restrictionUIs)
        {
            if(restictions[restrictionUI.restriction] > 0)
            {
                restrictionUI.image.gameObject.SetActive(true);
                restrictionUI.image.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, numImage * height);

                restrictionUI.textPass.gameObject.SetActive(true);
                restrictionUI.textNeeded.gameObject.SetActive(true);
                restrictionUI.textPass.text = "0/";
                restrictionUI.textNeeded.text = restictions[restrictionUI.restriction].ToString();
                restrictionUI.textPass.gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, numImage * height);
                restrictionUI.textNeeded.gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, numImage * height);
                numImage++;

            }

        }
    }

    public void UpdateQuotaPassed(Quota quotaPassed, bool isQuotaPassed)
    {
        this.cuotaText = quota.QuotaValue.ToString();
        textoQuota.text = $"{quotaPassed.QuotaValue.ToString()}/{cuotaText}";
        if (isQuotaPassed)
        {
            textoQuota.color = Color.green;
        }
        else
        {
            textoQuota.color = Color.black;
        }

        foreach (var restrictionUI in restrictionUIs)
        {
            if (restictions[restrictionUI.restriction] > 0)
            {
                restrictionUI.textNeeded.gameObject.SetActive(true);
                restrictionUI.textPass.text = quotaPassed.Restrictions[restrictionUI.restriction].ToString()+"/";
                if (restictions[restrictionUI.restriction] <= quotaPassed.Restrictions[restrictionUI.restriction])
                {
                    restrictionUI.textPass.color = Color.green;
                }
                else
                {
                    restrictionUI.textPass.color = Color.black;
                }
            }
        }
    }

    public void OnBeforeSerialize()
    {
        SincronizarConEnum();
    }


    private void SincronizarConEnum()
    {
        var tipos = (Restriction[])Enum.GetValues(typeof(Restriction));

        // Agregar los tipos que falten
        foreach (var t in tipos)
        {
            if (!restrictionUIs.Exists(x => x.restriction == t))
                restrictionUIs.Add(new RestrictionUI { restriction = t, isPassed = false});
        }

        // Eliminar los que ya no existen en el enum
        restrictionUIs.RemoveAll(x => Array.IndexOf(tipos, x.restriction) == -1);
    }

    public void OnAfterDeserialize()
    {
    }
}
