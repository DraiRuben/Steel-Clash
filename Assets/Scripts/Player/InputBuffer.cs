using System;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    [HideInInspector] public bool CanAct = true;
    public PlayerMovesDictionary ActionInputBuffer;

    private PlayerActionExecutor m_actionExecutor;

    private void Awake()
    {
        m_actionExecutor = GetComponent<PlayerActionExecutor>();
    }

    private void Update()
    {
        if(Time.timeScale != 0)
        {
            m_actionExecutor.TryExecuteValidAction(ActionInputBuffer);
            ReduceActionsTimer();
        }
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
    public void ResetAirUses()
    {
        foreach(var action in ActionInputBuffer.Values)
        {
            action.AirUses = 0;
        }
    }
}
[Serializable]
public class PlayerInputAction
{
    public int MaxTimer; // this is a frame ammount, not seconds
    [HideInInspector] public int TimerSinceInput; //same thing here
    [HideInInspector] public int AirUses;
    public int MaxAirUses;

    [Header("Frame Data")]
    public AnimationFrameInfo StartupFrameInfo;
    public AnimationFrameInfo ActiveFrames;
    public AnimationFrameInfo RecoveryFrames;

    [Header("Attack")]
    public int Damage;
    public PlayerAttack Weapon;

    [Header("Movement")]
    public Vector2 VelocityChange;
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