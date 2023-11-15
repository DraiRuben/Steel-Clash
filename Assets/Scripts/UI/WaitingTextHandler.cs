using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaitingTextHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject _text;
    [SerializeField] GameObject _textOutline;

    [Header("Customizable values")]
    [SerializeField] float _timeBetweenBlinking;
    [SerializeField] string _waitingTextFirstPlayer;
    [SerializeField] string _waitingTextSecondPlayer;
    #endregion

    #region
    void Start()
    {
        StartCoroutine(BlinkingTheText(_text, _textOutline, _timeBetweenBlinking));
    }

    IEnumerator BlinkingTheText(GameObject text, GameObject textOutline, float timeBetweenBlinking)
    {
        while (PlayerInputManager.instance.playerCount == 1)
        {
            text.SetActive(true); 
            textOutline.SetActive(true);

            yield return new WaitForSecondsRealtime(timeBetweenBlinking);

            text.SetActive(false); 
            textOutline.SetActive(false);

            yield return new WaitForSecondsRealtime(timeBetweenBlinking);
        }
    }
    #endregion
}