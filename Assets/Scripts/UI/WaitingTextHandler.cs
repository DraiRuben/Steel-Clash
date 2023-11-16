using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaitingTextHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject _text;
    [SerializeField] GameObject _textOutline;

    [Header("Customizable values")]
    [SerializeField] float _timeBetweenBlinking;
    #endregion

    #region
    void Start()
    {   
        StartCoroutine(BlinkingTheText(_text, _textOutline, _timeBetweenBlinking));
    }

    IEnumerator BlinkingTheText(GameObject text, GameObject textOutline, float timeBetweenBlinking)
    {
        while (PlayerInputManager.instance.playerCount < 2)
        {
            if (PauseMenuHandler.Instance.gameObject.activeSelf == false)
            {
                text.SetActive(true);
                textOutline.SetActive(true);

                yield return new WaitForSecondsRealtime(timeBetweenBlinking);

                text.SetActive(false);
                textOutline.SetActive(false);

                yield return new WaitForSecondsRealtime(timeBetweenBlinking);
            }
            else
            {
                text.SetActive(false);
                textOutline.SetActive(false);

                yield return new WaitForSecondsRealtime(timeBetweenBlinking);
            }
        }
    }
    #endregion
}