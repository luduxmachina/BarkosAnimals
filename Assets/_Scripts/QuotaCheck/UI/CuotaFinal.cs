using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CuotaFinal : MonoBehaviour, QuotaUiInterface
{
    [SerializeField]bool mostrarQuotaNecesaria;
    [SerializeField, HideIf("mostrarQuotaNecesaria", false)] TextMeshProUGUI textoQuota;


    [SerializeField]TextMeshProUGUI textoQuotaPassed;
    [SerializeField] TextMeshProUGUI textoQuotaFailed;


    [SerializeField] UnityEvent OnQuotaPassed = new();

    int _quota = 0;
    int Quota
    {
        get { return _quota; }
        set { 
            _quota = value; 
            if(mostrarQuotaNecesaria)textoQuota.text = _quota.ToString();
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
        GameFlowManager.instance.quotaChecker.AddNewUI(this);
    }
    public void UpdateQuotaPassed(Quota quotaPassed, bool isQuotaPassed)
    {
        QuotaPassed = quotaPassed.QuotaValue;
        if (!mostrarQuotaNecesaria)Quota = QuotaPassed;

        if (QuotaPassed <= 0)
        {
            textoQuotaFailed.text = $"No has alcanzado la cuota te has quedado a: {(Quota - QuotaPassed).ToString()}";
        }
        else
        {
            textoQuotaFailed.text = "¡Lo has conseguido!";
            OnQuotaPassed.Invoke();
        }
    }
}
