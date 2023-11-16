using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaitingTextHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject m_text;
    [SerializeField] private GameObject m_textOutline;

    [Header("Customizable values")]
    [SerializeField] private float m_timeBetweenBlinking;
    #endregion

    #region
    void Start()
    {   
        StartCoroutine(BlinkingTheText(m_text, m_textOutline, m_timeBetweenBlinking));
    }

    IEnumerator BlinkingTheText(GameObject _text, GameObject _textOutline, float _timeBetweenBlinking)
    {
        while (PlayerInputManager.instance.playerCount < 2)
        {
            if (PauseMenuHandler.Instance.gameObject.activeSelf == false)
            {
                _text.SetActive(true);
                _textOutline.SetActive(true);

                yield return new WaitForSecondsRealtime(_timeBetweenBlinking);

                _text.SetActive(false);
                _textOutline.SetActive(false);

                yield return new WaitForSecondsRealtime(_timeBetweenBlinking);
            }
            else
            {
                _text.SetActive(false);
                _textOutline.SetActive(false);

                yield return new WaitForSecondsRealtime(_timeBetweenBlinking);
            }
        }
    }
    #endregion
}