using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum state{
        idle,
        isShaking
    }
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask CrusherLayer;
    private CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin multiChannel;
    public bool needShake;
    private float shakeTimer;
    private state currentState;
    private void Awake()
    {

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        multiChannel = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        currentState = state.idle;
    }

    private void Update()
    {
        if(IsCrusherDetected())
        Debug.Log("hello");
        if(currentState == state.idle)
        {
            if(needShake && IsCrusherDetected()) currentState = state.isShaking;
        }
        if(currentState == state.isShaking)
        {
            Shake();
            shakeTimer += Time.deltaTime;
            if(shakeTimer > shakeDuration)
            {
                shakeTimer = 0;
                needShake = false;
                multiChannel.m_AmplitudeGain = 0.0f;
                currentState = state.idle;
            }
        }
    }

    public void Shake()
    {
        multiChannel.m_AmplitudeGain = shakeIntensity;
    }

    private bool IsCrusherDetected()=>Physics2D.BoxCast(transform.position , size , 0 , new Vector2(0,0) , 0 , CrusherLayer);
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position , size);
    }
}
