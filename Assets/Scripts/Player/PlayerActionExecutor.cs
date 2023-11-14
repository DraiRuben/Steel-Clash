using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionExecutor : MonoBehaviour
{
    [SerializeField] private JumpInfo m_jumpInfo;
    [SerializeField] private MovementInfo m_movementInfo;
    public PlayerFeet m_feet;
    [HideInInspector] public int CurrentJumpAmount = 0;
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
        public float JumpHeldGravityScale;
        public float JumpCooldown;
    }
    [Serializable]
    public struct MovementInfo
    {
        public float GroundMovementSpeed;
        public float AirMovementSpeed;
        public float TimeToReachMaxSpeed;
        public float GroundDrag;
        public float AirDrag;
        public AnimationCurve m_movementCurve;
    }
    private bool CanUseAction(PlayerInputActionType actionType)
    {
        PlayerInputAction action = m_inputBuffer.ActionInputBuffer[actionType];
        if (!m_inputBuffer.CanAct || (!m_feet.IsGrounded && action.AirUses>= action.MaxAirUses)) return false;
        
        action.AirUses++;
        return true;
        
    }
    private void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            m_timeSinceLastJump += Time.fixedDeltaTime;
            if (m_player.m_playerMovementInput.x != 0f && m_inputBuffer.CanAct)
            {
                transform.rotation = Quaternion.Euler(0, m_player.m_playerMovementInput.x < 0 ? 180 : 0, 0);

                //if we suddenly change directions, don't keep the current curve's value, reset it as if we stopped moving
                //so that the player doesn't have sudden changes in movement and teleports around
                if(m_player.m_playerMovementInput.x>0 && m_player.Rb.velocity.x<0
                    || m_player.m_playerMovementInput.x<0 && m_player.Rb.velocity.x > 0)
                {
                    m_timeSinceMovementInput = 0f;
                    m_player.Rb.velocity = new(0, m_player.Rb.velocity.y);
                }
                else
                {
                    m_timeSinceMovementInput += Time.fixedDeltaTime / m_movementInfo.TimeToReachMaxSpeed;
                }
                m_player.Rb.velocity = new (
                    m_player.m_playerMovementInput.x * (m_feet.IsGrounded?m_movementInfo.GroundMovementSpeed:m_movementInfo.AirMovementSpeed) * m_movementInfo.m_movementCurve.Evaluate(m_timeSinceMovementInput),
                    m_player.Rb.velocity.y);

                if (m_inputBuffer.CanAct)
                    m_animator.SetInteger("State", 1);
            }
            else
            {
                m_timeSinceMovementInput = 0f;
                m_player.Rb.velocity = Mathf.Abs(m_player.Rb.velocity.x) > 0.5f?
                    new(m_player.Rb.velocity.x * (1 - (m_feet.IsGrounded?m_movementInfo.GroundDrag:m_movementInfo.AirDrag) * Time.fixedDeltaTime), m_player.Rb.velocity.y)
                    : new(0, m_player.Rb.velocity.y);
                
                if (m_inputBuffer.CanAct)
                    m_animator.SetInteger("State", 0);
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
                    m_animator.SetInteger("State", (int)actionType + 4);
                    m_player.Rb.velocity = new(m_player.Rb.velocity.x, 0); //resets y so that the impulse is the same when falling and on ground
                    m_player.Rb.AddForce(Vector2.up * m_jumpInfo.JumpStrength, ForceMode2D.Impulse);
                    CurrentJumpAmount++;
                    StartCoroutine(TryHoldJump());
                }
                break;
            default:
                m_animator.SetInteger("State", (int)actionType + 4); //0 for idle, 1 for walk, 2 for hurt, 3 for dizzy
                m_animationManager.m_actionInfo = (actionType,actionInfo);
                if (actionInfo.Weapon != null) actionInfo.Weapon.Damage = actionInfo.Damage;
                StartCoroutine(ActionStun(actionInfo));
                break;
        }
    }
    public void Hurt(int damageReceived)
    {
        m_animator.speed = damageReceived * 1.5f;
        StartCoroutine(ActionStun());
    }
    private IEnumerator ActionStun(PlayerInputAction actionInfo = null)
    {
        m_inputBuffer.CanAct = false;
        int animationState = m_animator.GetInteger("State");
        if (actionInfo != null && actionInfo.VelocityChange != Vector2.zero)
        {
            m_player.Rb.velocity = Vector2.zero;
            m_player.Rb.AddForce(actionInfo.VelocityChange,ForceMode2D.Impulse);
        }
        yield return new WaitWhile(() => animationState == m_animator.GetInteger("State"));
        
        m_inputBuffer.CanAct = true;
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
}
