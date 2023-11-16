using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActionExecutor : MonoBehaviour
{
    [SerializeField] private JumpInfo m_jumpInfo;
    [SerializeField] private MovementInfo m_movementInfo;
    public PlayerFeet Feet;

    private float m_timeSinceMovementInput = 0f;
    private float m_timeSinceLastJump = 0f;
    private float m_playerRot = 0f;

    [NonSerialized] public int CurrentJumpAmount = 0;
    [NonSerialized] public bool DontOverrideVelX = false;
    [NonSerialized] public bool HasBeenHit = false;
    [NonSerialized] public float HitDirX = -1;

    private PlayerAnimationManager m_animationManager;
    private PlayerInputMapper m_player;
    private PlayerInputAction m_currentAction;
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
        public float AttackGroundDrag;
        public float FastFallGravityScale;
        public AnimationCurve MovementEvolution;
    }
    private bool CanUseAction(PlayerInputActionType actionType)
    {
        PlayerInputAction action = m_inputBuffer.ActionInputBuffer[actionType];
        if (!m_inputBuffer.CanAct || (!Feet.IsGrounded && action.AirUses >= action.MaxAirUses)) return false;

        action.AirUses++;
        return true;
    }
    private void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            m_timeSinceLastJump += Time.fixedDeltaTime;
            //fast fall
            if (m_player.Rb.gravityScale != m_jumpInfo.JumpHeldGravityScale)
            {
                m_player.Rb.gravityScale = m_player.PlayerMovementInput.y < 0 && m_inputBuffer.CanAct ? m_movementInfo.FastFallGravityScale : 1;
            }
            if (m_player.PlayerMovementInput.x != 0f
                && !(Feet.IsGrounded && !m_inputBuffer.CanAct))
            {

                if (m_currentAction == null || m_currentAction.DirectionChangesUsed < m_currentAction.AllowedDirectionChanges)
                {
                    if (m_player.PlayerMovementInput.x > 0.5f)
                    {
                        m_playerRot = 0f;
                    }
                    else if (m_player.PlayerMovementInput.x < -0.5f)
                    {
                        m_playerRot = 180f;
                    }
                    if (m_playerRot != transform.rotation.eulerAngles.y)
                    {
                        m_player.IsLookingRight = m_playerRot == 0f;
                        if (m_currentAction != null)
                            m_currentAction.DirectionChangesUsed++;
                        transform.rotation = Quaternion.Euler(0, m_playerRot, 0);
                    }
                }
                if (HasBeenHit && Mathf.Sign(m_player.Rb.velocity.x) != HitDirX)
                {
                    HasBeenHit = false;
                }
                //if we suddenly change directions, don't keep the current curve's value, reset it as if we stopped moving
                //so that the player doesn't have sudden changes in movement and teleports around
                if (m_player.PlayerMovementInput.x > 0 && m_player.Rb.velocity.x < 0
                    || m_player.PlayerMovementInput.x < 0 && m_player.Rb.velocity.x > 0)
                {
                    m_timeSinceMovementInput = 0f;
                    if (Feet.IsGrounded || (!Feet.IsGrounded && !HasBeenHit))
                        m_player.Rb.velocity = new(m_player.Rb.velocity.x * 0.85f, m_player.Rb.velocity.y);

                }
                else
                {
                    m_timeSinceMovementInput += Time.fixedDeltaTime / m_movementInfo.TimeToReachMaxSpeed;
                }
                float MovementSpeed = (Feet.IsGrounded ? m_movementInfo.GroundMovementSpeed : m_movementInfo.AirMovementSpeed);
                float AddVelX = DontOverrideVelX ? m_player.Rb.velocity.x :
                        m_player.PlayerMovementInput.normalized.x * MovementSpeed * (1 / m_movementInfo.MovementEvolution.Evaluate(m_timeSinceMovementInput));
                if (Mathf.Abs(m_player.Rb.velocity.x + AddVelX) > MovementSpeed)
                {
                    AddVelX = (MovementSpeed - Mathf.Abs(m_player.Rb.velocity.x)) * Mathf.Sign(m_player.PlayerMovementInput.x);
                }
                m_player.Rb.AddForce(new Vector2(AddVelX, 0), ForceMode2D.Force);

                if (m_inputBuffer.CanAct)
                    m_animator.SetInteger("State", 1);
            }
            else
            {
                m_timeSinceMovementInput = 0f;
                m_player.Rb.velocity = Mathf.Abs(m_player.Rb.velocity.x) > 0.5f ?
                    new(m_player.Rb.velocity.x * (1 - (Feet.IsGrounded ? m_inputBuffer.CanAct ? m_movementInfo.GroundDrag : m_movementInfo.AttackGroundDrag : m_movementInfo.AirDrag) * Time.fixedDeltaTime), m_player.Rb.velocity.y)
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
                if (m_timeSinceLastJump >= m_jumpInfo.JumpCooldown)
                {
                    m_animator.SetInteger("State", (int)actionType + 4);
                    m_player.Rb.velocity = new(m_player.Rb.velocity.x, 0); //resets y so that the impulse is the same when falling and on ground
                    HasBeenHit = false;
                    m_player.Rb.AddForce(Vector2.up * m_jumpInfo.JumpStrength, ForceMode2D.Impulse);
                    CurrentJumpAmount++;
                    StartCoroutine(TryHoldJump());
                }
                break;
            default:
                m_animator.SetInteger("State", (int)actionType + 4); //0 for idle, 1 for walk, 2 for hurt, 3 for dizzy
                m_animationManager.ActionInfo = (actionType, actionInfo);
                m_currentAction = actionInfo;
                if(actionInfo.AudioPlayer != null)
                {
                    actionInfo.AudioPlayer.clip = actionInfo.SfxToPlay[UnityEngine.Random.Range(0,actionInfo.SfxToPlay.Count)];
                    actionInfo.AudioPlayer.Play();
                }
                actionInfo.DirectionChangesUsed = 0;
                if (actionInfo.Weapon != null)
                {
                    actionInfo.Weapon.Damage = actionInfo.Damage;
                    actionInfo.Weapon.IsCounter = actionType == PlayerInputActionType.Counter;
                }
                StartCoroutine(ActionStun(actionInfo));
                break;
        }
    }
    public void Hurt(int damageReceived)
    {
        m_animator.speed = 4f / damageReceived;
        StartCoroutine(ActionStun());
    }
    public void CounterStun()
    {
        StopAllCoroutines();
        StartCoroutine(ActionStun());
    }
    private IEnumerator ActionStun(PlayerInputAction actionInfo = null)
    {
        m_inputBuffer.CanAct = false;
        int animationState = m_animator.GetInteger("State");
        if (actionInfo != null && actionInfo.VelocityChange != Vector2.zero)
        {
            m_player.Rb.velocity = Vector2.zero;
            m_player.Rb.AddForce(actionInfo.VelocityChange, ForceMode2D.Impulse);
            DontOverrideVelX = actionInfo.VelocityChange.x != 0;
        }
        yield return new WaitWhile(() => animationState == m_animator.GetInteger("State"));
        m_currentAction = null;
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
