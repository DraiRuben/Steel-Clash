using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int m_percentage = 0;
    private int m_lives = 3;
    public string PlayerName;
    public GameObject Body;
    [SerializeField] private float m_spawnInvulnerabilityDuration = 3f;
    [NonSerialized] public StatsInterfaceHandler m_display;
    [NonSerialized] public bool IsCountering = false;
    [NonSerialized] public bool IsInvulnerable = false;
    private PlayerActionExecutor m_actionExecutor;
    private void Awake()
    {
        m_actionExecutor = GetComponent<PlayerActionExecutor>();
    }

    public int Percentage
    {
        get { return m_percentage; }
        set { m_display.SetCurrentPourcentageTo(value); m_percentage = value; }
    }
    public int Lives
    {
        get { return m_lives; }
        set
        {
            m_display.SetLifeTo(value);
            m_lives = value;
            if (m_lives <= 0)
            {
                PlayerManager.instance.AlivePlayers.Remove(this);
                if (PlayerManager.instance.AlivePlayers.Count == 1)
                {
                    LevelEnd.instance.DisplayWinMessage(PlayerManager.instance.AlivePlayers[0].PlayerName);
                    Time.timeScale = 0f;
                }
            }
        }
    }
    public void MakeInvincible(int _invincible)
    {
        IsInvulnerable = _invincible == 1;
    }
    public void ApplyKnockBack(int _damageTaken, Rigidbody2D _attackingPlayer)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float kbPower = (float)(Mathf.Log(_damageTaken) * m_percentage) / 9f;
        Vector2 kbDir = (transform.position - _attackingPlayer.transform.position).normalized;

        rb.velocity = Vector3.zero;
        rb.AddForce(kbDir * kbPower, ForceMode2D.Impulse);
        m_actionExecutor.HitDirX = Mathf.Sign(kbDir.x);
        m_actionExecutor.HasBeenHit = true;
        m_actionExecutor.Hurt(_damageTaken);

    }
    public void SpawnInvulnerability()
    {
        if (gameObject.activeSelf)
            StartCoroutine(SpawnInvulnerable());
    }
    private IEnumerator SpawnInvulnerable()
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(m_spawnInvulnerabilityDuration);
        IsInvulnerable = false;
    }
}
