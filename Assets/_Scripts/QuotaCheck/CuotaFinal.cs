using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CuotaFinal : MonoBehaviour, QuotaUiInterface
{
    [SerializeField]TextMeshProUGUI textoQuota;
    [SerializeField]TextMeshProUGUI textoQuotaPassed;
    [SerializeField] TextMeshProUGUI textoQuotaFailed;

    int _quota = 0;
    int Quota
    {
        get { return _quota; }
        set { 
            _quota = value; 
            textoQuota.text = _quota.ToString();
        }
    }

    int _quotaPassed = 0;
    int QuotaPassed
    {
        get { return _quotaPassed; }
        set
        {
            _quotaPassed = value;
            textoQuotaPassed.text = _quotaPassed.ToString();
        }
    }

    public bool IsQuotaPassed()
    {
        return Quota <= QuotaPassed;
    }

    public void AutoPassQuota()
    {
        QuotaPassed = Quota;
        //Se puede hacer que se actualize el manager.
    }


    private void Start()
    {
        Quota = GameFlowManager.instance.quotaChecker.GetQuota().QuotaValue;
    }
    public void UpdateQuotaPassed(Quota quotaPassed, bool isQuotaPassed)
    {
        QuotaPassed = quotaPassed.QuotaValue;

        if (QuotaPassed < Quota)
        {
            textoQuotaFailed.text = $"No has alcanzado la cuota te has quedado a: {(Quota - QuotaPassed).ToString()}";
        }
        else
        {
            textoQuotaFailed.text = "¡Lo has conseguido!";
        }
    }
}
