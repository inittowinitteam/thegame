using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool vertical;
    public float min, max;
    private float current;
    private Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float working = vertical ? transform.position.y : transform.position.x;

        if (working >= max) current = min;
        else if (working <= min) current = max;

        float offset = (current - working < 0 ? -1 : 1) * 4;
        rigidbody.velocity = vertical ? new Vector2(0, offset) : new Vector2(offset, 0);
    }
}
