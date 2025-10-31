using UnityEngine;
using UnityEngine.InputSystem;

public class SideScrollerCharacterController : MonoBehaviour
{
    [Header("2.5D Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayers = 1;

    [Header("Animation Parameters")]
    public string moveSpeedParam = "MoveSpeed";
    public string jumpParam = "Jump";
    public string groundedParam = "Grounded";

    [Header("Game Feel")]
    public float acceleration = 10f;
    public float deceleration = 15f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip playerJumpSound;
    public AudioClip playerLandSound;
    public AudioClip playerWalkSound;

    // Walking sound control
    private bool isWalking = false;
    private float walkSoundCooldown = 0f;

    // Component references
    private Animator m_animator;
    private Rigidbody m_rigidbody;
    private CapsuleCollider m_collider;

    // Movement state
    private float m_currentHorizontalSpeed;
    private bool m_isGrounded;
    private bool m_wasGrounded;
    private bool m_jumpInput;
    private Vector3 m_groundNormal = Vector3.up;

    // Input
    private Vector2 m_inputDirection;

    // Events for game juice
    public System.Action OnJump;
    public System.Action OnLand;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();

        // Configure rigidbody for 2.5D platformer
        if (m_rigidbody != null)
        {
            m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimator();
        HandleWalkSound();
    }

    private void FixedUpdate()
    {
        HandleGroundDetection();
        HandleMovement();
        HandleJump();
    }

    private void HandleInput()
    {
        // Get horizontal input (left/right)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        m_inputDirection = new Vector2(horizontalInput, 0);

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_jumpInput = true;
        }
    }

    private void HandleGroundDetection()
    {
        m_wasGrounded = m_isGrounded;

        float sphereRadius = m_collider ? m_collider.radius * 0.8f : 0.3f;
        Vector3 sphereStart = transform.position + Vector3.up * 0.1f;
        float rayLength = groundCheckDistance;

        RaycastHit hit;
        m_isGrounded = Physics.SphereCast(sphereStart, sphereRadius, Vector3.down, out hit, rayLength, groundLayers);

        // Play landing sound
        if (!m_wasGrounded && m_isGrounded && m_rigidbody.linearVelocity.y <= 0.1f)
        {
            PlayLandSound();
            OnLand?.Invoke();
        }
    }

    private void HandleMovement()
    {
        // Calculate target speed based on input
        float targetSpeed = m_inputDirection.x * moveSpeed;

        // Smooth acceleration and deceleration
        if (Mathf.Abs(targetSpeed) > 0.1f)
        {
            m_currentHorizontalSpeed = Mathf.MoveTowards(
                m_currentHorizontalSpeed,
                targetSpeed,
                acceleration * Time.fixedDeltaTime
            );
            isWalking = true;
        }
        else
        {
            m_currentHorizontalSpeed = Mathf.MoveTowards(
                m_currentHorizontalSpeed,
                0f,
                deceleration * Time.fixedDeltaTime
            );
            isWalking = false;
        }

        // NUCLEAR DRIFT FIX
        if (Mathf.Abs(m_inputDirection.x) < 0.01f)
        {
            m_currentHorizontalSpeed = 0f;
            m_rigidbody.linearVelocity = new Vector3(0, m_rigidbody.linearVelocity.y, 0);
        }
        else
        {
            Vector3 velocity = m_rigidbody.linearVelocity;
            velocity.x = m_currentHorizontalSpeed;
            m_rigidbody.linearVelocity = velocity;
        }

        // Handle character facing direction
        if (Mathf.Abs(m_currentHorizontalSpeed) > 0.1f)
        {
            float facingDirection = Mathf.Sign(m_currentHorizontalSpeed);
            transform.rotation = Quaternion.Euler(0, facingDirection > 0 ? 90 : 270, 0);
        }
    }

    private void HandleJump()
    {
        if (m_jumpInput)
        {
            if (m_isGrounded)
            {
                m_rigidbody.linearVelocity = new Vector3(
                    m_rigidbody.linearVelocity.x,
                    jumpForce,
                    m_rigidbody.linearVelocity.z
                );
                PlayJumpSound();
                OnJump?.Invoke();
            }
            m_jumpInput = false;
        }
    }

    private void HandleWalkSound()
    {
        if (isWalking && m_isGrounded)
        {
            walkSoundCooldown -= Time.deltaTime;
            if (walkSoundCooldown <= 0f)
            {
                PlayWalkSound();
                walkSoundCooldown = 0.3f; // Adjust for walk rhythm
            }
        }
        else
        {
            walkSoundCooldown = 0f;
        }
    }

    private void PlayJumpSound()
    {
        if (audioSource != null && playerJumpSound != null)
        {
            audioSource.PlayOneShot(playerJumpSound);
        }
    }

    private void PlayLandSound()
    {
        if (audioSource != null && playerLandSound != null)
        {
            audioSource.PlayOneShot(playerLandSound);
        }
    }

    private void PlayWalkSound()
    {
        if (audioSource != null && playerWalkSound != null)
        {
            audioSource.PlayOneShot(playerWalkSound, 1.0f); // Lower volume for footsteps
        }
    }

    private void UpdateAnimator()
    {
        if (m_animator == null) return;

        m_animator.SetFloat(moveSpeedParam, Mathf.Abs(m_currentHorizontalSpeed));
        m_animator.SetBool(groundedParam, m_isGrounded);
    }

    // Input System support
    public void OnMove(InputValue value)
    {
        m_inputDirection = value.Get<Vector2>();
    }

    public void OnJumpInput(InputValue value)
    {
        if (value.isPressed)
        {
            m_jumpInput = true;
        }
    }

    // Public methods for external control
    public void AddKnockback(Vector3 force)
    {
        m_rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void SetMovementEnabled(bool enabled)
    {
        this.enabled = enabled;
        if (!enabled)
        {
            m_rigidbody.linearVelocity = Vector3.zero;
        }
    }

    // Getters for other systems
    public bool IsGrounded() => m_isGrounded;
    public float GetCurrentSpeed() => Mathf.Abs(m_currentHorizontalSpeed);

    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = m_isGrounded ? Color.green : Color.red;
        float sphereRadius = m_collider ? m_collider.radius * 0.8f : 0.3f;
        Vector3 sphereStart = transform.position + Vector3.up * 0.1f;
        Gizmos.DrawWireSphere(sphereStart, sphereRadius);
        Gizmos.DrawLine(sphereStart, sphereStart + Vector3.down * groundCheckDistance);
    }
}