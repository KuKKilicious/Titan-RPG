using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapons
{
    

public class Projectile : MonoBehaviour {
    [SerializeField]
    float projectileSpeed = 10f;

    float damageValue = 5f;
    private const float DESTROY_DELAY = 0.04f;
    private GameObject shooter;
    private int shooterLayer;
    public float ProjectileSpeed {
        get {
            return projectileSpeed;
        }

        set {
            projectileSpeed = value;
        }
    }

    public GameObject Shooter {
        //get {
        //    return shooter;
        //}

        set {
            shooter = value;
            shooterLayer = value.layer;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collisionLayer = collision.gameObject.layer; //ToDo make generic

        if (collisionLayer == shooterLayer)
        {
            return;
        }
        DamageDamageable(collision);

    }

    private void DamageDamageable(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damageValue);
            Destroy(gameObject, DESTROY_DELAY);
        }
    }

    internal void setDamage(float damage) {
        damageValue = damage;

    }
}
}
