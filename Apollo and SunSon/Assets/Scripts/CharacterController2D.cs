using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentSpeed = Speed;
        currentDashLengthRight = InitialDashLength;
        currentDashLengthLeft = InitialDashLength;
    }

    Rigidbody2D rb;

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
        if (Input.GetKey(KeyCode.D))
        {
            if (canDashRight && Input.GetKeyDown(KeyCode.C))
            {
                transform.Translate(new Vector3(currentDashLengthRight, 0f, 0f));
            }
        }
        if (canDashLeft && Input.GetKey(KeyCode.A))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                transform.Translate(new Vector3(-currentDashLengthLeft, 0f, 0f));
            }
        }
        // End Dash
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
}