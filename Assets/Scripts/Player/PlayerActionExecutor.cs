using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionExecutor : MonoBehaviour
{
    [SerializeField] private JumpInfo m_jumpInfo;
    [SerializeField] private MovementInfo m_movementInfo; 
    private PlayerInputActionType m_currentAction;
    private int m_currentJumpAmount = 0;
    private float m_timeSinceMovementInput = 0f;
    private float m_timeSinceLastJump = 0f;

    private PlayerAnimationManager m_animationManager;
    private PlayerInputMapper m_player;
    private InputBuffer m_inputBuffer;
    private Animator m_animator;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_inputBuffer = GetComponent<InputBuffer>();
        m_player = GetComponent<PlayerInputMapper>();
        m_animationManager = GetComponent<PlayerAnimationManager>();
    }
    [Serializable]
    public struct JumpInfo
    {
        public float JumpStrength;
        public int ExtraJumpsAllowed;
        public float JumpHeldGravityScale;
        public float JumpCooldown;
    }
    [Serializable]
    public struct MovementInfo
    {
        public float m_movementSpeed;
        public float m_timeToReachMaxSpeed;
        public AnimationCurve m_movementCurve;
    }
    private bool CanUseAction(PlayerInputActionType actionType)
    {
        if (actionType == PlayerInputActionType.Jump)
        {
            return m_currentJumpAmount < m_jumpInfo.ExtraJumpsAllowed + 1;
        }
        else
        {
            return true;
        }
    }
    private void FixedUpdate()
    {
        m_timeSinceLastJump += Time.fixedDeltaTime;
        if (Time.timeScale != 0 && m_inputBuffer.CanAct)
        {
            if(m_player.m_playerMovementInput.x != 0f)
            {
                //if we suddenly change directions, don't keep the current curve's value, reset it as if we stopped moving
                //so that the player doesn't have sudden changes in movement and teleports around
                if(m_player.m_playerMovementInput.x>0 && m_player.Rb.velocity.x<0
                    || m_player.m_playerMovementInput.x<0 && m_player.Rb.velocity.x > 0)
                {
                    m_timeSinceMovementInput = 0f;
                }
                else
                {
                    m_timeSinceMovementInput += Time.fixedDeltaTime / m_movementInfo.m_timeToReachMaxSpeed;
                }
                m_player.Rb.velocity = new(
                    m_player.m_playerMovementInput.x * m_movementInfo.m_movementSpeed * m_movementInfo.m_movementCurve.Evaluate(m_timeSinceMovementInput),
                    m_player.Rb.velocity.y);
            }
            else
            {
                m_timeSinceMovementInput = 0f;
            }
        }
    }
    private void ExecuteAction(PlayerInputActionType actionType, PlayerInputAction actionInfo)
    {
        switch (actionType)
        {
            case PlayerInputActionType.Jump:
                if(m_timeSinceLastJump>= m_jumpInfo.JumpCooldown)
                {
                    m_player.Rb.velocity = new(m_player.Rb.velocity.x, 0); //resets y so that the impulse is the same when falling and on ground
                    m_player.Rb.AddForce(Vector2.up * m_jumpInfo.JumpStrength, ForceMode2D.Impulse);
                    m_currentJumpAmount++;
                    StartCoroutine(TryHoldJump());
                }
                break;
            default:
                m_animator.SetInteger("State", (int)actionType + 5);
                m_currentAction = actionType;
                break;
        }
    }
    private IEnumerator TryHoldJump()
    {
        while (true)
        {
            if (m_player.IsHoldingJump)
            {
                m_player.Rb.gravityScale = m_jumpInfo.JumpHeldGravityScale;
            }
            else
            {
                m_player.Rb.gravityScale = 1;
                yield break;
            }
            yield return null;
        }
    }
    public void TryExecuteValidAction(PlayerMovesDictionary ActionInputBuffer)
    {
        List<KeyValuePair<PlayerInputActionType, PlayerInputAction>> validActions = ActionInputBuffer
            .Where(x => x.Value.TimerSinceInput > 0)
            .OrderBy(x => x.Value.MaxTimer - x.Value.TimerSinceInput)
            .ToList();
        if (validActions != null && validActions.Count > 0)
        {
            foreach (KeyValuePair<PlayerInputActionType, PlayerInputAction> validAction in validActions)
            {
                if (CanUseAction(validAction.Key))
                {
                    ExecuteAction(validAction.Key, validAction.Value);
                    foreach (PlayerInputAction action in ActionInputBuffer.Values)
                    {
                        action.TimerSinceInput = 0;
                    }
                    return;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_currentJumpAmount = 0;
    }
}
