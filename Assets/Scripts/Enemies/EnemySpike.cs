
using UnityEngine;

public class EnemySpike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag =="Player") LevelManager.instance.isPlayerDead = true;
    }
}
