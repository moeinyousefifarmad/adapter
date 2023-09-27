
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRayDistance;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        if (IsOnGround() && Input.GetButtonDown("Jump")) Jump();
    }
    private void Move()
    {
        rb2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed , rb2d.velocity.y);
    }

    private void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x , jumpPower);
    }

    private bool IsOnGround() => Physics2D.Raycast(groundCheckPos.position , Vector2.down , groundCheckRayDistance , groundLayer);
}
