using UnityEngine;

namespace BehaviourAPI.UnityToolkit
{
    using Core;

    [SelectionGroup("PROPIOS")]
    public class FleeActionNoRecto : FleeAction
    {
        public AAnimal animalContext;
        /// <summary>
        /// The movement speed of the agent.
        /// </summary>
        public float angulo;

        public override void Start()
        {
            _timeRunning = Time.time;
            context.Movement.Speed *= speed;
        }
        public override Status Update()
        {

            OtherTransform = animalContext.GetClosestPredator();
            if(OtherTransform == null)
            {
                return Status.Success;
            }
            if (_timeRunning + maxTimeRunning < Time.time) return Status.Failure;
            if (Vector3.Distance(context.Transform.position, OtherTransform.position) > distance)
            {
                return Status.Success;
            }
            else
            {
                var dir = (context.Transform.position - OtherTransform.position).normalized;

                //añadir aqui un poco de angulo

                Vector3 axis = Vector3.up;             

                Vector3 rotated = Quaternion.AngleAxis(angulo, axis) * dir;


                var targetPos = context.Transform.position + rotated;
                context.Movement.SetTarget(targetPos);
                return Status.Running;
            }
        }
    }

}