
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    private enum state{
        None,
        OnChasing,
        OnPatrolling,
        freezing,
        OnTakingDamage
    }
    [SerializeField] float freezingDuration;
    [SerializeField] float circleCastRadius;
    [SerializeField] float circleCastDistance;
    [SerializeField] float forcingSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float speed;
    [SerializeField] float takeDamageEffectDuration;
    [SerializeField] Transform PatrolPoint;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] private int Health;
    [SerializeField] private float limitPower;
    private state currentState;
    private float freezingTimer;
    private float takeDamageEffectTimer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        currentState = state.OnPatrolling;
    }




    private void Update()
    {
        Debug.Log(rb2d.velocity.x);
        Flip();
        if(currentState == state.OnPatrolling)
        {
            
            MoveToPatrolPoint();
            if(IsPlayerDetected()) currentState = state.OnChasing;
        }

        if(currentState == state.freezing)
        {
            freezingTimer += Time.deltaTime;
            if(freezingTimer > freezingDuration)
            {
                currentState = state.OnPatrolling;
                freezingTimer = 0;
            } 
            if(IsPlayerDetected()) currentState = state.OnChasing;
        }


        if(currentState == state.OnChasing)
        {
            MoveToPlayer();
            if(!IsPlayerDetected()) currentState = state.freezing;
        }

        if(currentState == state.OnTakingDamage)
        {
            takeDamageEffectTimer += Time.deltaTime;
            if(takeDamageEffectTimer > takeDamageEffectDuration)
            {
                takeDamageEffectTimer = 0;
                currentState = state.OnPatrolling; 
            }
            TakeDamage();
        }

        if(Health == 0)
            Destroy(gameObject);


        //Debug.Log(GameObject.Find("Mobile").GetComponent<Rigidbody2D>().velocity);
                            
    }





    private void MoveToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position ,
        GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position , chaseSpeed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player") LevelManager.instance.isPlayerDead = true;
        else if(other.gameObject.tag == "Mobile" && currentState != state.OnTakingDamage)
        {
            if(GameObject.Find("Mobile").GetComponent<Rigidbody2D>().velocity.x > limitPower ||
            GameObject.Find("Mobile").GetComponent<Rigidbody2D>().velocity.x < -limitPower 
            || GameObject.Find("Mobile").GetComponent<Rigidbody2D>().velocity.y > limitPower ||
            GameObject.Find("Mobile").GetComponent<Rigidbody2D>().velocity.y < -limitPower)
            {
                currentState = state.OnTakingDamage;
                Health--;
            }
        }
    }
    private void TakeDamage()
    {
        if(GameObject.FindGameObjectWithTag("Player").transform.position.x - 
        transform.position.x < 0) 
            transform.position = new Vector2(transform.position.x + forcingSpeed * Time.deltaTime , transform.position.y);
        else if(GameObject.FindGameObjectWithTag("Player").transform.position.x - 
        transform.position.x > 0) 
            transform.position = new Vector2(transform.position.x - forcingSpeed * Time.deltaTime , transform.position.y);
    }

    private bool IsPlayerDetected()=>Physics2D.CircleCast(transform.position , circleCastRadius , Vector2.left , circleCastDistance , playerLayer);
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position , circleCastRadius);
    }

    private void MoveToPatrolPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position , PatrolPoint.position , speed);
    }

    private void Flip()
    {
        if(rb2d.velocity.x > 0 ) spriteRenderer.flipX = false;
        else if (rb2d.velocity.x < 0) spriteRenderer.flipX = true;
    }
}
