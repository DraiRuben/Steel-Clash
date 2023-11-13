using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private AnimationState m_currentAnimationState;
    public (PlayerInputActionType type, PlayerInputAction action) m_actionInfo;
    private Animator m_animator;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    public void UpdateAnimatorSpeed(int isNewAnimation = 0) 
    {
        m_currentAnimationState = isNewAnimation == 1? AnimationState.StartupFrames: m_currentAnimationState.Next();
        AnimationFrameInfo animationFrameInfo =
            m_currentAnimationState == AnimationState.StartupFrames ? m_actionInfo.action.StartupFrameInfo :
            m_currentAnimationState == AnimationState.ActiveFrames ? m_actionInfo.action.ActiveFrames :
            m_actionInfo.action.RecoveryFrames;
        float targetSpeed = (float)animationFrameInfo.FrameAnimationAmount/ ((float)animationFrameInfo.FrameDuration/5);
        m_animator.speed = targetSpeed;
    }
    public void EndAnimation()
    {
        m_animator.SetInteger("State", 0);
        m_animator.speed = 1f;
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
