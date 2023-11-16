using UnityEngine;
using UnityEngine.InputSystem;

public class MapBoundaries : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D _collider)
    {
        // If any player goes outside the view of the camera
        if(!_collider.isTrigger && _collider.CompareTag("Player"))
        {
            var _health = _collider.transform.root.GetComponent<PlayerHealth>();

            // Play the death SFX
            SoundEffectHandler.Instance.PlaySoundEffect(SoundEffectHandler.SoundEffectEnum.death);

            // The player will not lose a life when he is alone
            if (PlayerInputManager.instance.playerCount < 2)
            {
                SpawnManager.Instance.PutPlayerAtSpawnPoint(5, _collider.transform.root.gameObject);
                _health.Percentage = 0;
            }
            else
            {
                _health.Percentage = 0;
                _health.Lives--;
                SpawnManager.Instance.PutPlayerAtSpawnPoint(5, _collider.transform.root.gameObject);
            }
        }
    }
}
