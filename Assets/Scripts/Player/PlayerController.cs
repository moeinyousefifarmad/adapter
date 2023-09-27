
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float downSpeed;

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRayDistance;
    [SerializeField] private LayerMask groundLayer;

    private float dashTimer;
    private bool isDashing;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Flip();
        Move();
        if(!IsOnGround() && Input.GetKeyDown(KeyCode.S)) MoveDown(); 
        if (IsOnGround() && Input.GetButtonDown("Jump")) Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift)) isDashing = true;
        if (isDashing) Dash();
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

    private void Dash()
    {
        dashTimer += Time.deltaTime;
        if(dashTimer < dashDuration)
        {
            if (!spriteRenderer.flipX)  rb2d.velocity = new Vector2(dashSpeed , 0);            
            else if (spriteRenderer.flipX) rb2d.velocity = new Vector2(-dashSpeed , 0);
        } 
        else 
        {
            dashTimer = 0;
            isDashing = false;
        } 

    }

    private void Flip()
    {
        if(Input.GetAxisRaw("Horizontal") > 0 ) spriteRenderer.flipX = false;
        else if (Input.GetAxisRaw("Horizontal") < 0) spriteRenderer.flipX = true;
    }

    private void MoveDown()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x , -downSpeed);
    }
}
