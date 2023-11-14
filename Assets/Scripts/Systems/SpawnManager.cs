using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> SpawnPoints;

    public static SpawnManager instance;
    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(instance);
    }

    public void PutPlayerAtSpawnPoint(int _playerID, GameObject _player)
    {
        _player.transform.position = SpawnPoints[_playerID - 1].position;
        _player.GetComponent<PlayerHealth>().SpawnInvulnerability();
    }
}
