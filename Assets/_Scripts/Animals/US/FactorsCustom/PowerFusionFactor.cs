using System.Collections.Generic;
using System.Linq;

namespace BehaviourAPI.UtilitySystems
{
    /// <summary>
    /// Fusion factor that returns the weighted average from its children utility.
    /// </summary>
    public class PowerFusionFactor : FusionFactor
    {

        protected override float Evaluate(List<float> utilities)
        {
            float val = utilities.Aggregate(1f, (x, n) => x * n);
            return val *1.5f;
        }
    }
}
