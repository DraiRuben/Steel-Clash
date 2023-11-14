using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject _resumeButton;

    public static PauseMenuHandler Instance;
    #endregion

    #region Methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        gameObject.SetActive(false);
    }
    public void PauseGame()
    {
        if (gameObject.activeSelf != true)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(_resumeButton);
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