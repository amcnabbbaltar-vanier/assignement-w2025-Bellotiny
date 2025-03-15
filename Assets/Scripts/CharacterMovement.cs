using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))] // Ensures that a Rigidbody component is attached to the GameObject
public class CharacterMovement : MonoBehaviour
{
    private Animator animator;
    // ============================== Movement Settings ==============================
    [Header("Movement Settings")]
    [SerializeField] private float baseWalkSpeed = 5f;    // Base speed when walking
    [SerializeField] private float baseRunSpeed = 8f;     // Base speed when running
    [SerializeField] private float rotationSpeed = 10f;   // Speed at which the character rotates

    // ============================== Jump Settings =================================
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;        // Jump force applied to the character
    [SerializeField] private float groundCheckDistance = 1.1f; // Distance to check for ground contact (Raycast)
    private int jumpCount = 0; // Tracks the number of jumps
    private float jumpTimeWindow = 0.8f; // Time window for detecting second jump
    private float lastJumpTime = 0f;

    // ============================== Modifiable from other scripts ==================
    public float speedMultiplier = 1.0f; // Additional multiplier for character speed ( WINK WINK )

    // ============================== Private Variables ==============================
    private Rigidbody rb; // Reference to the Rigidbody component
    private Transform cameraTransform; // Reference to the camera's transform

    // Input variables
    private float moveX; // Stores horizontal movement input (A/D or Left/Right Arrow)
    private float moveZ; // Stores vertical movement input (W/S or Up/Down Arrow)
    private bool jumpRequest; // Flag to check if the player requested a jump
    //private bool flipRequest; // Flag to check if the player requested do flip
    private Vector3 moveDirection; // Stores the calculated movement direction

    // ============================== Animation Variables ==============================
    [Header("Anim values")]
    public float groundSpeed; // Speed value used for animations

    // ============================== Character State Properties ==============================
    /// <summary>
    /// Checks if the character is currently grounded using a Raycast.
    /// If false, the character is in the air.
    /// </summary>
    public bool IsGrounded => 
        Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);
    private bool isInAir = false;
    /// <summary>
    /// Checks if the player is currently holding the "Run" button.
    /// </summary>
    private bool IsRunning = false;
    public bool doFlip = false;
    private bool isFlipping = false;
    private AudioSource audioSource;
    public AudioClip flipSound;

    // ============================== Unity Built-in Methods ==============================

    /// <summary>
    /// Called when the script is first initialized.
    /// </summary>
    private void Awake()
    {
        InitializeComponents(); // Initialize Rigidbody and Camera reference
    }

    /// <summary>
    /// Called every frame, used to register player input.
    /// </summary>
    private void Update()
    {
        if (IsGrounded && !isInAir)  // Only reset when we are grounded and was previously in the air
        {
            Debug.Log("Resetting jump count and flip after landing.");
            jumpCount = 0;  // Reset jump count after landing
            isFlipping = false;  // Stop flip animation when grounded
        }
        RegisterInput(); // Collect player input
    }

    /// <summary>
    /// Called every physics update (FixedUpdate ensures physics stability).
    /// </summary>
    private void FixedUpdate()
    {
        HandleMovement(); // Process movement and physics-based updates
        
    }

    // ============================== Initialization ==============================

    /// <summary>
    /// Initializes Rigidbody and camera reference.
    /// Also locks and hides the cursor for better control.
    /// </summary>
    private void InitializeComponents()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.freezeRotation = true; // Prevent Rigidbody from rotating due to physics interactions
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth physics interpolation

        audioSource = GetComponent<AudioSource>();
        // Assign the main camera if available
        if (Camera.main)
            cameraTransform = Camera.main.transform;

        // Lock and hide the cursor for better gameplay control
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // ============================== Input Handling ==============================

    /// <summary>
    /// Reads player input values and registers movement/jump requests.
    /// </summary>
    private void RegisterInput()
    {
        moveX = Input.GetAxis("Horizontal"); // Get horizontal movement input
        moveZ = Input.GetAxis("Vertical");   // Get vertical movement input

        // Register a jump request if the player presses the Jump button
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            IsRunning = !IsRunning;
        }

        
    }

    // ============================== Movement Handling ==============================

    /// <summary>
    /// Handles movement-related logic: calculating direction, jumping, rotating, and moving.
    /// </summary>
    private void HandleMovement()
    {
        CalculateMoveDirection(); // Compute the movement direction based on input
        HandleJump(); // Process jump input
        RotateCharacter(); // Rotate the character towards the movement direction
        MoveCharacter(); // Move the character using velocity-based movement
    }

    /// <summary>
    /// Calculates the movement direction based on player input and camera orientation.
    /// </summary>
    private void CalculateMoveDirection()
    {
        // If the camera is not assigned, move based on world space
        if (!cameraTransform)
        {
            moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        }
        else
        {
            // Get forward and right vectors from the camera perspective
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Ignore Y-axis movement to prevent unwanted tilting
            forward.y = 0f;
            right.y = 0f;

            // Normalize vectors to maintain consistent movement speed
            forward.Normalize();
            right.Normalize();

            // Calculate movement direction relative to the camera orientation
            moveDirection = (forward * moveZ + right * moveX).normalized;
        }
    }

    /// <summary>
    /// Handles jumping by applying an impulse force if the character is grounded.
    /// </summary>
    private void HandleJump()
    {
        // Apply jump force only if jump was requested and the character is grounded
        if(jumpRequest){
            Debug.Log("There was a jumpRequest!!!!!!!");
            Debug.Log("IsGrounded: " + IsGrounded);
            Debug.Log("isInAir: " + isInAir);
            Debug.Log("jumpCount: " + jumpCount);
            Debug.Log("doFlip: " + doFlip);
        }
        
        if (jumpRequest && IsGrounded)
        {
            Vector3 currentVelocity = rb.velocity; // Get the current velocity
            float horizontalVelocityFactor = 1.0f; // Modify this to scale the horizontal velocity preservation
            rb.velocity = new Vector3(currentVelocity.x * horizontalVelocityFactor, 0f, currentVelocity.z * horizontalVelocityFactor);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply force upwards
            //Check first jump
            jumpCount = 1;
            lastJumpTime = Time.time;
            isFlipping = false;
            isInAir = true;
            jumpRequest = false; // Reset jump request after applying jump
            Debug.Log("First jump executed!!!!!!!!!!!!");
        }else if (jumpRequest && jumpCount == 1 && doFlip && (Time.time - lastJumpTime) <= jumpTimeWindow)
        {
            // Second jump (Flip)
            Vector3 currentVelocity = rb.velocity; // Get the current velocity
            float flipHorizontalVelocityFactor = 0.75f; // Scale horizontal velocity on flip jump, if needed

            rb.velocity = new Vector3(currentVelocity.x * flipHorizontalVelocityFactor, 0f, currentVelocity.z * flipHorizontalVelocityFactor);
            rb.AddForce(Vector3.up * (jumpForce * 0.8f), ForceMode.Impulse); // Higher force for flip
            PlayFlipSound(); // Play sound
            jumpCount = 2;
            isFlipping = true;
            animator.SetBool("isFlipping", isFlipping);
            jumpRequest = false;
            isInAir = false;
        }
    }

    /// <summary>
    /// Rotates the character towards the movement direction.
    /// </summary>
    private void RotateCharacter()
    {
        // Rotate only if the character is moving
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Moves the character using Rigidbody's velocity instead of MovePosition.
    /// This ensures smooth movement while avoiding physics conflicts.
    /// </summary>
    private void MoveCharacter()
    {
        // Determine movement speed (walking or running)
        float speed = IsRunning ? baseRunSpeed : baseWalkSpeed;
        
        // Set ground speed value for animation purposes
        groundSpeed = (moveDirection != Vector3.zero) ? speed : 0.0f;

        // Preserve the current Y velocity to maintain gravity effects
        Vector3 newVelocity = new Vector3(
            moveDirection.x * speed * speedMultiplier, 
            rb.velocity.y, // Keep the existing Y velocity for jumping & gravity
            moveDirection.z * speed * speedMultiplier
        );

        // Apply the new velocity directly
        rb.velocity = newVelocity;
    }

    public void PlayFlipSound()
{
    if (audioSource != null && flipSound != null)
    {
        audioSource.PlayOneShot(flipSound);
    }
    else
    {
        Debug.LogError("AudioSource or FlipSound is missing.");
    }
}

}
