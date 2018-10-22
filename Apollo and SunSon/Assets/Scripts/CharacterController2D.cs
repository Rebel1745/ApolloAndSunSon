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
    }

    Rigidbody2D rb;

    public float Speed = 3f;
    public float SprintSpeed = 4f;
    float currentSpeed;

    public float DashSpeed = 5f;

    public float JumpForce = 10f;
    public int ExtraJumps;
    int extraJumpValue;
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    public float DashForce = 50f;

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
        if(isGrounded && Input.GetButton("Jump"))
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
            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("D");
                /*float velX = rb.velocity.x;
                float newVel = velX + DashForce;
                rb.velocity = new Vector2(newVel, rb.velocity.y);
                rb.AddForce(Vector2.right * DashForce);*/
                //rb.velocity = Vector2.right * DashSpeed;
                transform.Translate(Vector3.right);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("A");
                /*float velX = rb.velocity.x;
                float newVel = velX + DashForce;
                rb.velocity = new Vector2(newVel, rb.velocity.y);
                rb.AddForce(Vector2.left * DashForce);*/
                //rb.velocity = Vector2.left * DashSpeed;
                transform.Translate(Vector3.left);
            }
        }
        // End Dash
    }
}