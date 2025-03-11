using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;
    private Rigidbody rb;
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
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("CharacterSpeed", rb.velocity.magnitude);
        animator.SetBool("IsGrounded", movement.IsGrounded);
        animator.SetBool("doFlip", isJumpBoostActive);

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned.");
        }
        if (movement == null)
        {
            Debug.LogError("CharacterMovement is not assigned.");
        }
        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned.");
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
        if (other.gameObject.tag == "SpeedBoost")
        {
            Destroy(other.gameObject);
            isSpeedBoosted = true;
            movement.speedMultiplier = 2.0f;
            speedBoostTime = 5.0f;
        }
        if (other.gameObject.tag == "JumpBoost")
        {
            Destroy(other.gameObject);
            isJumpBoostActive = true;
            jumpBoostTime = 30.0f;
        }
        if (other.gameObject.tag == "ScorePickup")
        {
            if(GameManager.Instance != null){
                GameManager.Instance.IncrementScore(50);
                // scoreText.text = ("Score  = " + score);
                Destroy(other.gameObject);
            }else{
                Debug.LogError("GameManager is not instantiated.");
            }
        }
    }
}
