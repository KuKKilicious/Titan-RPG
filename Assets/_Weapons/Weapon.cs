using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
    

[CreateAssetMenu(menuName = ("RPG/Weapon"))]
public class Weapon : ScriptableObject {
    [SerializeField]
    GameObject weaponPrefab;
    [SerializeField]
    Transform gripTransform;
    [SerializeField]
    AnimationClip attackAnimation;

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
}
}
