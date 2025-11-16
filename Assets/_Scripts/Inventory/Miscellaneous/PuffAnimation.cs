using UnityEngine;

public class PuffAnimation : MonoBehaviour
{
    [SerializeField] private GameObject puffPrefab;

    private void OnDisable()
    {
        if (puffPrefab == null)
        {
            Debug.LogWarning($"No se asignó un prefab para el Puff en [{gameObject.name}]");
            return;
        }

        GameObject instancia = Instantiate(puffPrefab, transform.position, transform.rotation);

        Animator animator = instancia.GetComponent<Animator>();

        if (animator != null)
        {
            // Obtenemos la duración del clip actual
            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                float duracion = clips[0].clip.length;
                Destroy(instancia, duracion);
            }
            else
            {
                Debug.LogWarning("El Animator no tiene un clip por defecto. Se destruye en 1 segundo.");
                Destroy(instancia, 1f);
            }
        }
        else
        {
            Debug.LogWarning("El prefab no tiene Animator. Se destruye en 1 segundo.");
            Destroy(instancia, 1f);
        }
    }
}
