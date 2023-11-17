using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public static DynamicCamera Instance;
    [SerializeField] private CinemachineVirtualCamera m_cam;
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private float m_minZoom = 4f;
    [SerializeField] private float m_maxZoom = 17f;
    [SerializeField] private float m_zoomLimiter = 50f;
    private CinemachineConfiner2D m_confiner;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        m_confiner = m_cam.GetComponent<CinemachineConfiner2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.AlivePlayers.Count <= 0) return;

        Move();
        Zoom();
    }
    private void Move()
    {
        Vector3 CenterPoint = GetCenterPoint();
        Vector3 NewPosition = CenterPoint + m_offset;
        transform.position = NewPosition;
    }
    private void Zoom()
    {
        float newZoom = Mathf.Lerp(m_minZoom,m_maxZoom, GetGreatestDistance()/m_zoomLimiter);
        m_cam.m_Lens.OrthographicSize = Mathf.Lerp(m_cam.m_Lens.OrthographicSize, newZoom, 2f*Time.deltaTime);
        m_confiner.InvalidateCache();
    }
    private float GetGreatestDistance()
    {
        var bounds = new Bounds(PlayerManager.Instance.AlivePlayers[0].transform.position, Vector3.zero);
        for (int i = 0; i < PlayerManager.Instance.AlivePlayers.Count; i++)
        {
            bounds.Encapsulate(PlayerManager.Instance.AlivePlayers[i].transform.position);
        }
        return bounds.size.x;
    }
    private Vector3 GetCenterPoint()
    {
        if(PlayerManager.Instance.AlivePlayers.Count == 1)
        {
            return PlayerManager.Instance.AlivePlayers[0].transform.position;
        }
        var bounds = new Bounds(PlayerManager.Instance.AlivePlayers[0].transform.position, Vector3.zero);
        for(int i = 0;i< PlayerManager.Instance.AlivePlayers.Count;i++)
        {
            bounds.Encapsulate(PlayerManager.Instance.AlivePlayers[i].transform.position);
        }
        return bounds.center;
    }
}
