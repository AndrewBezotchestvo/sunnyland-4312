using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 10;
    public float rayLenght = 1;
    public Vector2 rayOffset = Vector2.zero;

    private bool isGround = true;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        animator.SetBool("IsJump", !isGround);

        if (Input.GetKeyDown(KeyCode.W) && isGround == true)
        {
            movement.y = jumpForce;
            rb.linearVelocity = movement;
            //rb.linearVelocityY = jumpForce;
            //rb.AddForce(jumpForce * transform.up);
        }

RaycastHit2D ground = Physics2D.Raycast((Vector2)transform.position + rayOffset, Vector2.down, rayLenght);
        isGround = ground.collider != null;


        if (rb.linearVelocityX > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (rb.linearVelocityX < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }

    }


    void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxis("Horizontal") * speed, rb.linearVelocityY);
        rb.linearVelocity = movement;
    }


}
