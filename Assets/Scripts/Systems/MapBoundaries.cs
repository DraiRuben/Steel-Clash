using UnityEngine;
using UnityEngine.InputSystem;

public class MapBoundaries : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If any player goes outside the view of the camera
        if(!collision.isTrigger && collision.CompareTag("Player"))
        {
            PlayerHealth _health = collision.transform.root.GetComponent<PlayerHealth>();

            // Play the death SFX
            SoundEffectHandler.Instance.PlaySoundEffect(SoundEffectHandler.SoundEffectEnum.death);

            // The player will not lose a life when he is alone
            if (PlayerInputManager.instance.playerCount < 2)
            {
                SpawnManager.instance.PutPlayerAtSpawnPoint(5, collision.transform.root.gameObject);
                _health.Percentage = 0;
            }
            else
            {
                _health.Percentage = 0;
                _health.Lives--;
                SpawnManager.instance.PutPlayerAtSpawnPoint(5, collision.transform.root.gameObject);
            }
        }
    }
}
