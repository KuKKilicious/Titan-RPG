using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{

    [CreateAssetMenu(menuName = "RPG/Special Ability/Self Heal")]
    public class SelfHealConfig : SpecialAbility
    {
        [Header("Self Heal Specific")]
        [SerializeField]
        float healAmount;
        [SerializeField]
        AudioClip[] sfx;

        public float HealAmount {
            get {
                return healAmount;
            }

        }

        public AudioClip[] Sfx {
            get {
                return sfx;
            }
        }

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<SelfHealBehaviour>();

            behaviourComponent.SetConfig = this;
            behaviour = behaviourComponent;
        }
    }

}
