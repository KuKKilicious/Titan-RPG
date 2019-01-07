using System;
using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickupSFX;

        AudioSource audioSource;
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void RefreshPrefab()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
            var weapon = weaponConfig.WeaponPrefab;
            weapon.transform.position = Vector3.zero;
            var weaponInst = Instantiate(weapon, gameObject.transform);

        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.PutWeaponInHand(weaponConfig);
                audioSource.PlayOneShot(pickupSFX);
            }
        }
    }
}
