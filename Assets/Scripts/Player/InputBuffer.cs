using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    public bool CanAct = true;

    private int m_remainingRecoveryFrames = 0;
    private PlayerInputActionType m_currentAction;
    private Dictionary<PlayerInputActionType,PlayerInputAction> ActionInputBuffer = new() 
    {
        { PlayerInputActionType.Counter,(10,3,10,20,4) },
        { PlayerInputActionType.Attack, (10,7,5,6,4) },
        { PlayerInputActionType.SpecialAttack,(10,15,8,20,15) },
        { PlayerInputActionType.UpSpecialAttack,(10,6,10,10,10)},
        { PlayerInputActionType.Jump,(10,-1,-1,-1,4) } //-1 means moves that don't have a hitbox
    };
    public class PlayerInputAction
    {
        public int MaxTimer; // this is a frame ammount, not seconds
        public int TimerSinceInput; //same thing here
        public int StartupFrames;
        public int ActiveFrames;
        public int RecoveryFrames;
        public int Damage;

        public static implicit operator PlayerInputAction((int _refreshAmount, int _startupFrames, int _activeFrames, int _recoveryFrames, int _damage) values )
        {
            return new PlayerInputAction()
            {
                MaxTimer = values._refreshAmount,
                StartupFrames = values._startupFrames,
                ActiveFrames = values._activeFrames,
                RecoveryFrames = values._recoveryFrames,
                Damage = values._damage,
            };
        }
    }
    private void Update()
    {
        if (!CanAct)
        {
            m_remainingRecoveryFrames--;
            if( m_remainingRecoveryFrames <= 0)
            {
                CanAct = true;
                TryExecuteValidAction();
            }
        }
        ReduceActionsTimer();
    }
    public void TryDoAction(PlayerInputActionType action) //called when the player inputs an action
    {
        ActionInputBuffer[action].TimerSinceInput = ActionInputBuffer[action].MaxTimer;
    }
    private void ReduceActionsTimer()
    {
        foreach(PlayerInputAction action in ActionInputBuffer.Values) //reduces timer by 1 frame each frame, until we reach 0
        {
            if(action.TimerSinceInput>0)
                action.TimerSinceInput--;
        }
    }
    private void TryExecuteValidAction()
    {
        var validActions = ActionInputBuffer
            .Where(x=>x.Value.TimerSinceInput>0)
            .OrderBy(x=>x.Value.MaxTimer - x.Value.TimerSinceInput)
            .ToList();
        if(validActions != null && validActions.Count>0)
        {
            foreach(var validAction in validActions)
            {
                if (CanUseAction(validAction.Key))
                {
                    ExecuteAction(validAction.Key);
                    ResetActionsTimer();
                    return;
                }
            }
        }
    }
    private void ExecuteAction(PlayerInputActionType actionType)
    {

    }
    private void ResetActionsTimer()
    {
        foreach(var action in ActionInputBuffer.Values)
        {
            action.TimerSinceInput = 0;
        }
    }
    private bool CanUseAction(PlayerInputActionType actionType)
    {
        if(actionType == PlayerInputActionType.Jump)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
public enum PlayerInputActionType
{
    Counter,
    Attack,
    SpecialAttack,
    UpSpecialAttack,
    Jump,
}