using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private int health = 3;
    private bool onGround = false, jumpPressed;
    private new Rigidbody2D rigidbody;
    private float horizontal = 0f;
    public float speed, jumpForce;
    public bool alive = true;
    private Vector2 jump;
    public GameObject jumpBox;
    public Vector2 spawn;

    private void Awake()
    {
        jump = new Vector2(0.0f, jumpForce);
        rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.blue;
    }

    private void Start()
    {
        Respawn();
    }

    private SpriteRenderer m_SpriteRenderer;
    void Update()
    {
        m_SpriteRenderer.color = onGround ? new Color(0, 255, 0) : new Color(255, 0, 0);
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump"))
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            if (onGround)
            {
                onGround = false;
                rigidbody.AddForce(jump, ForceMode2D.Impulse);
            }
        }
        float y = rigidbody.velocity.y;
        rigidbody.velocity = new Vector2(horizontal * speed, y);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        ModGravity(coll, true);
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        ModGravity(coll, false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            health--;
            Debug.Log("Damage: " + health + " remaining");
        }
        else if (other.gameObject.layer == 9)
        {
            health = 0;
        }

        if (health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        alive = false;
        Debug.Log("Dead!");
        Respawn();
    }

    private void Respawn()
    {
        alive = true;
        health = 3;
        rigidbody.position = spawn;
    }

    private void ModGravity(Collision2D coll, bool enter)
    {
        if (coll.gameObject.layer == 7) onGround = enter;
    }
}