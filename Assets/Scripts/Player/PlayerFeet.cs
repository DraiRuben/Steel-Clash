using UnityEngine;
using System;
public class PlayerFeet : MonoBehaviour
{
    [NonSerialized] public bool IsGrounded;
    [NonSerialized] public BoxCollider2D Collider;
    [NonSerialized] public Platform CurrentPlatform;
    private PlayerActionExecutor m_executor;
    private InputBuffer m_inputBuffer;
    private void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
        m_executor = transform.root.GetComponent<PlayerActionExecutor>();
        m_inputBuffer = transform.root.GetComponent<InputBuffer>();
    }
    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.CompareTag("Ground"))
        {
            IsGrounded = true;
            m_executor.CurrentJumpAmount = 0;
            m_inputBuffer.ResetAirUses();
            CurrentPlatform = _collider.GetComponent<Platform>();
        }
    }
    private void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.CompareTag("Ground"))
        {
            IsGrounded = false;
            CurrentPlatform = null;
        }
    }
}
