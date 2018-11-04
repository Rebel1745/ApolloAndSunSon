using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        if(sr == null)
        {
            Debug.LogError("No sprite renderer found");
        }
    }

    private void Start()
    {
        currentSpeed = Speed;
        currentDashLengthRight = InitialDashLength;
        currentDashLengthLeft = InitialDashLength;
    }

    Rigidbody2D rb;
    SpriteRenderer sr;

    public float Speed = 3f;
    public float SprintSpeed = 4f;
    float currentSpeed;

    public Transform RightDashPoint;
    public Transform LeftDashPoint;
    public float InitialDashLength = 1f;
    float currentDashLengthRight;
    float currentDashLengthLeft;
    public bool canDashRight = true;
    public bool canDashLeft = true;
    public float DashCooldown = 0.5f;
    public float currentDashCooldown;

    public float JumpForce = 10f;
    public int ExtraJumps;
    int extraJumpValue;
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    float moveInput;
    bool isFacingRight = true;

    public bool isGrounded;
    public Transform GroundCheck;
    public float CheckRadius;
    public LayerMask WhatIsGround;

    private void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, WhatIsGround);

        // Jump
        // if we are on the ground, we can jump
        if (isGrounded)
        {
            extraJumpValue = ExtraJumps;
        }

        if (isGrounded && Input.GetButton("Jump"))
        {
            rb.velocity = Vector2.up * JumpForce;
        }
        if (Input.GetButtonDown("Jump") && extraJumpValue > 0)
        {
            rb.velocity = Vector2.up * JumpForce;
            extraJumpValue--;
        }
        else if (Input.GetButtonDown("Jump") && extraJumpValue == 0 && isGrounded)
        {
            rb.velocity = Vector2.up * JumpForce;
        }

        // if jumping or falling, alter gravity to allow for medium and large jumps
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * FallMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * LowJumpMultiplier * Time.deltaTime;
        }
        // END Jump

        // Sprint
        if (Input.GetButton("Sprint"))
        {
            currentSpeed = SprintSpeed;
        }
        else
        {
            currentSpeed = Speed;
        }
        //

        // Dash
        if (Input.GetKey(KeyCode.D) && currentDashCooldown <= 0)
        {
            if (canDashRight && Input.GetKeyDown(KeyCode.C))
            {
                transform.Translate(new Vector3(currentDashLengthRight, 0f, 0f));
                currentDashCooldown = DashCooldown;
            }
        }
        if (Input.GetKey(KeyCode.A) && currentDashCooldown <= 0)
        {
            if (canDashLeft && Input.GetKeyDown(KeyCode.C))
            {
                transform.Translate(new Vector3(-currentDashLengthLeft, 0f, 0f));
                currentDashCooldown = DashCooldown;
            }
        }

        // lower the dash cooldown
        if (currentDashCooldown > 0)
            currentDashCooldown -= Time.deltaTime;
        else
            currentDashCooldown = 0;
        // End Dash

        // If the input is moving the player right and the player is facing left...
        if (moveInput > 0 && !isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (moveInput < 0 && isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    public void CanDash(int dashDir, bool canDash, float dashLength)
    {
        switch(dashDir)
        {
            case 1:
                canDashRight = canDash;
                currentDashLengthRight = dashLength;
                break;
            case 2:
                canDashLeft = canDash;
                currentDashLengthLeft = dashLength;
                break;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        /*Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;*/

        sr.flipX = !sr.flipX;       
    }
}