using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlayerFeet Target;
    private PolygonCollider2D m_collider;
    private void Awake()
    {
        m_collider = GetComponent<PolygonCollider2D>();
    }
    void Update()
    {
        if(m_collider.bounds.max.y < Target.Collider.bounds.max.y)
        {
            m_collider.isTrigger = false;
        }
        else
        {
            m_collider.isTrigger = true;
        }
    }
}
