using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private float health = 100;
    private PlayerHealth playerHealth; 
    private Animator animator;
    private ParticleSystem hitParticles;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        hitParticles = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void GotHit()
    {
        if(playerHealth != null){
            playerHealth.TakeDamage(30);
            animator.SetTrigger("GotHit");
            hitParticles.Play();
            audioSource.Play();
        }
    }
}
