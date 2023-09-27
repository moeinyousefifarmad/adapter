
using UnityEngine;

public class Cable : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mobile;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0 , player.transform.position);
        lineRenderer.SetPosition(1 , mobile.transform.position);
    }
}
