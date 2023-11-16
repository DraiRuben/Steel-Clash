using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlayerFeet Target;
    private PolygonCollider2D m_collider;
    private bool m_goThrough;
    private void Awake()
    {
        m_collider = GetComponent<PolygonCollider2D>();
    }
    void Update()
    {
        if (m_collider.bounds.max.y < Target.Collider.bounds.max.y && !m_goThrough)
        {
            m_collider.isTrigger = false;
        }
        else
        {
            m_collider.isTrigger = true;
        }
    }
    public void GoThrough()
    {
        m_goThrough = true;
        m_collider.isTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_goThrough = false;
        }
    }
}
