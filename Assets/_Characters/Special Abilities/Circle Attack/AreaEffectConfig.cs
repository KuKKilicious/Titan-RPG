using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{

    [CreateAssetMenu(menuName = "RPG/Special Ability/Area Effect")]
    public class AreaEffectConfig : AbilityConfig
    {
        [Header("Area Effect Specific")]
        [SerializeField]
        float radius = 1f;
        float damageToEachTarget = 10f;

        public float Radius {
            get {
                return radius;
            }
            
        }

        public float DamageToEachTarget {
            get {
                return damageToEachTarget;
            }
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<AreaEffectBehaviour>();
        }

    }
}
