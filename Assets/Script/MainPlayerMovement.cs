using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    private float horizontalMovement;
    private float verticalMovement;
   
    public bool isFacingRight = true;

    private Vector3 velocity = Vector3.zero;

    [Header("Jump")]
    private bool isGrounded;
    private bool isJumping;
    private bool isInWater;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    public float jumpForce = 350f;

    [Header("Verificator")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 0.2f;


    [Header("Other")]
    public Rigidbody2D rb;
    public Animator animator;


    public static MainPlayerMovement instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de MainPlayerMovement dans la sc�ne");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, raycastDistance, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * raycastDistance, Color.red);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f))
        {
            isJumping = true;
            jumpBufferCounter = 0f;
        }

    
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("speed", characterVelocity);

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, raycastDistance, groundLayer) || isInWater;
         MovePlayer(horizontalMovement, verticalMovement);
        
    }

    private void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        
            Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);

            if (isJumping)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                isJumping = false;
            }
       
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    



   
    
}
