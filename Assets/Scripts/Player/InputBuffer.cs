using System;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    [HideInInspector] public bool CanAct = true;
    [SerializeField] private PlayerMovesDictionary ActionInputBuffer;

    private PlayerActionExecutor m_actionExecutor;

    private int m_remainingRecoveryFrames = 0;
    private void Awake()
    {
        m_actionExecutor = GetComponent<PlayerActionExecutor>();
    }

    private void Update()
    {
        if (!CanAct)
        {
            m_remainingRecoveryFrames--;
            if (m_remainingRecoveryFrames <= 0)
            {
                CanAct = true;
                m_actionExecutor.TryExecuteValidAction(ActionInputBuffer);
            }
        }
        m_actionExecutor.TryExecuteValidAction(ActionInputBuffer);
        ReduceActionsTimer();
    }
    public void TryDoAction(PlayerInputActionType action) //called when the player inputs an action
    {
        ActionInputBuffer[action].TimerSinceInput = ActionInputBuffer[action].MaxTimer;
    }
    private void ReduceActionsTimer()
    {
        foreach (PlayerInputAction action in ActionInputBuffer.Values) //reduces timer by 1 frame each frame, until we reach 0
        {
            if (action.TimerSinceInput > 0)
                action.TimerSinceInput--;
        }
    }
}
[Serializable]
public class PlayerInputAction
{
    public int MaxTimer; // this is a frame ammount, not seconds
    [HideInInspector] public int TimerSinceInput; //same thing here
    public AnimationFrameInfo StartupFrameInfo;
    public AnimationFrameInfo ActiveFrames;
    public AnimationFrameInfo RecoveryFrames;
    public AnimationFrameInfo Damage;
}
[Serializable]
public enum PlayerInputActionType
{
    Counter,
    Attack,
    SpecialAttack,
    UpSpecialAttack,
    Jump,
}
[Serializable]
public struct AnimationFrameInfo
{
    public int FrameDuration;
    public int FrameAnimationAmount;
}