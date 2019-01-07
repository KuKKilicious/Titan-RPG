using UnityEngine;
namespace RPG.Characters
{

    [CreateAssetMenu(menuName = "RPG/Special Ability/Self Heal")]
    public class SelfHealConfig : AbilityConfig
    {
        [Header("Self Heal Specific")]
        [SerializeField]
        float healAmount;


        public float HealAmount {
            get {
                return healAmount;
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
