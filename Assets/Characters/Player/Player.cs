using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {
    [SerializeField]
    int enemyLayer = 10;

    [SerializeField]
    float maxHealthPoints = 100f;

    [SerializeField]
    float damageToDeal = 10f;
    [SerializeField]
    float minTimeBetweenHits = 0.5f;
    [SerializeField]
    float maxAttackRange = 3f;

    float currentHealthPoints;
    GameObject currentTarget;
    float lastHitTime = 0f;

    CameraRaycaster cameraRaycaster;
    [SerializeField]
    Transform aimTransform;
    public float healthAsPercentage {
        get {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    public Transform AimTransform {
        get {
            return aimTransform;
        }
    }

    // Use this for initialization
    void Start() {
        currentHealthPoints = maxHealthPoints;
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += CameraRaycaster_notifyMouseClickObservers;
    }

    private void CameraRaycaster_notifyMouseClickObservers(RaycastHit raycastHit, int layerHit) {
        if (layerHit == enemyLayer) {

            var enemy = raycastHit.collider.gameObject;
            currentTarget = enemy;
            if (Vector3.Distance(transform.position, currentTarget.transform.position) <= maxAttackRange) { //check enemy is in range
                if (Time.time - lastHitTime >= minTimeBetweenHits) {
                    currentTarget.GetComponent<IDamageable>().TakeDamage(damageToDeal);
                    lastHitTime = Time.time;
                }
            }
        }
    }



    // Update is called once per frame
    void Update() {

    }

    void IDamageable.TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }
}
