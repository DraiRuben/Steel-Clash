using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    #region Variables
    List<Color> _colorList;
    public static PlayerManager instance;
    private PlayerInputManager inputManager;
    [NonSerialized] public List<GameObject> Players = new();
    [NonSerialized] public List<PlayerHealth> AlivePlayers = new();
    [NonSerialized] public List<PlatformManager> Platforms = new();
    [SerializeField] GameObject _statsInterfacePrefab;
    [SerializeField] GameObject _statsInterfacesParent;
    #endregion

    #region Methods

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        inputManager = GetComponent<PlayerInputManager>();
        // Creating a list of color to asign on each player
        _colorList = new List<Color>
        {
            new Color(175f / 255f, 0, 0), // Red
            new Color(175f / 255f, 175f / 255f, 0), // Yellow
            new Color(0, 175f / 255f, 0), // Green
            new Color(0, 80f / 255f, 175f / 255f), // Blue
        };
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        GameObject _statsInterface = Instantiate(_statsInterfacePrefab, _statsInterfacesParent.transform);

        StatsInterfaceHandler _statsInterfaceHandler = _statsInterface.GetComponent<StatsInterfaceHandler>();
        Players.Add(input.gameObject);
        _statsInterfaceHandler.SetIDTo(inputManager.playerCount, _colorList);
        _statsInterfaceHandler.SetCurrentPourcentageTo(0);

        PlayerHealth Health = input.GetComponent<PlayerHealth>();
        Health.m_display = _statsInterfaceHandler;
        Health.Body.layer = LayerMask.NameToLayer($"Player{inputManager.playerCount}");
        Health.PlayerName = "Player " + inputManager.playerCount;
        AlivePlayers.Add(Health);
        PlayerFeet playerFeet = input.GetComponent<PlayerActionExecutor>().m_feet;
        playerFeet.gameObject.layer = LayerMask.NameToLayer("Player" + inputManager.playerCount);
        
        foreach (PlatformManager p in Platforms)
        {
            p.GenerateCollision(inputManager.playerCount, playerFeet);
        }
        SpawnManager.instance.PutPlayerAtSpawnPoint(inputManager.playerCount, input.gameObject);
    }
    public void OnPlayerLeft(PlayerInput input)
    {
        //nothing as of now, however we might want to display a disconnected icon on the UI of the concerned player
    }
    #endregion
}