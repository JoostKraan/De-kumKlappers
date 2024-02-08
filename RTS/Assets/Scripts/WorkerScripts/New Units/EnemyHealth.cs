using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public bool IsDead { get { return currentHealth <= 0; } }

   
    public delegate void OnTakeDamageDelegate(int damage);
    public event OnTakeDamageDelegate OnTakeDamage;

    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        OnTakeDamage?.Invoke(damage);
    }

    void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
    }
}