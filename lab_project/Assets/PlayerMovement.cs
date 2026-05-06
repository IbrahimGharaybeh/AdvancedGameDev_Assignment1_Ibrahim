using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 14f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    Animator anim;

    Rigidbody2D rb;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);

    
    anim.SetFloat("Speed", Mathf.Abs(moveInput));
    anim.SetBool("IsTurning", moveInput != 0 && Mathf.Sign(moveInput) != Mathf.Sign(transform.localScale.x));
    }
}