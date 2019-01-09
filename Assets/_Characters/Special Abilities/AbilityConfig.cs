using UnityEngine;

namespace RPG.Characters
{


    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] bool targetsSelf = false;
        [SerializeField] private AudioClip[] sfx = new AudioClip[0];
        [SerializeField] private AnimationClip abilityAnimation;

        protected AbilityBehaviour behaviour;

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

        public AnimationClip AbilityAnimation {
            get {
                return abilityAnimation;
            }

        }
        #endregion

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo);

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        internal void AttachComponentTo(GameObject gameObject)
        {
            var behaviourComponent = GetBehaviourComponent(gameObject);
            behaviourComponent.SetConfig = this;
            behaviour = behaviourComponent;
        }


    }

}

