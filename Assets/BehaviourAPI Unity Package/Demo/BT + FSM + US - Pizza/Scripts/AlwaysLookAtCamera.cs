using System;
using UnityEngine;

namespace BehaviourAPI.UnityToolkit.Demos
{
    public class AlwaysLookAtCamera : MonoBehaviour
    {

        #region variables

        [SerializeField] private Camera target;

        #endregion variables

        private void Start()
        {
            if (target == null)
                target = Camera.main;
        }

        // Update is called once per frame
        private void Update()
        {
            if(!target.enabled) target = Camera.main;

            Vector3 dir = target.transform.position - transform.position;
            transform.forward = -dir;
        }
    }
}