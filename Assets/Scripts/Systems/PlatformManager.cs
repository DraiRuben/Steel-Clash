using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private Platform m_platform;
    //When a new player joins, add a new platform linked to that player
    private void Start()
    {
        PlayerManager.instance.Platforms.Add(this);
    }
    public void GenerateCollision(int _playerCount,PlayerFeet target)
    {
        var Instantiated = Instantiate(m_platform.gameObject,transform);
        Instantiated.GetComponent<Platform>().Target = target;
        m_platform.gameObject.layer = LayerMask.NameToLayer($"Player{_playerCount}Platforms");
    }
}
