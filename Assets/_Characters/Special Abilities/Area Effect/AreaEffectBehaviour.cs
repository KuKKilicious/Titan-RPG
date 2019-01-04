using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Characters
{

    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;

        public AreaEffectConfig SetConfig {
            set {
                config = value;
            }
        }
        public void Use(AbilityUseParams useParams)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.SphereCastAll(ray, config.Radius);

            float damageToDeal = useParams.baseDamage+config.DamageToEachTarget;//move into loop, if considering enemy based adjustment
            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable!=null)
                {
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
    }
}
