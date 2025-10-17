using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
#endif
public class ShowParentPivot : MonoBehaviour
{

#if UNITY_EDITOR
    public bool show = true; // Puedes activarlo/desactivarlo desde el Inspector
    [SerializeField]
    public Color parentColor = Color.red;
    [SerializeField, Range(0, 3)]
    float sphereSize = 0.25f;
    private void OnDrawGizmosSelected()
    {
        //Dibujar una bola roja en el objeto
        Gizmos.color = parentColor;
        Gizmos.DrawSphere(transform.parent.position, sphereSize);

    }
#endif
}

