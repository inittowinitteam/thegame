using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{


    [Range(1, 10)]
    public float jumpVelocity;

    public float groundThreshold = 0.05f, scale = 1, hangTime = .2f;
    public LayerMask mask;

    private float hangCounter;

    public float speed = 4.0f, jumpForce = 6.0f;
    private Vector2 jump, jumpBox, playerSize, movement;
    public bool jumpPressed = false, grounded;
    private Rigidbody2D rigidBody;
    private float force = 0;

    public float lastX, lastVeloX;
    public float veloY = 0;


    void Awake()
    {
        jump = new Vector2(0.0f, jumpForce);
        rigidBody = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<CapsuleCollider2D>().size;
        jumpBox = new Vector2((playerSize.x * scale) - 0.05f, groundThreshold);
    }

    void Update()
    {
        // float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && hangCounter > 0)
        {
            jumpPressed = true;
        }

        force = Input.GetAxis("Horizontal");
        //movement = new Vector2(Input.GetAxis("Horizontal"), 0);

        //transform.Translate(horizontal, 0, 0);
    }

    private void FixedUpdate()
    {
        ProcessPlayerMove();
        ProcessPlayerJump();
        ProcessPlayerGravity();
    }


    private void ProcessPlayerJump()
    {
        if (jumpPressed)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.AddForce(jump, ForceMode2D.Impulse);
            jumpPressed = false;
            grounded = false;
        }
        else
        {
            Vector2 centrePoint = (Vector2)transform.position + Vector2.down * (playerSize.y + jumpBox.y) * 0.5f * scale;
            grounded = Physics2D.OverlapBox(centrePoint, jumpBox, 0f, mask) != null;
        }
        if (rigidBody.velocity.y > 8.8)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce-0.2f);
        }
    }

    private void ProcessPlayerMove()
    {
        lastVeloX = rigidBody.velocity.x;
        lastX = rigidBody.transform.position.x;
        if (grounded)
        {
            movement = new Vector2(force * speed, rigidBody.velocity.y);
            hangCounter = hangTime;
        }
        else
        {
            if (rigidBody.velocity.x == 0)
            {
                movement.x = 0;
            }
            float max = speed * 0.5f, min = -max, actual = force * speed;
            if (movement.x < min)
            {
                min = movement.x;
            }
            else if (movement.x > max)
            {
                max = movement.x;
            }
            movement.x = Mathf.Clamp(movement.x += force * speed * 0.1f, min, max);

            movement.y = rigidBody.velocity.y;
            if (rigidBody.position.y >= rigidBody.position.y)
            {
                hangCounter -= Time.deltaTime;
            }
            else
            {
                hangCounter = 0;
            }
        }
        rigidBody.velocity = movement;
    }

    private void ProcessPlayerGravity()
    {
        rigidBody.gravityScale = rigidBody.velocity.y < 5 ? 5 : 1;
    }
}
