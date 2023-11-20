using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputMapper : MonoBehaviour
{
    [SerializeField] private PlayerFeet m_feet;
    [SerializeField] private GameObject m_body;
    [NonSerialized] public Rigidbody2D Rb;
    [NonSerialized] public Vector2 PlayerMovementInput;
    [NonSerialized] public bool IsHoldingJump = false;
    [NonSerialized] public bool IsLookingRight = true;

    private PlayerTrail m_playerTrail;
    private InputBuffer m_inputBuffer;

    private int m_playerLayer;
    private float m_timeSinceDash;

    private void Awake()
    {
        m_playerTrail = GetComponent<PlayerTrail>();
        Rb = GetComponent<Rigidbody2D>();
        m_inputBuffer = GetComponent<InputBuffer>();
        m_playerLayer = m_body.layer;

    }
    public void Movement(InputAction.CallbackContext ctx)
    {
        PlayerMovementInput = ctx.ReadValue<Vector2>();

    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            IsHoldingJump = true;
            m_inputBuffer.TryDoAction(PlayerInputActionType.Jump);
        }
        if (ctx.canceled)
        {
            IsHoldingJump = false;
        }
    }
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(!m_feet.IsGrounded && PlayerMovementInput.y < -0.6f)
            {
                m_inputBuffer.TryDoAction(PlayerInputActionType.DownAttack);

            }
            else if(PlayerMovementInput.y > 0.6f)
            {
                m_inputBuffer.TryDoAction(PlayerInputActionType.UpAttack);

            }
            else
            {
                m_inputBuffer.TryDoAction(PlayerInputActionType.Attack);
            }
        }
    }
    public void SpecialAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (PlayerMovementInput.y > 0.6f)
            {
                m_inputBuffer.TryDoAction(PlayerInputActionType.UpSpecialAttack);
            }
            else
            {
                m_inputBuffer.TryDoAction(PlayerInputActionType.SpecialAttack);
            }
        }
    }
    public void Counter(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            m_inputBuffer.TryDoAction(PlayerInputActionType.Counter);
        }
    }
    public void GoThroughPlatform(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && m_feet.IsGrounded && m_feet.CurrentPlatform!=null)
        {
            m_feet.CurrentPlatform.GoThrough();
        }
    }
    public void Dash(InputAction.CallbackContext ctx)
    {
        if (m_feet.IsGrounded && Time.time-m_timeSinceDash>1.5f)
        {
            m_timeSinceDash = Time.time;
            m_inputBuffer.CanDash = true;
        }
        if (ctx.performed && m_inputBuffer.CanAct && m_inputBuffer.CanDash && PlayerMovementInput.normalized.magnitude !=0)
        {
            m_timeSinceDash = Time.time;
            Rb.velocity = PlayerMovementInput.normalized * 20f;
            m_inputBuffer.CanDash = false;
            m_playerTrail.StartAfterimageTrail(0.5f, 6);
            StartCoroutine(SetLayer(m_playerLayer));
            //timer is not reset, which means m_body.layer is playerintangible if the dash is done before coroutine end
            m_body.layer = LayerMask.NameToLayer("PlayerIntangible");
            
        }
    }
    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed & LevelEnd.Instance.gameObject.activeSelf == false)
        {
            PauseMenuHandler.Instance.PauseGame();
        }
    }
    private IEnumerator SetLayer(int _layer)
    {
        yield return new WaitForSeconds(1.2f);
        m_body.layer = _layer;
    }
}