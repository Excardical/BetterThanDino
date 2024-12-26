using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth = 20;

    public GameObject destroyedBaseAnim;

    [SerializeField] FloatingStatusBar healthBar;
    public void Start()
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
    public void Awake() {
        healthBar = GetComponentInChildren<FloatingStatusBar>();
    }
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

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
        destroyedBaseAnim.SetActive(true);
    }
}
