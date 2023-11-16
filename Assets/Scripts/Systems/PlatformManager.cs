using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private GameObject m_platform;
    //When a new player joins, add a new platform linked to that player
    private void Start()
    {
        PlayerManager.instance.Platforms.Add(this);
    }
    public void GenerateCollision(int _playerCount, PlayerFeet target)
    {
        GameObject Instantiated = Instantiate(m_platform, transform);
        Instantiated.GetComponent<PlatformEffector2D>().colliderMask = LayerMask.GetMask("Player"+_playerCount);
    }
}
