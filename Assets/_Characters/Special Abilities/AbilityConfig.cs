using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;
        public AbilityUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        } 
    }

    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField]
        float energyCost = 10f;
        [SerializeField]
        GameObject particlePrefab = null;
        [SerializeField]
        bool targetsSelf = false;
        protected AbilityBehaviour behaviour;
        [SerializeField]
        private AudioClip[] sfx = new AudioClip[0];


        #region Getter
        public AudioClip GetRandomSfx()
        {
            int randomIndex = Random.Range(0, sfx.Length);
            if (sfx.Length > 0)
            {
                return sfx[randomIndex];
            }
            return null;
        }

        public float EnergyCost {
            get {
                return energyCost;
            }
        }

        public GameObject ParticlePrefab {
            get {
                return particlePrefab;
            }
        }

        public bool TargetsSelf {
            get {
                return targetsSelf;
            }
        }
        #endregion

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo);
        
        public void Use(AbilityUseParams abilityParams)
        {
            behaviour.Use(abilityParams);
        }

        internal void AttachComponentTo(GameObject gameObject)
        {
            var behaviourComponent = GetBehaviourComponent(gameObject);
            behaviourComponent.SetConfig = this;
            behaviour = behaviourComponent;
        }
    }

}

