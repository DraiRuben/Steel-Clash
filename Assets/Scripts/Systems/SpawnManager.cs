using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> SpawnPoints;

    public static SpawnManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    public void PutPlayerAtSpawnPoint(int _playerID, GameObject _player)
    {
        PlayerHealth _Health = _player.GetComponent<PlayerHealth>();
        if (_Health.Lives > 0)
        {
            _player.transform.position = SpawnPoints[_playerID - 1].position;
            _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _Health.SpawnInvulnerability();
        }
    }
}
