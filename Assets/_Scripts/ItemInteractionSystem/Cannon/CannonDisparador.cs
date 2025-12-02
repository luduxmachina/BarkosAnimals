using UnityEngine;

public class CannonDisparador : MonoBehaviour
{
    [SerializeField] private LazyDetector detector;
    public void Fire()
    {
        GameObject[] targets= detector.GetAllTargets();
        foreach (GameObject target in targets)
        {
            Suciedad suciedad = target.GetComponentInChildren<Suciedad>();
            if (suciedad != null)
            {
                suciedad.Limpiar();
                return;
            }
            AAnimalFase2 animal = target.GetComponentInChildren<AAnimalFase2>();
            if (animal != null)
            {
                animal.Limpiarse();
                return;
            }
        }
    }
}
