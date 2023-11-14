using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerManager : MonoBehaviour
{
    #region Variables
    List<Color> _colorList;

    

    [SerializeField] GameObject _statsInterfacePrefab;
    [SerializeField] GameObject _statsInterfacesParent;
    #endregion

    #region Methods

    private void Awake()
    {
        // Creating a list of color to asign on each player
        _colorList = new List<Color>
        {
            new Color(175f / 255f, 0, 0), // Red
            new Color(175f / 255f, 175f / 255f, 0), // Yellow
            new Color(0, 175f / 255f, 0), // Green
            new Color(0, 80f / 255f, 175f / 255f), // Blue
        };
    }

    private void Start()
    {
        // POUR TEST :

        GameObject _statsInterface = Instantiate(_statsInterfacePrefab, _statsInterfacesParent.transform);

        StatsInterfaceHandler _statsInterfaceHandler = _statsInterface.GetComponent<StatsInterfaceHandler>();

        _statsInterfaceHandler.SetIDTo(1, _colorList);
        _statsInterfaceHandler.SetCurrentPourcentageTo(266);

        _statsInterface = Instantiate(_statsInterfacePrefab, _statsInterfacesParent.transform);

        _statsInterfaceHandler = _statsInterface.GetComponent<StatsInterfaceHandler>();

        _statsInterfaceHandler.SetIDTo(2, _colorList);
        _statsInterfaceHandler.SetCurrentPourcentageTo(0);
        _statsInterfaceHandler.SetLifeTo(0);

        _statsInterface = Instantiate(_statsInterfacePrefab, _statsInterfacesParent.transform);

         _statsInterfaceHandler = _statsInterface.GetComponent<StatsInterfaceHandler>();

        _statsInterfaceHandler.SetIDTo(3, _colorList);
        _statsInterfaceHandler.SetCurrentPourcentageTo(54);
        _statsInterfaceHandler.SetLifeTo(1);

        _statsInterface = Instantiate(_statsInterfacePrefab, _statsInterfacesParent.transform);

        _statsInterfaceHandler = _statsInterface.GetComponent<StatsInterfaceHandler>();

        _statsInterfaceHandler.SetIDTo(4, _colorList);
        _statsInterfaceHandler.SetCurrentPourcentageTo(126);
        _statsInterfaceHandler.SetLifeTo(2);
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        // TO DO : Replace NewNumberOfPlayer by the current number of player connected
        int NewNumberOfPlayer = 0;

        GameObject _statsInterface = Instantiate(_statsInterfacePrefab, _statsInterfacesParent.transform);

        StatsInterfaceHandler _statsInterfaceHandler = _statsInterface.GetComponent<StatsInterfaceHandler>();

        _statsInterfaceHandler.SetIDTo(NewNumberOfPlayer, _colorList);
        _statsInterfaceHandler.SetCurrentPourcentageTo(0);
    }
    public void OnPlayerLeft(PlayerInput input)
    {
        

        //nothing as of now, however we might need to display a disconnected icon on the UI of the concerned player
    }
    #endregion
}