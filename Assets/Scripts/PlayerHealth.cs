using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public Slider healthbar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = currentHealth;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        healthbar.value = currentHealth;
        if(currentHealth <= 0){
            Die();
        }
    }

    public void Die(){
        if(GameManager.Instance != null){
            GameManager.Instance.RestartGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
