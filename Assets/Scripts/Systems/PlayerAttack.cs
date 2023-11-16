using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [NonSerialized] public int Damage;
    [NonSerialized] public bool IsCounter;
    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (!IsCounter && 
            ((!_collider.isTrigger && _collider.CompareTag("Player"))
            ||(_collider.isTrigger && _collider.CompareTag("PlayerAttack") && _collider.GetComponent<PlayerAttack>().IsCounter)))
        {
            Transform _hitPlayer = _collider.transform.root;
            PlayerHealth _healthModule = _hitPlayer.GetComponent<PlayerHealth>();
            Vector3 _collisionPos = _collider.ClosestPoint(transform.position);

            if (!_healthModule.IsInvulnerable)
            {
                if (_healthModule.IsCountering
                    && ((transform.root.position.x < _hitPlayer.position.x && !_hitPlayer.GetComponent<PlayerInputMapper>().IsLookingRight)
                    || transform.root.position.x > _hitPlayer.position.x && _hitPlayer.GetComponent<PlayerInputMapper>().IsLookingRight))
                {
                    transform.root.GetComponent<Animator>().SetInteger("State", 3);
                    transform.root.GetComponent<PlayerActionExecutor>().CounterStun();
                    _hitPlayer.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                    HitFeedbackManager.Instance.DisplayHit(_collisionPos + Vector3.back, HitFeedbackManager.HitType.Counter);
                }
                else
                {
                    _healthModule.Percentage += Damage;
                    _hitPlayer.GetComponent<Animator>().SetInteger("State", 2);
                    _healthModule.ApplyKnockBack(Damage, transform.root.GetComponent<PlayerInputMapper>().Rb);
                    transform.root.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                    HitFeedbackManager.Instance.DisplayHit(_collisionPos + Vector3.back, HitFeedbackManager.HitType.Hit);

                }
            }
            else if (_healthModule.IsCountering)
            {
                transform.root.GetComponent<Animator>().SetInteger("State", 3);
                transform.root.GetComponent<PlayerActionExecutor>().CounterStun();
                _hitPlayer.GetComponent<PlayerAnimationManager>().ReduceRecovery = true;
                HitFeedbackManager.Instance.DisplayHit(_collisionPos + Vector3.back, HitFeedbackManager.HitType.Counter);

            }
        }
    }
}
