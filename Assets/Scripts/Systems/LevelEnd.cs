using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_winMessage;
    [SerializeField] private Button m_restartButton;

    public static LevelEnd instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        gameObject.SetActive(false);
    }
    public void DisplayWinMessage(string _winner)
    {
        EventSystem.current.SetSelectedGameObject(m_restartButton.gameObject);
        gameObject.SetActive(true);
        m_winMessage.text = _winner + " Wins !";
    }
}
