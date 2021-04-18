using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour {


    [Range(1, 10)]
    public float jumpVelocity;

    public float groundThreshold = 0.05f, scale = 1, hangTime =.2f;
    public LayerMask mask;

    public float hangCounter, lastYw;

    public float speed = 4.0f, jumpForce = 6.0f;
    private Vector2 jump, jumpBox, playerSize, movement;
    private bool jumpPressed = false, grounded;
    private Rigidbody2D rigidBody;
    private float force = 0;


    void Awake()
    {
        jump = new Vector2(0.0f, jumpForce);
        rigidBody = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<BoxCollider2D>().size;
        jumpBox = new Vector2(playerSize.x * scale, groundThreshold);
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
            rigidBody.AddForce(jump, ForceMode2D.Impulse);
            jumpPressed = false;
            grounded = false;
        }
        else
        {
            Vector2 centrePoint = (Vector2)transform.position + Vector2.down * (playerSize.y + jumpBox.y) * 0.5f * 0.2f;
            grounded = Physics2D.OverlapBox(centrePoint, jumpBox, 0f, mask) != null;
        }
    }

    private void ProcessPlayerMove()
    {
        if (grounded)
        {
            rigidBody.velocity = new Vector2(force * speed, rigidBody.velocity.y);
            lastYw = rigidBody.position.y;
            hangCounter = hangTime;
        }
        else
        {
            if (lastYw >= rigidBody.position.y)
            {
                hangCounter -= Time.deltaTime;
            }
            else
            {
                hangCounter = 0;
            }

        }
        
    }

    private void ProcessPlayerGravity()
    {
        float lastY = rigidBody.velocity.y, scale;

        if (lastY < 0)
        {
            scale = 2.5f;
        }
        else if (lastY > 0 && !Input.GetButton("Jump"))
        {
            scale = 2;
        }
        else
        {
            scale = 1;
        }
        rigidBody.gravityScale = scale;
    }
}
