using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputMapper : MonoBehaviour
{
    [SerializeField] private PlayerFeet m_feet;
    [NonSerialized] public Rigidbody2D Rb;
    [NonSerialized] public Vector2 PlayerMovementInput;
    [NonSerialized] public bool IsHoldingJump = false;
    [NonSerialized] public bool IsLookingRight = true;

    private InputBuffer m_inputBuffer;
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        m_inputBuffer = GetComponent<InputBuffer>();

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
    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed & LevelEnd.Instance.gameObject.activeSelf == false)
        {
            PauseMenuHandler.Instance.PauseGame();
        }
    }
}