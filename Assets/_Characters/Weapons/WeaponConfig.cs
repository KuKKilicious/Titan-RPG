using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{


    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField]
        GameObject weaponPrefab;
        [SerializeField]
        Transform gripTransform;
        [SerializeField]
        AnimationClip attackAnimation;
        [SerializeField]
        float maxAttackRange = 3f;
        [SerializeField]
        float additionalDamage = 5f;

        [SerializeField] private float minTimeBetweenHits;

        #region Getters
        public GameObject WeaponPrefab {
            get {
                return weaponPrefab;
            }
        }

        public Transform GripTransform {
            get {
                return gripTransform;
            }
        }

        public AnimationClip AttackAnimation {
            get {
                RemoveAnimationEvents();
                return attackAnimation;
            }
        }

        public float MinTimeBetweenHits {
            get {
                return minTimeBetweenHits;
            }
        }

        public float MaxAttackRange {
            get {
                return maxAttackRange;
            }
        }

        public float AdditionalDamage {
            get {
                return additionalDamage;
            }
        }
        #endregion

        //clear assetpack predefined animations
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}
