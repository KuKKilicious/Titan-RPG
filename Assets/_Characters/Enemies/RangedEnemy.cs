using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    public class RangedEnemy : MonoBehaviour
    {
        [SerializeField]
        WeaponConfig rangedWeapon;

        public float WeaponRange { get { return RangedWeapon.MaxAttackRange; } }

        public WeaponConfig RangedWeapon { get { return rangedWeapon; } }
    }

}
