using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [NonSerialized] public int Damage;
    [NonSerialized] public bool IsCounter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsCounter && 
            ((!collision.isTrigger && collision.CompareTag("Player"))
            ||(collision.isTrigger && collision.CompareTag("PlayerAttack") && collision.GetComponent<PlayerAttack>().IsCounter)))
        {
            Transform HitPlayer = collision.transform.root;
            PlayerHealth HealthModule = HitPlayer.GetComponent<PlayerHealth>();
            Vector3 CollisionPos = collision.ClosestPoint(transform.position);

            if (!HealthModule.IsInvulnerable)
            {
                if (HealthModule.IsCountering
                    && ((transform.root.position.x < HitPlayer.position.x && !HitPlayer.GetComponent<PlayerInputMapper>().IsLookingRight)
                    || transform.root.position.x > HitPlayer.position.x && HitPlayer.GetComponent<PlayerInputMapper>().IsLookingRight))
                {
                    transform.root.GetComponent<Animator>().SetInteger("State", 3);
                    transform.root.GetComponent<PlayerActionExecutor>().CounterStun();
                    HitPlayer.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                    HitFeedbackManager.instance.DisplayHit(CollisionPos + Vector3.back, HitFeedbackManager.HitType.Counter);
                }
                else
                {
                    HealthModule.Percentage += Damage;
                    HitPlayer.GetComponent<Animator>().SetInteger("State", 2);
                    HealthModule.ApplyKnockBack(Damage, transform.root.GetComponent<PlayerInputMapper>().Rb);
                    transform.root.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                    HitFeedbackManager.instance.DisplayHit(CollisionPos + Vector3.back, HitFeedbackManager.HitType.Hit);

                }
            }
            else if (HealthModule.IsCountering)
            {
                transform.root.GetComponent<Animator>().SetInteger("State", 3);
                transform.root.GetComponent<PlayerActionExecutor>().CounterStun();
                HitPlayer.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                HitFeedbackManager.instance.DisplayHit(CollisionPos + Vector3.back, HitFeedbackManager.HitType.Counter);

            }
        }
    }
}
