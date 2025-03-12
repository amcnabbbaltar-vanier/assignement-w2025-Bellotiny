using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;
    private Rigidbody rb;
    private PlayerHealth health;
    private ParticleSystem hitParticles;
    public float fallThreshold = -10f;
    private float speedBoostTime = 5.0f;
    private bool isSpeedBoosted = false;
    private float jumpBoostTime = 30.0f;
    private bool isJumpBoostActive = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<PlayerHealth>();
        hitParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("CharacterSpeed", rb.velocity.magnitude);
        animator.SetBool("IsGrounded", movement.IsGrounded);
        animator.SetBool("doFlip", isJumpBoostActive);

        if (transform.position.y < fallThreshold)
        {
            Debug.Log("Player fell off!");
            health.TakeDamage(1);
            if (GameManager.Instance != null){
                GameManager.Instance.currentLevelScore = 0;
            }

        }

        if (isSpeedBoosted)
        {
            speedBoostTime -= Time.deltaTime;
            if (speedBoostTime <= 0)
            {
                isSpeedBoosted = false;
                movement.speedMultiplier = 1.0f;
            }
        }

        if (isJumpBoostActive)
        {
            jumpBoostTime -= Time.deltaTime;
            if (jumpBoostTime <= 0)
            {
                isJumpBoostActive = false; // End the jump boost after time runs out
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        var mainModule = hitParticles.main;

        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.Lerp(Color.red, Color.yellow, 0.5f)); // Orange
            hitParticles.Play();
            Destroy(other.gameObject);
            isSpeedBoosted = true;
            movement.speedMultiplier = 2.0f;
            speedBoostTime = 5.0f;
        }
        else if (other.gameObject.CompareTag("JumpBoost"))
        {
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.blue); // Blue
            hitParticles.Play();
            Destroy(other.gameObject);
            isJumpBoostActive = true;
            jumpBoostTime = 30.0f;
        }
        else if (other.gameObject.CompareTag("ScorePickup"))
        {
            if (GameManager.Instance != null)
            {
                mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.green); // Green
                hitParticles.Play();
                GameManager.Instance.IncrementScore(50);
                Destroy(other.gameObject);
            }
            else
            {
                Debug.LogError("GameManager is not instantiated.");
            }
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Color.red); // Green
            //hitParticles.Play();
            health.TakeDamage(1);
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncrementTotal();
                GameManager.Instance.LoadNextScene();
            }
            else
            {
                Debug.LogError("GameManager is not instantiated.");
            }
        }
    }
}
