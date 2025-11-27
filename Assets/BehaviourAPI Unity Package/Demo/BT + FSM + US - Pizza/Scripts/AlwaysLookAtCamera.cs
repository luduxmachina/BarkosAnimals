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
            transform.LookAt(target.transform.position);
        }
    }
}