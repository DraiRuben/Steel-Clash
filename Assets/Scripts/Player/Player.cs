using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    private Vector2 m_playerMovementInput;
    public void Movement(InputAction.CallbackContext ctx)
    {
        m_playerMovementInput = ctx.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {

        }
    }
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {

        }
    }
    public void SpecialAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (m_playerMovementInput.y > 0)
            {

            }
        }
    }
    public void Counter(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {

        }
    }
}

