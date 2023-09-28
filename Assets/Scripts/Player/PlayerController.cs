
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private enum State{
        idle,
        run,
        jump,
        fall,
        takeDamage
    }
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float downSpeed;

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRayDistance;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private State currentState;
    private void Awake()
    {
        currentState = State.idle;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        rb2d = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        Flip();

        if(currentState == State.idle)
        {
            if(Input.GetAxisRaw("Horizontal") != 0) currentState = State.run;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                currentState = State.jump;
                animator.SetBool("IsJump" , true);
            }

           // Debug.Log(currentState);
        }

        if(currentState == State.run)
        {
            animator.SetBool("IsRun" , true);
            Move();
            if(Input.GetAxisRaw("Horizontal") == 0)
            {
                currentState = State.idle;
                animator.SetBool("IsRun" , false);
                animator.SetBool("IsIdle" , true);
            }
            if(Input.GetKeyDown(KeyCode.Space) && IsOnGround())
            {
                Jump();
                currentState = State.jump;
                animator.SetBool("IsRun" , false);
                animator.SetBool("IsJump" , true);
            }
//Debug.Log(currentState);
        }

        if(currentState == State.jump)
        {
            Move();
            animator.SetFloat("yVelocity" , rb2d.velocity.y);
            if(rb2d.velocity.y < 0)
            {
                currentState = State.fall;
                animator.SetFloat("yVelocity" , rb2d.velocity.y);
            }
           // Debug.Log(currentState);
        }

        if(currentState == State.fall)
        {
            animator.SetFloat("yVelocity" , rb2d.velocity.y);
            Move();
            if(IsOnGround())
            {
                 Debug.Log("ground detected");
                currentState = State.idle;
                animator.SetBool("IsJump" , false);
                animator.SetBool("IsIdle" , true);
            }
        }

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


    private void Flip()
    {
        if(Input.GetAxisRaw("Horizontal") > 0 ) spriteRenderer.flipX = false;
        else if (Input.GetAxisRaw("Horizontal") < 0) spriteRenderer.flipX = true;
    }

    // private void MoveDown()
    // {
    //     rb2d.velocity = new Vector2(rb2d.velocity.x , -downSpeed);
    // }
}
