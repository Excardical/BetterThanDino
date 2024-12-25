using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;

    private void Start()
    {
        UpdateHealth();
        UpdateSpeed();
        UpdateDamage();
        UpdateCooldown();
        UpdateWeaponRange();
        UpdateKnockbackForce();
        UpdateKnockbackTime();
        UpdateStunTime();

    }
    public void UpdateHealth()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Health: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
    }
    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
    }
    public void UpdateDamage()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
    }
    public void UpdateCooldown()
    {
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = "Attack cooldown: " + StatsManager.Instance.cooldown;
    }
    public void UpdateWeaponRange()
    {
        statsSlots[4].GetComponentInChildren<TMP_Text>().text = "Weapon Range: " + StatsManager.Instance.weaponRange;
    }
    public void UpdateKnockbackForce()
    {
        statsSlots[5].GetComponentInChildren<TMP_Text>().text = "Knockback Force: " + StatsManager.Instance.knockbackForce;
    }
    public void UpdateKnockbackTime()
    {
        statsSlots[6].GetComponentInChildren<TMP_Text>().text = "Knockback Time: " + StatsManager.Instance.knockbackTime;
    }
    public void UpdateStunTime()
    {
        statsSlots[7].GetComponentInChildren<TMP_Text>().text = "Stun Time: " + StatsManager.Instance.stunTime;
    }

}