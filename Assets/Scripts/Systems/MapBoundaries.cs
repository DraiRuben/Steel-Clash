using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapBoundaries : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.isTrigger && collision.CompareTag("Player"))
        {
            var health = collision.transform.root.GetComponent<PlayerHealth>();

            if (PlayerInputManager.instance.playerCount < 2)
            {
                SpawnManager.instance.PutPlayerAtSpawnPoint(5, collision.transform.root.gameObject);
                health.Percentage = 0;
            }
            else
            {
                health.Percentage = 0;
                health.Lives--;
                SpawnManager.instance.PutPlayerAtSpawnPoint(5, collision.transform.root.gameObject);
            }
        }
    }
}
