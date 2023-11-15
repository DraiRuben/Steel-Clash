using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerHealth : MonoBehaviour
{
    private int m_percentage = 0;
    private int m_lives=3;
    public string PlayerName;
    public GameObject Body;
    [SerializeField] private float m_spawnInvulnerabilityDuration = 3f;
    [HideInInspector] public StatsInterfaceHandler m_display;
    [HideInInspector] public bool IsCountering = false;
    [HideInInspector] public bool IsInvulnerable = false;
    private PlayerActionExecutor m_actionExecutor;
    private void Awake()
    {
        m_actionExecutor = GetComponent<PlayerActionExecutor>();
    }

    public int Percentage { get { return m_percentage; }
        set { m_display.SetCurrentPourcentageTo(value); m_percentage = value; } }
    public int Lives { get { return m_lives; } 
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

    public void ApplyKnockBack(int _damageTaken,Rigidbody2D _attackingPlayer)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float kbPower = Mathf.Log(_damageTaken) * m_percentage / 10;
        Vector3 kbDir = _attackingPlayer.velocity.normalized;

        //if player isn't moving or if the player is getting pushed by the attacked player
        if (kbDir.magnitude == 0f || IsInCone(45f,rb.velocity.normalized,kbDir))
        {
            kbDir = (transform.position- _attackingPlayer.transform.position).normalized + Vector3.up;
            Debug.Log("adjusted");
        }
        rb.velocity = Vector3.zero;
        rb.AddForce(kbDir * kbPower,ForceMode2D.Impulse);
        m_actionExecutor.Hurt(_damageTaken);

    }
    private bool IsInCone(float _angle, Vector3 _bisector, Vector3 _toTest)
    {
        float dotProduct = Vector3.Dot(_bisector, _toTest);
        float angleBetween = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

        return angleBetween <= _angle;
    }
    public void SpawnInvulnerability()
    {
        if(gameObject.activeSelf)
            StartCoroutine(SpawnInvulnerable());
    }
    private IEnumerator SpawnInvulnerable()
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(m_spawnInvulnerabilityDuration);
        IsInvulnerable = false;
    }
}
