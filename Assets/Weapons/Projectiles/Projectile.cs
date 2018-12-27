using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    float projectileSpeed = 10f;

    float damageValue = 5f;

    public float ProjectileSpeed {
        get {
            return projectileSpeed;
        }

        set {
            projectileSpeed = value;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var enemy = collision.collider.gameObject.GetComponent<Enemy>(); //ToDo make generic

        if (enemy) {
            return;
        }
            Debug.Log("trigger with: " + collision.gameObject.name);
            IDamageable damageable = collision.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.TakeDamage(damageValue);
                Destroy(gameObject,0.05f);
            }
        
    }

    internal void setDamage(float damage) {
        damageValue = damage;

    }
}
