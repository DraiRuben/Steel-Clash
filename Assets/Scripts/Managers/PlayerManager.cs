using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    #region Variables
    public static PlayerManager Instance;

    private List<Color> m_colorList;
    private PlayerInputManager m_inputManager;
    [NonSerialized] public List<GameObject> Players = new();
    [NonSerialized] public List<PlayerHealth> AlivePlayers = new();
    [NonSerialized] public List<PlatformManager> Platforms = new();
    [SerializeField] private GameObject m_statsInterfacePrefab;
    [SerializeField] private GameObject m_statsInterfacesParent;
    #endregion

    #region Methods

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        m_inputManager = GetComponent<PlayerInputManager>();
        // Creating a list of color to asign on each player
        m_colorList = new List<Color>
        {
            new Color(175f / 255f, 0, 0), // Red
            new Color(175f / 255f, 175f / 255f, 0), // Yellow
            new Color(0, 175f / 255f, 0), // Green
            new Color(0, 80f / 255f, 175f / 255f), // Blue
        };
    }

    public void OnPlayerJoined(PlayerInput _input)
    {
        GameObject _statsInterface = Instantiate(m_statsInterfacePrefab, m_statsInterfacesParent.transform);

        StatsInterfaceHandler _statsInterfaceHandler = _statsInterface.GetComponent<StatsInterfaceHandler>();
        Players.Add(_input.gameObject);
        _statsInterfaceHandler.SetIDTo(m_inputManager.playerCount, m_colorList);
        _statsInterfaceHandler.SetCurrentPourcentageTo(0);

        PlayerHealth _health = _input.GetComponent<PlayerHealth>();
        _health.m_display = _statsInterfaceHandler;
        _health.Body.layer = LayerMask.NameToLayer("Player" + m_inputManager.playerCount);
        _health.PlayerName = "Player " + m_inputManager.playerCount;
        AlivePlayers.Add(_health);
        PlayerFeet _playerFeet = _input.GetComponent<PlayerActionExecutor>().Feet;
        _playerFeet.gameObject.layer = LayerMask.NameToLayer("Player" + m_inputManager.playerCount);
        
        foreach (PlatformManager p in Platforms)
        {
            p.GenerateCollision(m_inputManager.playerCount);
        }
        SpawnManager.Instance.PutPlayerAtSpawnPoint(m_inputManager.playerCount, _input.gameObject);
    }
    public void OnPlayerLeft(PlayerInput _input)
    {
        //nothing as of now, however we might want to display a disconnected icon on the UI of the concerned player
    }
    #endregion
}