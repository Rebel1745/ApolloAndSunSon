using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D_old : MonoBehaviour
{

    public Rigidbody2D rb;

    void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        lastTapTimeD = 0;
        lastTapTimeA = 0;
        currentSpeed = Speed;
    }

    public float Speed = 3f;
    public float SprintSpeed = 4f;
    float currentSpeed;
    public float JumpForce = 10f;                          // Amount of force added when the player jumps.
    public float DashForce = 50f;
    [Range(0, 1)] public float CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] public float MovementSmoothing = .05f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    float move;
    bool facingRight = true;  // For determining which way the player is currently facing.
    bool jump = false;
    bool crouch = false;

    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public int extraJumps;
    int extraJumpValue;

    public float tapSpeed = 0.5f; //in seconds
    private float lastTapTimeD = 0;
    private float lastTapTimeA = 0;

    public float JumpTime;
    private float jumpTimeCounter;

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // if we are on the ground, we can jump
        if (isGrounded)
        {
            extraJumpValue = extraJumps;
        }

        // Sprint
        if(Input.GetButton("Sprint"))
        {
            currentSpeed = SprintSpeed;
        }
        else
        {
            currentSpeed = Speed;
        }
        // END Sprint

        // Jump
        // TODO Allow x axis movement when jumping
        if (Input.GetButtonDown("Jump") && extraJumpValue > 0)
        {
            jump = true;
            jumpTimeCounter = JumpTime;
            rb.velocity = Vector2.up * JumpForce;
            extraJumpValue--;
        } else if(Input.GetButtonDown("Jump") && extraJumpValue == 0 && isGrounded)
        {
            jump = true;
            jumpTimeCounter = JumpTime;
            rb.velocity = Vector2.up * JumpForce;
        }

        // if we hold down Jump, go higher
        if (Input.GetButton("Jump") && jump)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * JumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                jump = false;
            }
        }
        if(Input.GetButtonUp("Jump"))
        {
            jump = false;
        }

        // if jumping or falling, alter gravity to allow for medium and large jumps
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
        // END Jump

        // Crouch
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
        // END Crouch

        // dash (double tap)
        if (Input.GetKeyDown(KeyCode.D))
        {
            if ((Time.time - lastTapTimeD) < tapSpeed)
            {
                Debug.Log("Dash Right");
                Dash(true);
            }

            lastTapTimeD = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if ((Time.time - lastTapTimeA) < tapSpeed)
            {
                Debug.Log("Dash Left");
                Dash(false);
            }

            lastTapTimeA = Time.time;
        }
        // end dash

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    void FixedUpdate()
    {
        move = Input.GetAxisRaw("Horizontal") * currentSpeed;
        rb.velocity = new Vector2(move * currentSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
       
    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }
    void Dash(bool dashRight)
    {
        if (dashRight)
            rb.AddForce(Vector2.right * DashForce);
        else
            rb.AddForce(Vector2.left * DashForce);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}