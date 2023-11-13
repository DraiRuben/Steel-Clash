using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public List<PlatformManager> Platforms = new();
    public static PlayerManager instance;
    private PlayerInputManager inputManager;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        inputManager = GetComponent<PlayerInputManager>();
    }
    public void OnPlayerJoined(PlayerInput input)
    {
        //create percent UI, life UI and rearrange UI of every player
        //Caution: only call the function from the UI component that does this, don't do the code in this thing
        foreach(var p in Platforms)
        {
            p.GenerateCollision(inputManager.playerCount,input.GetComponent<PlayerActionExecutor>().m_feet);
        }
    }
    public void OnPlayerLeft(PlayerInput input)
    {
        //nothing as of now, however we might need to display a disconnected icon on the UI of the concerned player
    }
}
