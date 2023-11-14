using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    #region Methods
    public void StartTheGame()
    {
        SceneManager.LoadSceneAsync(sceneName: "Level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}