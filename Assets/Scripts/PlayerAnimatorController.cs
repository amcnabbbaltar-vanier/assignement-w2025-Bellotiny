using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;
    private Rigidbody rb;
    public float speedBoostTime = 5.0f;
    public float jumpBoostTime = 30.0f;
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
        //animator.SetBool("doFlip", movement.doFlip);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            Destroy(other.gameObject);
            speedBoostTime -= Time.deltaTime;
        }
        if (other.gameObject.tag == "JumpBoost")
        {
            Destroy(other.gameObject);
            jumpBoostTime -= Time.deltaTime;
        }
        if (other.gameObject.tag == "ScorePickup")
        {
            GameManager.Instance.IncrementScore(50);
            // scoreText.text = ("Score  = " + score);
            Destroy(other.gameObject);
        }
        
    }
}
