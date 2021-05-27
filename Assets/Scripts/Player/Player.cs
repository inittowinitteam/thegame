using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] bool m_noBlood = false;

    [SerializeField]
    private int health;
    private bool alive;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Player m_groundSensor;
    private Sensor_Player m_wallSensorR1;
    private Sensor_Player m_wallSensorR2;
    private Sensor_Player m_wallSensorL1;
    private Sensor_Player m_wallSensorL2;
    private bool m_grounded = false;
    private float m_delayToIdle = 0.0f;
    private float inputX = 0f;
    private bool jumpPressed = false;

    public int Health { get => health; set => health = value; }
    public bool Alive { get => alive; set => alive = value; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            Health--;
            Damage();
        }
        else if (other.gameObject.layer == 9)
        {
            Health = 0;
        }

        if (Health <= 0)
        {
            OnDeath();
        }
    }

    private void Damage()
    {
        m_animator.SetTrigger("Hurt");
        FindObjectOfType<AudioSystem>().Play("PlayerDamage");
    }

    private void OnDeath()
    {
        alive = false;
        FindObjectOfType<AudioSystem>().Play("PlayerDeath");
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetTrigger("Death");
    }

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_Player>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_Player>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_Player>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_Player>();
    }

    void Update ()
    {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0) GetComponent<SpriteRenderer>().flipX = false;
        else if (inputX < 0) GetComponent<SpriteRenderer>().flipX = true;

        //Wall Slide
        m_animator.SetBool("WallSlide", (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State()));   

        //Jump
        if (Input.GetButtonDown("Jump") && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            jumpPressed = true;
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0) m_animator.SetInteger("AnimState", 0);
        }
    }

    private void FixedUpdate()
    {
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        if (jumpPressed)
        {
            jumpPressed = false;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        }
        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);
    }
}