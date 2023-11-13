using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    private float m_percentage = 0f;
    [HideInInspector] public bool IsCountering = false;
    [HideInInspector] public bool IsInvulnerable = false;
    public float Percentage { get { return m_percentage; } set { OnPercentageChange.Invoke(); m_percentage = value; } }
    public UnityEvent OnPercentageChange = new();
}
