namespace UnityStandardAssets.CinematicEffects
{
    using UnityEngine;

    public sealed class MinAttribute_RFX4 : PropertyAttribute
    {
        public readonly float min;

        public MinAttribute_RFX4(float min)
        {
            this.min = min;
        }
    }
}
