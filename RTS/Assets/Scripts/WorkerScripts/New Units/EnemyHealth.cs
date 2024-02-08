using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth >= 0)
        {
            currentHealth -= damage;
        }
        else
        {
            isDead = true;
            Invoke("Die", 1);
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
    }
}