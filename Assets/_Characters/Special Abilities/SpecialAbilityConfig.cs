using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    
    public abstract class SpecialAbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField]
        public readonly float energyCost = 10f;

        protected ISpecialAbility behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);
        public void Use() {
            behaviour.Use();
        }
    }

}

