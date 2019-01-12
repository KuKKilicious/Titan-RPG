using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{

    [CreateAssetMenu(menuName = "RPG/Special Ability/Fire Stomp")]
    public class FireStompConfig : AbilityConfig
    {
        [Header("Area Effect Specific")]
        [SerializeField]float radius = 1f;
        [SerializeField]float animationDelay= 0.5f;
        [SerializeField] float damageToEachTarget = 10f;

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

        public float AnimationDelay {
            get {
                return animationDelay;
            }
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<FireStompBehaviour>();
        }

    }
}
