using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFeedbackManager : MonoBehaviour
{
    [SerializeField] private GameObject m_hitParticleSystem;
    [SerializeField] private GameObject m_counterParticleSystem;
    public static HitFeedbackManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void DisplayHit(Vector3 _hitPos,HitType _type)
    {
        if (_type == HitType.Hit)
            Instantiate(m_hitParticleSystem, _hitPos, Quaternion.identity);
        else if( _type == HitType.Counter)
            Instantiate(m_counterParticleSystem, _hitPos, Quaternion.identity);
    }
    public enum HitType
    {
        Hit,
        Counter
    }
}
