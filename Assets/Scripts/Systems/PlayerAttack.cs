using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public int Damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.isTrigger && collision.CompareTag("Player"))
        {
            var HealthModule = collision.transform.root.GetComponent<PlayerHealth>();
            if (!HealthModule.IsInvulnerable)
            {
                if (HealthModule.IsCountering)
                {
                    transform.root.GetComponent<Animator>().SetInteger("State", 3);
                }
                else
                {
                    HealthModule.Percentage += Damage; 
                    collision.transform.root.GetComponent<Animator>().SetInteger("State", 2);
                    HealthModule.ApplyKnockBack(Damage, transform.root.GetComponent<PlayerInputMapper>().Rb);
                    collision.transform.root.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                }
            }
            else if (HealthModule.IsCountering)
            {
                transform.root.GetComponent<Animator>().SetInteger("State", 3);
            }
        }
    }
}
