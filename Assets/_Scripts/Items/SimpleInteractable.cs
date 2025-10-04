using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;


public class SimpleInteractable : Interactable
{
    [SerializeField, Tooltip("The events will play if playCodedEvents is called with this code.")]
    ItemInteraction code;
    [SerializeField]
    private bool useOtherCodes = false;
    [SerializeField, CustomLabel("", true), HideIf("useOtherCodes", false), Tooltip("The events will also play if playCodedEvents is called with any of the added codes.")]
    private List<ItemInteraction> extraCodes;
    [Space]
    [SerializeField] 
    private bool useSimpleEvents = true;
    [SerializeField, HideIf("useSimpleEvents", false), Tooltip("The events will play alongside OnInteraction.")]
    private UnityEvent OnInteractSimple;


    public override void Interact(ItemInteraction code)
    {

        if (this.code != code)
        {
            if (!useOtherCodes) return;
            if (!extraCodes.Contains(code)) { return; }
        }

        base.Interact(code);
        if (useSimpleEvents) OnInteractSimple?.Invoke();
    }


}