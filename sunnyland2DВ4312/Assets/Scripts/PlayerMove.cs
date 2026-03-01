using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 10;

    private bool isGround = false;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxis("Horizontal") * speed, rb.linearVelocityY);
        rb.linearVelocity = movement;
    }


}
