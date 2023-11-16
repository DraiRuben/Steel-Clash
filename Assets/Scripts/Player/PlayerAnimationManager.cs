using System;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [NonSerialized] public bool ReduceRecovery;
    private AnimationState m_currentAnimationState;
    public (PlayerInputActionType type, PlayerInputAction action) ActionInfo;
    private Animator m_animator;
    private PlayerHealth m_playerHealth;

    private void Awake()
    {
        m_playerHealth = GetComponent<PlayerHealth>();
        m_animator = GetComponent<Animator>();
    }
    public void UpdateAnimatorSpeed(int isNewAnimation = 0)
    {
        m_playerHealth.IsCountering = false;
        m_currentAnimationState = isNewAnimation == 1 ? AnimationState.StartupFrames : m_currentAnimationState.Next();
        AnimationFrameInfo animationFrameInfo =
            m_currentAnimationState == AnimationState.StartupFrames ? ActionInfo.action.StartupFrameInfo :
            m_currentAnimationState == AnimationState.ActiveFrames ? ActionInfo.action.ActiveFrames :
            ActionInfo.action.RecoveryFrames;
        float targetSpeed = (float)animationFrameInfo.FrameAnimationAmount / ((float)animationFrameInfo.FrameDuration / 5);
        //for when an attack hit
        if (m_currentAnimationState == AnimationState.RecoveryFrames && ReduceRecovery)
        {
            targetSpeed *= 2;
        }
        if (ActionInfo.type == PlayerInputActionType.Counter)
        {
            m_playerHealth.IsCountering = m_currentAnimationState == AnimationState.ActiveFrames;
        }
        m_animator.speed = targetSpeed;
    }
    public void EndAnimation()
    {
        m_animator.SetInteger("State", 0);
        ResetAnim();
    }
    public void ResetAnim()
    {
        m_animator.speed = 1f;
        ReduceRecovery = false;
    }
    public enum AnimationState
    {
        StartupFrames,
        ActiveFrames,
        RecoveryFrames
    }
}

//https://stackoverflow.com/questions/642542/how-to-get-next-or-previous-enum-value-in-c-sharp
//thanks random guy
public static class Extensions
{
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }
}
