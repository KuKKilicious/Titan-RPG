using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    float damageValue = 5f;
    private void OnTriggerEnter(Collider other) {
        Debug.Log("trigger with: " + other.gameObject.name);
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable!=null) {
            damageable.TakeDamage(damageValue);
        }
    }
}
