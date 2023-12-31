using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PolygonCollider2D m_collider;
    private void Awake()
    {
        m_collider = GetComponent<PolygonCollider2D>();
    }

    public void GoThrough()
    {
        m_collider.enabled = false;
        StartCoroutine(EnablableLate());
    }
    private IEnumerator EnablableLate()
    {
        yield return new WaitForSeconds(0.5f);
        m_collider.enabled = true;
    }
}
