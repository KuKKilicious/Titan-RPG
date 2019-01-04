using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{

    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;

        public PowerAttackConfig SetConfig {
            set {
                config = value;
            }
        }

        public void Use()
        {
        }

    }
}
