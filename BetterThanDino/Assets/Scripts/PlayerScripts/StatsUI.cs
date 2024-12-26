using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;

    private void Start()
    {
        // Check if StatsManager.Instance is available
        if (StatsManager.Instance == null)
        {
            Debug.LogError("StatsManager.Instance is null! Ensure a StatsManager object exists in the scene.");
            return;
        }

        UpdateStats();
    }

    public void UpdateStats()
    {
        if (StatsManager.Instance == null)
        {
            Debug.LogError("StatsManager.Instance is not available when UpdateStats is called!");
            return;
        }

        statsSlots[0].GetComponentInChildren<TMP_Text>().text = $"Max Health: {StatsManager.Instance.maxHealth}";
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = $"Speed: {StatsManager.Instance.speed}";
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = $"Damage: {StatsManager.Instance.damage}";
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = $"Attack Cooldown: {StatsManager.Instance.cooldown}";
        statsSlots[4].GetComponentInChildren<TMP_Text>().text = $"Weapon Range: {StatsManager.Instance.weaponRange}";
        statsSlots[5].GetComponentInChildren<TMP_Text>().text = $"Knockback Force: {StatsManager.Instance.knockbackForce}";
        statsSlots[6].GetComponentInChildren<TMP_Text>().text = $"Knockback Time: {StatsManager.Instance.knockbackTime}";
        statsSlots[7].GetComponentInChildren<TMP_Text>().text = $"Stun Time: {StatsManager.Instance.stunTime}";
    }
}
