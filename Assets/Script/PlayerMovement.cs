using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    private float horizontalMovement;
    private float verticalMovement;
    private float horizontalMovementDash;
    private float verticalMovementDash;
    public bool isFacingRight = true;
    public Transform backGround;
    public Transform cameraTransform;

    private Vector3 velocity = Vector3.zero;

    [Header("Climb")]
    public float climbSpeed = 7f;
    [HideInInspector]public bool isClimbing;

    [Header("Jump")]
    private bool isGrounded;
    private bool isJumping;
    private bool isInWater;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    public float jumpForce = 350f;

    [Header("Wall Run / Jump")]
    private bool isWallSliding;
    public float wallSlidingSpeed = 2f;
    private bool isWallJumping;
    public float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(10f, 350f);
    public bool isWallSlide;

    [Header("Dash")]
    [SerializeField] private bool canDash;
    private bool isDashing;
    public float dashingPower;
    public float dashingTime;
    public float dashingCooldown;
    public Color effectDashColor;

    [Header("Verificator")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 0.2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    [Header("Other")]
    public Rigidbody2D rb;
    public GameObject squareDash;
    public Animator animator;
    public float afterimageSpawnInterval = 0.1f;

    public static PlayerMovement instance;

    private float afterimageSpawnTimer;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la sc�ne");
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
         backGround.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, 0);
       

        if (isDashing)
        {
            afterimageSpawnTimer -= Time.deltaTime;
            if (afterimageSpawnTimer <= 0)
            {
                SpawnAfterimage();
                afterimageSpawnTimer = afterimageSpawnInterval;
            }
            return;
        }

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;
        verticalMovement = Input.GetAxis("Vertical") * climbSpeed;


        verticalMovementDash = Input.GetAxisRaw("Vertical") < 0f ? -1f : (Input.GetAxisRaw("Vertical") == 0f ? 0f : 1f);
        horizontalMovementDash = Input.GetAxisRaw("Horizontal") < 0f ? -1f : (Input.GetAxisRaw("Horizontal") == 0f ? 0f : 1f);




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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }


        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("speed", characterVelocity);

    }

    private void FixedUpdate()
    {

       
        if (isDashing)
        {
            return;
        }

        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, raycastDistance, groundLayer) || isInWater;

        if (!isWallJumping)
        {
            MovePlayer(horizontalMovement, verticalMovement);
        }
    }

    public void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        if (!isClimbing)
        {
            Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);

            if (isJumping)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                isJumping = false;
            }
        }
        else
        {
            Vector3 targetVelocity = new Vector2(0, _verticalMovement);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        afterimageSpawnTimer = afterimageSpawnInterval;
        rb.velocity = new Vector2(horizontalMovementDash * dashingPower, verticalMovementDash * (dashingPower / 2.5f));
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    

    private void SpawnAfterimage()
    {
        GameObject afterimage = Instantiate(squareDash, transform.position, transform.rotation);

        afterimage.AddComponent<SpriteRenderer>();
        afterimage.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        afterimage.transform.localScale = transform.localScale;
        StartCoroutine(FadeAfterimage(afterimage));
    }

    private IEnumerator FadeAfterimage(GameObject afterimage)
    {
        SpriteRenderer sr = afterimage.GetComponent<SpriteRenderer>();
        Color color;
        for (float t = 0; t < 1; t += Time.deltaTime / dashingTime)
        {

            color = new Color(effectDashColor.r, effectDashColor.g, effectDashColor.b, Mathf.Lerp(1, 0, t));
            sr.color = color;
            yield return null;
        }
        Destroy(afterimage);
    }

}
