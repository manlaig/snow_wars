using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HealthBar for objects that can be destroyed or killed
/// <summary>
public class HealthBar : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    bool damaged = false;
    bool destroyed = false;

    void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        //playerAudio.Play (); // Disabled because I can't find the missing import/using statement

        if (currentHealth <= 0 && !destroyed)
        {
            Destroy(gameObject);
        }
    }
}
