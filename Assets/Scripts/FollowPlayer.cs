using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public bool clamp;
    public GameObject player;

    public Vector2 min, max;
    private Vector2 velocity;

    public float smoothTimeX, smoothTimeY;

    private Rigidbody2D rigidBody;
    private Movement hs;
    private void Awake()
    {
        
        rigidBody = player.GetComponent<Rigidbody2D>();
        hs = player.GetComponent<Movement>();
    }

    private void FixedUpdate()
    {
        if (hs.alive)
        {
            float x = player.transform.position.x + (rigidBody.velocity.x), y = player.transform.position.y;

            x = Mathf.SmoothDamp(transform.position.x, x, ref velocity.x, smoothTimeX);
            y = Mathf.SmoothDamp(transform.position.y, y, ref velocity.y, smoothTimeY);

            if (clamp)
            {
                x = Limit(x, min.x, max.x);
                y = Limit(y, min.y, max.y);
            }

            transform.position = new Vector3(x, y, -20);
        }
    }

    private float Limit(float value, float min, float max)
    {
        return value > max ? max : value < min ? min : value;
    }
}
