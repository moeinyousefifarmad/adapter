
using UnityEngine;

public class EnemyCrusher : MonoBehaviour
{
    private enum state{
        PullingBackState,
        CrushingMoveState,
        FreezingState
    }
    [SerializeField] private float crushingMoveSpeed;
    [SerializeField] private float pullBackSpeed;
    [SerializeField] private Transform StartingPoint;
    [SerializeField] private Transform FinnalPoint;
    [SerializeField] private float freezingDuration;
    private float freezingTimer;
    private bool isGroundDetected;
    private CameraController cameraController;
    private state currentState;

    private void Start()
    {
        currentState = state.PullingBackState;
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
    }
    private void Update()
    {
        if (currentState == state.PullingBackState)
        {
            PullBack();
            if(transform.position == StartingPoint.position) currentState = state.CrushingMoveState;
        }
        if(currentState == state.CrushingMoveState)
        {
            CrushingMove();
            if(transform.position == FinnalPoint.position)
            {
                currentState = state.FreezingState;
                cameraController.needShake = true;
            } 
        }
        if(currentState == state.FreezingState)
        {
            freezingTimer += Time.deltaTime;
            if(freezingTimer > freezingDuration)
            {
                freezingTimer = 0;
                currentState = state.PullingBackState;
            }
        }
    }
    private void PullBack()
    {
        transform.position = Vector2.MoveTowards(transform.position , StartingPoint.position , pullBackSpeed);
    }
    private void CrushingMove()
    {
        transform.position = Vector2.MoveTowards(transform.position , FinnalPoint.position , crushingMoveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Mobile") LevelManager.instance.isPlayerDead = true;
    }
}
