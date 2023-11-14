using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    [HideInInspector] public bool IsGrounded;
    [HideInInspector] public Collider2D Collider;
    private PlayerActionExecutor m_executor;
    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
        m_executor = transform.root.GetComponent<PlayerActionExecutor>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.CompareTag("Ground"))
        {
            IsGrounded = true;
            m_executor.CurrentJumpAmount = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
}
