using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{


    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        GameObject weaponPrefab;
        [SerializeField]
        Transform gripTransform;
        [SerializeField]
        AnimationClip attackAnimation;
        [SerializeField]
        float maxAttackRange = 3f;

        [SerializeField] private float minTimeBetweenHits;

        public GameObject WeaponPrefab {
            get {
                return weaponPrefab;
            }
        }

        public Transform GripTransform {
            get {
                return gripTransform;
            }

            set {
                gripTransform = value;
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

        //clear assetpack predefined animations
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}
