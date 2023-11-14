using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    private float m_percentage = 0f;
    [SerializeField] private float m_spawnInvulnerabilityDuration = 3f;
    [HideInInspector] public bool IsCountering = false;
    [HideInInspector] public bool IsInvulnerable = false;
    [HideInInspector] public UnityEvent OnDamageTaken = new();

    public float Percentage { get { return m_percentage; } set { OnDamageTaken.Invoke(); m_percentage = value; } }

    public void ApplyKnockBack(float DamageTaken,Rigidbody2D AttackingPlayer)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
        float kbPower = Mathf.Log(DamageTaken) * m_percentage / 10;
        Vector3 kbDir = AttackingPlayer.velocity.normalized;
        if(kbDir.magnitude==0f)
        {
            kbDir = (transform.position- AttackingPlayer.transform.position).normalized + Vector3.up;
        }
        rb.AddForce(kbDir * kbPower,ForceMode2D.Impulse);

    }
    public void SpawnInvulnerability()
    {
        StartCoroutine(SpawnInvulnerable());
    }
    private IEnumerator SpawnInvulnerable()
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(m_spawnInvulnerabilityDuration);
        IsInvulnerable = false;
    }
}
