using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private float health = 100;
    private PlayerHealth playerHealth; 
    private Animator animator;
    private ParticleSystem flipParticles;
    private CharacterMovement movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        flipParticles = GetComponent<ParticleSystem>();
        movement = GetComponent<CharacterMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        
    }
    // public void DoFlip()
    // {
    //     if(movement.doFlip){
    //         flipParticles.Play();
    //     }
    // }
}
