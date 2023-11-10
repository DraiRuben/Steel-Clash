using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private AnimationState m_currentAnimationState;
    private (PlayerInputActionType type, PlayerInputAction action) m_actionInfo;
    private Animator m_animator;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    public void UpdateAnimatorSpeed(bool isNewAnimation = false) 
    {
        m_currentAnimationState = isNewAnimation? AnimationState.StartupFrames: m_currentAnimationState.Next();
        AnimationFrameInfo animationFrameInfo =
            m_currentAnimationState == AnimationState.StartupFrames ? m_actionInfo.action.StartupFrameInfo :
            m_currentAnimationState == AnimationState.ActiveFrames ? m_actionInfo.action.ActiveFrames :
            m_actionInfo.action.RecoveryFrames;
        float targetSpeed = 0.1f * m_actionInfo.action.ActiveFrames.FrameAnimationAmount / animationFrameInfo.FrameDuration;
        m_animator.speed = targetSpeed;
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
