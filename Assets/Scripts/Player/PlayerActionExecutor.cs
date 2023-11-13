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
    private int m_currentJumpAmount = 0;
    private float m_timeSinceMovementInput = 0f;
    private float m_timeSinceLastJump = 0f;

    private PlayerAnimationManager m_animationManager;
    private PlayerInputMapper m_player;
    private InputBuffer m_inputBuffer;
    private SpriteRenderer[] m_spriteRenderers;
    private Animator m_animator;
    private void Awake()
    {
        m_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
        public float GroundMovementSpeed;
        public float AirMovementSpeed;
        public float TimeToReachMaxSpeed;
        public float GroundDrag;
        public float AirDrag;
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
        if (Time.timeScale != 0)
        {
            if(m_player.m_playerMovementInput.x != 0f && m_inputBuffer.CanAct)
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
                    m_player.m_playerMovementInput.x * m_movementInfo.GroundMovementSpeed * m_movementInfo.m_movementCurve.Evaluate(m_timeSinceMovementInput),
                    m_player.Rb.velocity.y);

                if (m_inputBuffer.CanAct)
                    m_animator.SetInteger("State", 1);
            }
            else
            {
                m_timeSinceMovementInput = 0f;
                //need to implement air drag
                m_player.Rb.velocity = Mathf.Abs(m_player.Rb.velocity.x) > 0.5f? new(m_player.Rb.velocity.x * (1 - m_movementInfo.GroundDrag * Time.fixedDeltaTime), m_player.Rb.velocity.y): new(0, m_player.Rb.velocity.y);
                
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
                    m_player.Rb.velocity = new(m_player.Rb.velocity.x, 0); //resets y so that the impulse is the same when falling and on ground
                    m_player.Rb.AddForce(Vector2.up * m_jumpInfo.JumpStrength, ForceMode2D.Impulse);
                    m_currentJumpAmount++;
                    StartCoroutine(TryHoldJump());
                }
                break;
            default:
                m_animator.SetInteger("State", (int)actionType + 4); //0 for idle, 1 for walk, 2 for hurt, 3 for dizzy
                m_animationManager.m_actionInfo = (actionType,actionInfo);
                m_inputBuffer.CanAct = false;
                StartCoroutine(ActionStun());
                break;
        }
    }
    private IEnumerator ActionStun()
    {

        int animationState = m_animator.GetInteger("State");
        yield return new WaitWhile(()=> animationState == m_animator.GetInteger("State"));
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_currentJumpAmount = 0;
    }
}
