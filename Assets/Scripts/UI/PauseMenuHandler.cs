using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject m_resumeButton;
    private AudioSource m_audioSource;

    public static PauseMenuHandler Instance;
    #endregion

    #region Methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        m_audioSource = GetComponent<AudioSource>();

        gameObject.SetActive(false);
    }
    public void PauseGame()
    {
        m_audioSource.Play();
        if (gameObject.activeSelf != true)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(m_resumeButton);
        }
        else
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void GoToMainMenuScene()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    #endregion
}