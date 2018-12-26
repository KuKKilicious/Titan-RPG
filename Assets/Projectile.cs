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

    private void OnTriggerEnter(Collider other) {
        Debug.Log("trigger with: " + other.gameObject.name);
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable!=null) {
            damageable.TakeDamage(damageValue);
        }
    }

    internal void setDamage(float damage) {
        damageValue = damage;

    }
}
