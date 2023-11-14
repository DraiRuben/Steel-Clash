using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    private int m_percentage = 0;
    private int m_lives=3;
    public GameObject Body;
    [SerializeField] private float m_spawnInvulnerabilityDuration = 3f;
    [HideInInspector] public StatsInterfaceHandler m_display;
    [HideInInspector] public bool IsCountering = false;
    [HideInInspector] public bool IsInvulnerable = false;

    public int Percentage { get { return m_percentage; } set { m_display.SetCurrentPourcentageTo(value); m_percentage = value; } }
    public int Lives { get { return m_lives; } set { m_display.SetLifeTo(value); m_lives = value; } }

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
