using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{

    public Rigidbody2D rb;

    void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    public float Speed = 40f;
    public float JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] public float CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] public float MovementSmoothing = .05f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    float move;
    bool facingRight = true;  // For determining which way the player is currently facing.
    Vector3 velocity = Vector3.zero;
    bool jump = false;
    bool crouch = false;

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal") * Speed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        // if jumping or falling, alter gravity to allow for medium and large jumps
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f * Time.fixedDeltaTime, rb.velocity.y);
        // And then smoothing it out and applying it to the character
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, MovementSmoothing);

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

        if (jump)
        {
            rb.AddForce(new Vector2(0f, JumpForce));
            jump = false;
        }
        
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
}