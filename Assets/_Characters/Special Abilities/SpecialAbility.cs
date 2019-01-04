using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
  public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;
        public AbilityUseParams(IDamageable target,float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class SpecialAbility : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField]
         float energyCost = 10f;

        protected ISpecialAbility behaviour;

        public float EnergyCost {
            get {
                return energyCost;
            }
        }

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);
        public void Use(AbilityUseParams abilityParams) {
            behaviour.Use(abilityParams);
        }
    }
    public interface ISpecialAbility
    {
        void Use(AbilityUseParams abilityParams);
    }
}

