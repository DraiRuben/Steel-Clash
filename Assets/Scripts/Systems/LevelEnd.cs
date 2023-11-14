using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_winMessage;

    public static LevelEnd instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void DisplayWinMessage(string _winner)
    {
        gameObject.SetActive(true);
        m_winMessage.text = _winner + " Won !";
    }
}
