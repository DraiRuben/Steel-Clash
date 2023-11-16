using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private GameObject m_platform;
    //When a new player joins, add a new platform linked to that player
    private void Start()
    {
        PlayerManager.Instance.Platforms.Add(this);
    }
    public void GenerateCollision(int _playerCount)
    {
        GameObject Instantiated = Instantiate(m_platform, transform);
        Instantiated.GetComponent<PlatformEffector2D>().colliderMask = LayerMask.GetMask("Player"+_playerCount);
    }
}
