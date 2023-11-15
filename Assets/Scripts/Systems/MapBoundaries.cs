using UnityEngine;

public class MapBoundaries : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.transform.root.GetComponent<PlayerHealth>();
            health.Percentage = 0;
            health.Lives--;
            SpawnManager.instance.PutPlayerAtSpawnPoint(5, collision.transform.root.gameObject);
        }
    }
}
