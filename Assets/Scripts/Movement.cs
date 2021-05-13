using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private int health = 3;
    private bool onGround = false;
    private Rigidbody2D rigidbody;
    private float horizontal = 0f;
    public float speed = 10;
    public bool alive = true;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.blue;
    }
    private SpriteRenderer m_SpriteRenderer;
    void Update()
    {
        m_SpriteRenderer.color = onGround ? new Color(0, 255,0) : new Color(255,0,0);
        horizontal = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        float y = rigidbody.velocity.y;
        rigidbody.velocity = new Vector2(horizontal * speed, onGround ? Mathf.Min(0, y) : y);
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
        } else if (other.gameObject.layer == 9)
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
    }

    private void ModGravity(Collision2D coll, bool enter)
    {
        if (coll.gameObject.layer == 7) onGround = enter;
    }
}