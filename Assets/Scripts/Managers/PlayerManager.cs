using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    public void OnPlayerJoined(PlayerInput input)
    {
        //create percent UI, life UI and rearrange UI of every player
        //Caution: only call the function from the UI component that does this, don't do the code in this thing
    }
    public void OnPlayerLeft(PlayerInput input)
    {
        //nothing as of now, however we might need to display a disconnected icon on the UI of the concerned player
    }
}
