
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
    private state currentState;
    private float freezingTimer;
    private float takeDamageEffectTimer;
    
    private void Start()
    {
        currentState = state.OnPatrolling;
    }
    private void Update()
    {
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
            currentState = state.OnTakingDamage;
            Health--;
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
}
