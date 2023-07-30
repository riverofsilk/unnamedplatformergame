using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float dashDistance;
    public float jumpHeight;
    public float minJumpHeight;
    public float gravityMultiplier;
    public float airSpeed;
    private BoxCollider2D groundCheck;
    private PlayerControls input;
    private Rigidbody2D rb;
    private ConstantForce2D gravity;
    private Vector2 move;
    private Vector2 lashDirection;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool jumpButtonPressed = false;
    private int lookDirection = 1;
    private float minJumpTimer;
    private Vector2 downDirection = new Vector2(0, -1);


    void setGravity(Vector2 direction)
    {
        gravity.force = rb.mass * Physics2D.gravity.magnitude * direction * gravityMultiplier;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerControls();
        groundCheck = transform.GetChild(0).GetComponent<BoxCollider2D>();
        gravity = GetComponent<ConstantForce2D>();
        input.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        input.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        input.Gameplay.Jump.performed += ctx => jumpButtonPressed = true;
        input.Gameplay.Jump.canceled += ctx => jumpButtonPressed = false;
        input.Gameplay.BasicLashing.performed += ctx => lashDirection = ctx.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {

        // Walking
        if (isGrounded)
        {
            if (downDirection.x == 0)
            {
                rb.velocity = new Vector2(move.x * moveSpeed * Time.deltaTime, rb.velocity.y);
            }
            if (downDirection.y == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, move.y * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            rb.AddForce(move * airSpeed * Time.deltaTime * (Vector2.one - new Vector2(Mathf.Abs(downDirection.x), Mathf.Abs(downDirection.y))));
        }
        // Ground Check
        if (groundCheck.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
        }
        // Jump
        if (jumpButtonPressed && isGrounded)
        {
            minJumpTimer = minJumpHeight;
            rb.AddForce(-downDirection * jumpHeight);
            isJumping = true;
        }
        if (rb.velocity.y < 0) { isJumping = false; }
        // Variable Jump Height
        if (isJumping)
        {
            minJumpTimer--;
        }
        if (isJumping && !jumpButtonPressed && minJumpTimer <= 0)
        {
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, -Time.deltaTime);
        }
        // Basic Lashing

        if (lashDirection.x > 0.5)
        {
            // Right
            downDirection = new Vector2(1, 0);
        }
        if (lashDirection.x < -0.5)
        {
            // Left
            downDirection = new Vector2(-1, 0);
        }
        if (lashDirection.y > 0.5)
        {
            // Up
            downDirection = new Vector2(0, 1);
        }
        if (lashDirection.y < -0.5)
        {
            // Down
            downDirection = new Vector2(0, -1);
        }

        // Turning
        if (move.x < -0.5 || move.y > 0.5)
        {
            lookDirection = -1;
        }
        if (move.x > 0.5 || move.y < -0.5)
        {
            lookDirection = 1;
        }
        Flip(lookDirection);
        setGravity(downDirection);
        transform.up = new Vector3(-downDirection.x, -downDirection.y, 0);

    }
    void Flip(int side)
    {
        Vector3 local_scale = transform.localScale;
        local_scale.x = math.abs(local_scale.x) * side;
        transform.localScale = local_scale;
    }

    void OnEnable()
    {
        input.Gameplay.Enable();
    }

    void OnDisable()
    {
        input.Gameplay.Disable();
    }
}
