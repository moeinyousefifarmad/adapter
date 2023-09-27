
using UnityEngine;

public class EnemySpike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag =="Player" || other.gameObject.tag == "Mobile") LevelManager.instance.isPlayerDead = true;
    }
}
