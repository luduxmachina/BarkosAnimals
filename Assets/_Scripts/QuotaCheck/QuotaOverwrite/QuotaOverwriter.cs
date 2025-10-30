using UnityEngine;
using UnityEngine.UI;

public class QuotaOverwriter : MonoBehaviour, QuotaUiInterface
{
    private Quota quotaPassed;
    [SerializeField] GameObject prefabUiAdButton;
    [SerializeField] GameObject prefabUiPassedIndicator;
    [SerializeField] Transform buttonsParent;
   

    public void Start()
    {
        GameFlowManager.instance.quotaChecker.AddNewUI(this);
    }

    public void UpdateQuotaPassed(Quota quotaPassed, bool isQuotaPassed)
    {
        this.quotaPassed = quotaPassed;
        if (isQuotaPassed)
        {
            gameObject.SetActive(false);
            return;
        }
        initButtons();

    }
    private void initButtons()
    {
        //voy a fucking destruir todos los hijos para que no me den por culo y encima se actualice bien
        foreach (Transform child in buttonsParent)
        {
            Destroy(child.gameObject);
        }
        //valor cuota
        if (quotaPassed.QuotaValue < GameFlowManager.instance.quotaChecker.GetQuota().QuotaValue)
        {
            GameObject buttonGO = Instantiate(prefabUiAdButton, buttonsParent);
            Button button = buttonGO.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => ForcePassQuotaValue());
        }
        else
        {
            Instantiate(prefabUiPassedIndicator, buttonsParent);
        }
        //las restricciones
        foreach (var restriction in GameFlowManager.instance.quotaChecker.GetQuota().Restrictions)
        {
            if (quotaPassed.Restrictions[restriction.Key] < restriction.Value && restriction.Value > 0) //hay restricciones pero no la pasa
            {
                instantiateButton(restriction.Key);
            }
            else //la habia pasado
            {
                Instantiate(prefabUiPassedIndicator, buttonsParent);
            }
        }
    }
    private void instantiateButton(Restriction restriction)
    {
        GameObject buttonGO = Instantiate(prefabUiAdButton, buttonsParent);
        Button button = buttonGO.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => ForcePassQuotaRestriccion(restriction));
    }

    public void ForcePassQuotaValue()
    {
        PlayAd();
        int pointsNeeded = GameFlowManager.instance.quotaChecker.GetQuota().QuotaValue - quotaPassed.QuotaValue;
        quotaPassed.AddPoints(pointsNeeded);
        GameFlowManager.instance.quotaChecker.UpdateCuote(AnimalType.Duck, 0); //forzar a que se recalcule la cuota
    }
    public void ForcePassQuotaRestriccion(Restriction restriction) // ni idea de que cojones poner lol
    {
        PlayAd();
        int pointsNeeded = GameFlowManager.instance.quotaChecker.GetQuota().Restrictions[restriction] - quotaPassed.Restrictions[restriction];
        quotaPassed.AddRestictionPassed(restriction, pointsNeeded);
        GameFlowManager.instance.quotaChecker.UpdateCuote(AnimalType.Duck, 0); //lets go
    }
    private void PlayAd()
    {
        //hee hee
    }
}
