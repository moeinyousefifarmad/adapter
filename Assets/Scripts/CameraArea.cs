
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player")  LevelManager.instance.isPlayerDead = true;
    }
}
