using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject m_resumeButton;
    private AudioSource m_audioSource;
    private Animator m_animator;

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
        m_animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    public void PauseGame()
    {
        gameObject.SetActive(true);
        m_audioSource.Play();
        if (m_animator.GetInteger("State")==0)
        {
            Time.timeScale = 0f;
            m_animator.SetInteger("State", 1);
            EventSystem.current.SetSelectedGameObject(m_resumeButton);
        }
        else
        {
            Time.timeScale = 1f;
            m_animator.SetInteger("State", 0);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void GoToMainMenuScene()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    #endregion
}