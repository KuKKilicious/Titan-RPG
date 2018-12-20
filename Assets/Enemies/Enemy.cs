﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour,IDamageable {
    [SerializeField]
    float maxHealthPoints = 100f;

    float currentHealthPoints =100f;

    [SerializeField] float aggroRadius = 5;
    AICharacterControl aiCharControl;
    Player player;

    private void Start() {
        aiCharControl = GetComponent<AICharacterControl>();
        player = FindObjectOfType<Player>();
    }

    private void Update() {
        CheckForPlayerInRange();
    }

    private void CheckForPlayerInRange() {
        if(Vector3.Distance(player.transform.position, transform.position)< aggroRadius) {
            aiCharControl.target = player.transform;
        }else {
            aiCharControl.target = transform;
        }
    }

    void IDamageable.TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

    }

    public float healthAsPercentage {
        get {
            return currentHealthPoints / maxHealthPoints;
        }
    }


}
