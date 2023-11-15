using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public int Damage;
    [HideInInspector] public bool IsCounter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsCounter && !collision.isTrigger && collision.CompareTag("Player"))
        {
            Transform HitPlayer = collision.transform.root;
            var HealthModule = HitPlayer.GetComponent<PlayerHealth>();

            if (!HealthModule.IsInvulnerable)
            {
                if (HealthModule.IsCountering
                    && ((transform.root.position.x< HitPlayer.position.x && !HitPlayer.GetComponent<PlayerInputMapper>().IsLookingRight)
                    || transform.root.position.x > HitPlayer.position.x && HitPlayer.GetComponent<PlayerInputMapper>().IsLookingRight))
                {
                    transform.root.GetComponent<Animator>().SetInteger("State", 3);
                    transform.root.GetComponent<PlayerActionExecutor>().CounterStun();
                    HitPlayer.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                }
                else
                {
                    HealthModule.Percentage += Damage; 
                    HitPlayer.GetComponent<Animator>().SetInteger("State", 2);
                    HealthModule.ApplyKnockBack(Damage, transform.root.GetComponent<PlayerInputMapper>().Rb);
                    transform.root.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                }
            }
            else if (HealthModule.IsCountering)
            {
                transform.root.GetComponent<Animator>().SetInteger("State", 3);
                transform.root.GetComponent<PlayerActionExecutor>().CounterStun();
                HitPlayer.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;

            }
        }
    }
}
