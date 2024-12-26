using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            DestroyBase();
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void DestroyBase()
    {
        Debug.Log("Base destroyed!");
        Destroy(gameObject);
    }
}
