using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;

public class BarrilInteractionControlesr : MonoBehaviour, IInteractable
{
    [SerializeField]ItemNames tipoContenido;

    [SerializeField] int maxStacks = 5;
    [SerializeField] int stacksContenido = 5;

    [SerializeField] GameObject ModeloLleno;
    [SerializeField] GameObject ModeloVacio;

    [SerializeField] TextMeshProUGUI textoContenido;

    private void Start()
    {
        textoContenido.text = stacksContenido.ToString() + "/" + maxStacks.ToString();
    }

    public bool Interact(ItemNames interactorType, GameObject interactor)
    {
        ItemInScene temp = interactor.GetComponent<ItemInScene>();
        CuencoManager cManager = temp.gameObject.GetComponentInParent<CuencoManager>();
        bool conseguido = false;
        if (cManager != null)
        {
            if(stacksContenido > 0 && !cManager.HayComida())
            {
                cManager.SetFood(tipoContenido);
                stacksContenido--;
                textoContenido.text = stacksContenido.ToString() + "/" + maxStacks.ToString();
                conseguido = true;
            }
            else if(cManager.HayComida() && cManager.ComidaEnCuenco() == tipoContenido && stacksContenido < maxStacks)
            {
                stacksContenido++;
                cManager.SetFood(ItemNames.None);
                textoContenido.text = stacksContenido.ToString() + "/" + maxStacks.ToString();
                conseguido = true;
            }
            else
            {
                conseguido = false;
            }

            if (stacksContenido > 0)
            {
                ModeloLleno.SetActive(true);
                ModeloVacio.SetActive(false);
            }
            else
            {
                ModeloLleno.SetActive(false);
                ModeloVacio.SetActive(true);
            }
        }
        return conseguido;
    }
}
